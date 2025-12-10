namespace AdventOfCode;

public class Day10(string input)
{
    public void Task1()
    {
        var machines = ParseInput();
        var sum = 0L;
        foreach (var machine in machines)
        {
            sum += machine.DepthFirstSearch();
        }
        
        Console.WriteLine($"To set every machine in its desired state, the buttons must be pressed a total of {sum} times.");
    }

    public void Task2()
    {
        var machines = ParseInput();
        var sum = 0L;
 
        foreach (var machine in machines)
        {
            sum += machine.FindMinimalPressesUsingGaussianElimination();
        }
        
        Console.WriteLine($"To set every machine in its desired state, the buttons must be pressed a total of {sum} times.");
    }

    private ICollection<Machine> ParseInput()
    {
        return input.Split(['\r', '\n'], StringSplitOptions.RemoveEmptyEntries)
            .Select(Machine.FromDescription)
            .ToArray();
    }

    private class Machine
    {
        private long _desiredIndicatorState;
        private readonly List<Button> _buttons = new();
        private readonly List<int> _joltages = new();

        private Machine(long state, List<Button> buttons, List<int> joltages)
        {
            _desiredIndicatorState = state;
            _buttons = buttons;
            _joltages = joltages;
        }

        public int DepthFirstSearch()
        {
            var initialState = 0L;
            var visitedStates = new Dictionary<long, int>();
            var stack = new Stack<(long State, int Presses)>();
            stack.Push((initialState, 0));

            var minPresses = int.MaxValue;

            while (stack.Count > 0)
            {
                var (currentState, presses) = stack.Pop();
                if (presses >= minPresses)
                {
                    continue;
                }

                if (currentState == _desiredIndicatorState)
                {
                    minPresses = Math.Min(minPresses, presses);
                    continue;
                }

                foreach (var button in _buttons)
                {
                    var newState = currentState ^ button.State;
                    var newPresses = presses + 1;
                    if (visitedStates.TryGetValue(newState, out var existingPresses) && existingPresses <= newPresses)
                    {
                        continue;
                    }

                    visitedStates[newState] = newPresses;
                    stack.Push((newState, newPresses));
                }
            }

            return minPresses;
        }

        public static Machine FromDescription(string description)
        {
            var indicatorStateEnd = description.IndexOf(']', StringComparison.Ordinal);
            var indicators = description[1..indicatorStateEnd];
            var desiredIndicatorState = 0L;
            for (var i = 0; i < indicators.Length; i++)
            {
                if (indicators[i] == '#')
                {
                    desiredIndicatorState |= 1L << i;
                }
            }

            var buttonDescriptions =
                description[(indicatorStateEnd + 2)..description.IndexOf('{', StringComparison.Ordinal)];
            var buttons = buttonDescriptions.Split(' ', StringSplitOptions.RemoveEmptyEntries)
                .Select(buttonDescription => buttonDescription.Trim(' ', '(', ')')
                    .Split(',', StringSplitOptions.RemoveEmptyEntries)
                    .Select(int.Parse)
                    .ToArray())
                .Select(toggles => new Button(toggles))
                .ToList();

            var joltageDescriptions = description[(description.IndexOf('{', StringComparison.Ordinal) + 1)..^1];
            var joltages = joltageDescriptions
                .Split(',')
                .Select(int.Parse)
                .ToList();

            return new Machine(desiredIndicatorState, buttons, joltages);
        }

        public long FindMinimalPressesUsingGaussianElimination()
        {
            var joltages = _joltages.Count;
            var buttons = _buttons.Count;

            var matrix = new int[joltages, buttons + 1];

            for (var eq = 0; eq < joltages; eq++)
            {
                for (var btn = 0; btn < buttons; btn++)
                {
                    if (_buttons[btn].IndicatorToggles.Contains(eq))
                    {
                        matrix[eq, btn] = 1;
                    }
                }

                matrix[eq, buttons] = _joltages[eq];
            }

            var solution = SolveGaussianElimination(matrix, buttons, joltages);

            if (solution != null)
            {
                return solution.Sum();
            }

            return 0;
        }

