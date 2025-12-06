namespace AdventOfCode;

public class Day6(string input)
{
    public void Task1()
    {
        var problems = ReadProblems();
        var grandTotal = 0L;
        Parallel.ForEach(problems, problem =>
        {
            var result = problem.Calculate();
            Interlocked.Add(ref grandTotal, result);
        });
        
        Console.WriteLine($"The grand total is {grandTotal}");
    }

    public void Task2()
    {
        var problems = ReadProblemsRtlColumns();
        var grandTotal = 0L;
        Parallel.ForEach(problems, problem =>
        {
            var result = problem.Calculate();
            Interlocked.Add(ref grandTotal, result);
        });
        
        Console.WriteLine($"The grand total is {grandTotal}");
    }

    private List<MathProblem> ReadProblems()
    {
        var lines = input.Split(['\r', '\n'], StringSplitOptions.RemoveEmptyEntries);
        var itemCount = lines.Last().Count(x => x != ' ');
        var problems = new List<MathProblem>(itemCount);
        for (var i = 0; i < itemCount; i++)
        {
            problems.Add(new MathProblem());
        }
        
        foreach (var line in lines)
        {
            var x = 0;
            foreach (var item in line.Split(' ', StringSplitOptions.RemoveEmptyEntries))
            {
                if (item[0] != '+' && item[0] != '*')
                {
                    problems[x].Add(int.Parse(item));
                }
                else
                {
                    problems[x].Operand = item[0];
                }
                x++;
            }
        }
        
        return problems;
    }
    
    private List<MathProblem> ReadProblemsRtlColumns()
    {
        var lines = input.Split(['\r', '\n'], StringSplitOptions.RemoveEmptyEntries);
        var itemCount = lines.Last().Count(x => x != ' ');
        var problems = new List<MathProblem>(itemCount);
        for (var i = 0; i < itemCount; i++)
        {
            problems.Add(new MathProblem());
        }

        var x = 0;
        for (var i = 0; i < lines[0].Length; i++)
        {
            if (lines.All(line => line[i] == ' '))
            {
                x++;
                continue;
            }
            
            var number = 0;
            foreach (var line in lines)
            {
                switch (line[i])
                {
                    case ' ':
                        break;
                    case '+':
                    case '*':
                        problems[x].Operand += line[i];
                        break;
                    case var c when char.IsDigit(c):
                        number *= 10;
                        number += c - '0';
                        break;
                }
            }

            if (number > 0)
            {
                problems[x].Add(number);
            }
        }
        
        return problems;
    }
    
    private class MathProblem : List<int>
    {
        public char Operand { get; set; }
        
        public long Calculate()
        {
            long result = this[0];
            for (var i = 1; i < Count; i++)
            {
                switch (Operand)
                {
                    case '+':
                        result += this[i];
                        break;
                    case '*':
                        result *= this[i];
                        break;
                }
            }

            return result;
        }
    }
}