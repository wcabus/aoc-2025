namespace AdventOfCode;

public class Day11(string input)
{
    public void Task1()
    {
        var paths = ParseInput();
        var numberOfPaths = TraversePaths(paths, "you", "out");
        Console.WriteLine($"There are {numberOfPaths} that lead to the out endpoint.");
    }

    public void Task2()
    {
        var paths = ParseInput();
        var numberOfPaths = TraverseSvrToOut(paths);
        Console.WriteLine($"There are {numberOfPaths} that lead to the out endpoint.");
    }

    private IDictionary<string, IReadOnlyList<string>> ParseInput()
    {
        var dict = new Dictionary<string, IReadOnlyList<string>>();
        foreach (var line in input.Split(['\r', '\n'], StringSplitOptions.RemoveEmptyEntries))
        {
            var key = line[..3];
            var values = line[5..].Split(' ', StringSplitOptions.RemoveEmptyEntries)
                .ToList();
            
            dict.Add(key, values);
        }
        
        return dict;
    }

    private static readonly Dictionary<string, long> PathMemory = new();
    private static long TraversePaths(IDictionary<string, IReadOnlyList<string>> paths, string current, string end, string avoid = "")
    {
        if (current == end)
        {
            return 1;
        } 
        if (current == avoid)
        {
            return 0;
        }

        if (PathMemory.TryGetValue(current, out var val))
        {
            return val;
        }

        var pathsCount = !paths.TryGetValue(current, out var nextPaths) 
            ? 0 
            : nextPaths.Sum(path => TraversePaths(paths, path, end));
        PathMemory.Add(current, pathsCount);
        return pathsCount;
    }

    private long TraverseSvrToOut(IDictionary<string, IReadOnlyList<string>> paths)
    {
        PathMemory.Clear();
        var svrToFft = TraversePaths(paths, "svr", "fft", "dac");
        PathMemory.Clear();
        var svrToDac = TraversePaths(paths, "svr", "dac", "fft");
        
        PathMemory.Clear();
        var fftToDac = TraversePaths(paths, "fft", "dac");
        PathMemory.Clear();
        var dacToFft = TraversePaths(paths, "dac", "fft");
        
        PathMemory.Clear();
        var dacToOut = TraversePaths(paths, "dac", "out");
        PathMemory.Clear();
        var fftToOut = TraversePaths(paths, "fft", "out");
        
        return svrToFft * fftToDac * dacToOut
               + svrToDac * dacToFft * fftToOut;
    }
}