        private int[]? SolveGaussianElimination(int[,] matrix, int buttons, int joltages)
        {
            var pivotCols = new List<int>();
            var currentRow = 0;

            // Forward elimination
            for (var col = 0; col < buttons && currentRow < joltages; col++)
            {
                // Find pivot
                var pivotRow = -1;
                for (var row = currentRow; row < joltages; row++)
                {
                    if (matrix[row, col] != 0)
                    {
                        pivotRow = row;
                        break;
                    }
                }

                if (pivotRow == -1)
                {
                    continue;
                }

                // Swap rows
                if (pivotRow != currentRow)
                {
                    for (var j = 0; j <= buttons; j++)
                    {
                        (matrix[currentRow, j], matrix[pivotRow, j]) = (matrix[pivotRow, j], matrix[currentRow, j]);
                    }
                }

                pivotCols.Add(col);

                // Eliminate below
                for (var row = currentRow + 1; row < joltages; row++)
                {
                    if (matrix[row, col] != 0)
                    {
                        var factor = matrix[row, col];
                        var pivotVal = matrix[currentRow, col];

                        for (var j = col; j <= buttons; j++)
                        {
                            matrix[row, j] = matrix[row, j] * pivotVal - matrix[currentRow, j] * factor;
                        }

                        // Reduce by GCD to prevent coefficient explosion
                        var gcd = 0;
                        for (var j = col; j <= buttons; j++)
                        {
                            if (matrix[row, j] != 0)
                            {
                                gcd = gcd == 0 ? Math.Abs(matrix[row, j]) : GCD(gcd, Math.Abs(matrix[row, j]));
                            }
                        }

                        if (gcd > 1)
                        {
                            for (var j = col; j <= buttons; j++)
                            {
                                matrix[row, j] /= gcd;
                            }
                        }
                    }
                }

                currentRow++;
            }

            // Find free variables
            var pivotSet = new HashSet<int>(pivotCols);
            var freeVars = new List<int>();
            for (var i = 0; i < buttons; i++)
            {
                if (!pivotSet.Contains(i))
                {
                    freeVars.Add(i);
                }
            }

            // No free variables - unique solution or no solution
            if (freeVars.Count == 0)
            {
                return TrySolution(matrix, buttons, joltages, pivotCols, freeVars, []);
            }

            int[]? bestSolution = null;
            var bestSum = int.MaxValue;

            // Compute search bounds based on free variable count
            var maxVal = freeVars.Count switch
            {
                1 => Math.Max(matrix[0, buttons] * 3, 2000),
                2 => ComputeMaxVal(matrix, pivotCols),
                3 => 500,
                4 => 200,
                5 => 100,
                _ => 0
            };

            if (freeVars.Count > 5)
            {
                return TrySolution(matrix, buttons, joltages, pivotCols, freeVars, new int[freeVars.Count]);
            }

            var current = new int[freeVars.Count];
            SearchFreeVars(0);

            return bestSolution;

            void SearchFreeVars(int depth)
            {
                if (depth == freeVars.Count)
                {
                    var solution = TrySolution(matrix, buttons, joltages, pivotCols, freeVars, current);
                    if (solution != null)
                    {
                        var totalPresses = solution.Sum();
                        if (totalPresses < bestSum)
                        {
                            bestSum = totalPresses;
                            bestSolution = solution;
                        }
                    }

                    return;
                }

                var limit = freeVars.Count == 1 ? maxVal + 1 : maxVal;

                // Pruning: skip if current partial sum already exceeds best
                var partialSum = 0;
                for (var i = 0; i < depth; i++)
                {
                    partialSum += current[i];
                }

                for (var v = 0; v < limit; v++)
                {
                    // Early exit if partial sum exceeds best
                    if (partialSum + v >= bestSum)
                    {
                        break;
                    }

                    current[depth] = v;
                    SearchFreeVars(depth + 1);
                }
            }

            static int ComputeMaxVal(int[,] matrix, List<int> pivotCols)
            {
                var maxVal = 0;
                var buttons = matrix.GetLength(1) - 1;
                for (var row = 0; row < pivotCols.Count; row++)
                {
                    maxVal = Math.Max(maxVal, Math.Abs(matrix[row, buttons]));
                }

                return Math.Min(Math.Max(maxVal * 2, 500), 2000);
            }
        }

        static int[]? TrySolution(int[,] matrix, int buttons, int joltages, List<int> pivotCols, List<int> freeVars,
            int[] freeValues)
        {
            var solution = new int[buttons];

            // Set free variables
            for (var i = 0; i < freeVars.Count; i++)
            {
                solution[freeVars[i]] = freeValues[i];
            }

            // Back substitution for pivot variables
            for (var i = pivotCols.Count - 1; i >= 0; i--)
            {
                var row = i;
                var col = pivotCols[i];
                var total = matrix[row, buttons];

                for (var j = col + 1; j < buttons; j++)
                {
                    total -= matrix[row, j] * solution[j];
                }

                if (matrix[row, col] == 0 || total % matrix[row, col] != 0)
                    return null;

                var val = total / matrix[row, col];
                if (val < 0) return null;

                solution[col] = val;
            }

            // Verify solution
            for (var i = 0; i < joltages; i++)
            {
                var sum = 0;
                for (var j = 0; j < buttons; j++)
                {
                    sum += matrix[i, j] * solution[j];
                }

                if (sum != matrix[i, buttons]) return null;
            }

            return solution;
        }

        static int GCD(int a, int b)
        {
            while (b != 0)
            {
                var temp = b;
                b = a % b;
                a = temp;
            }

            return a;
        }
    }

    private record Button(IReadOnlyCollection<int> IndicatorToggles)
    {
        public long State => IndicatorToggles.Aggregate(0L, (current, toggle) => current | 1L << toggle);
    }
}