namespace AdventOfCode;

public class Day12(string input)
{
    public void Task1()
    {
        var (presents, trees) = ParseInput();
        var fit = trees.Where(instruction =>
        {
            var area = instruction.Size.X * instruction.Size.Y;
            int[] shapeAreas =
            [
                .. instruction.RequiredShapes.Keys.Select(i =>
                    presents[i].Area * instruction.RequiredShapes[i]
                ),
            ];
            return shapeAreas.Sum() < area;
        });
        
        Console.WriteLine($"{fit.Count()} regions can fit the presents");
    }

    private (IReadOnlyList<Present>, IReadOnlyList<Tree>) ParseInput()
    {
        var lines = input.Split('\r', '\n');
        var shapeInput = lines.Take(30);
        var packInput = lines.Skip(30);

        List<Present> presents = [];

        var i = 0;
        while (shapeInput.Any())
        {
            presents.Add(new Present(i++, shapeInput.Take(5)));
            shapeInput = shapeInput.Skip(5);
        }

        Tree[] trees =
        [
            .. packInput
                .Select(line => line.Split(' '))
                .Select(parts =>
                {
                    var areaParts = string.Join("", parts.First().SkipLast(1)).Split('x');
                    var requiredShapesPart = parts.Skip(1);

                    (int x, int y) size = (x: int.Parse(areaParts[0]), int.Parse(areaParts[1]));
                    int[] requiredShapes = [.. requiredShapesPart.Select(int.Parse)];
                    return new Tree(size, requiredShapes);
                }),
        ];

        return (presents, trees);
    }
    
    public class Present
    {
        private readonly bool[][] _contents;

        public Present(int index, IEnumerable<string> input)
        {
            _contents =
            [
                .. input
                    .Skip(1)
                    .TakeWhile(line => !string.IsNullOrWhiteSpace(line))
                    .Select(line => line.Select(c => c == '#').ToArray()),
            ];
            Index = index;
        }
        
        public int Index { get; }

        public int Area => _contents.SelectMany(row => row).Count(b => b);
    }

    private class Tree
    {
        public Tree((int X, int Y) size, int[] requiredShapes)
        {
            Size = size;
            RequiredShapes = requiredShapes
                .Select((n, i) => (n, i))
                .ToDictionary(nAndIndex => nAndIndex.i, nAndIndex => nAndIndex.n);
        }
        
        public (int X, int Y) Size { get; }
        public Dictionary<int, int> RequiredShapes { get; }
    }
}