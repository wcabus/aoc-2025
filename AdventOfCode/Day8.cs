namespace AdventOfCode;

public class Day8(string input)
{
    public void Task1(int connections = 1000)
    {
        var points = input.Split('\n', StringSplitOptions.RemoveEmptyEntries)
            .Select(Point3D.Parse)
            .ToList();

        var map = CalculateDistances(points);
        var orderedMap = map.OrderBy(x => x.Key).Select(x => x.Value).ToArray();
        var circuits = points.ToDictionary(x => x, x => new HashSet<Point3D>([x]));
        foreach (var (a, b) in orderedMap.Take(connections)) 
        {
            if (circuits[a] != circuits[b]) 
            {
                Connect(a, b, circuits);
            }
        }
        
        var result = circuits.Values.Distinct()
            .OrderByDescending(set => set.Count)
            .Take(3)
            .Aggregate(1, (a, b) => a * b.Count);
        
        Console.WriteLine($"The result is: {result}");
    }

    public void Task2()
    {
        var points = input.Split('\n', StringSplitOptions.RemoveEmptyEntries)
            .Select(Point3D.Parse)
            .ToList();

        var map = CalculateDistances(points);
        var orderedMap = map.OrderBy(x => x.Key).Select(x => x.Value).ToArray();
        var circuits = points.ToDictionary(x => x, x => new HashSet<Point3D>([x]));
        var numberOfPoints = points.Count;
        var result = 0L;
        
        foreach (var (a, b) in orderedMap.TakeWhile(_ => numberOfPoints > 1))
        {
            if (circuits[a] == circuits[b])
            {
                continue;
            }
            Connect(a, b, circuits);
            result = (long)a.X * b.X;
            numberOfPoints--;
        }
        
        Console.WriteLine($"The result is: {result}");
    }
    
    private void Connect(Point3D a, Point3D b, Dictionary<Point3D, HashSet<Point3D>> set) {
        set[a].UnionWith(set[b]);
        foreach (var p in set[b]) 
        {
            set[p] = set[a];
        }
    }
    
    private Dictionary<double, (Point3D a, Point3D b)> CalculateDistances(List<Point3D> points)
    {
        var distances = new Dictionary<double, (Point3D a, Point3D b)>();
        
        for (var i = 0; i < points.Count; i++)
        {
            for (var j = i + 1; j < points.Count; j++)
            {
                var distance = Point3D.Distance(points[i], points[j]);
                distances[distance] = (points[i], points[j]);
            }
        }

        return distances;
    }

    private record Point3D(int X, int Y, int Z)
    {
        public static Point3D Parse(string s)
        {
            var parts = s.Split(',');
            return new Point3D(int.Parse(parts[0]), int.Parse(parts[1]), int.Parse(parts[2]));
        }
        
        public static double Distance(Point3D p1, Point3D p2)
        {
            return Math.Sqrt(Math.Pow(p1.X - p2.X, 2) + Math.Pow(p1.Y - p2.Y, 2) + Math.Pow(p1.Z - p2.Z, 2));
        }
    }
}