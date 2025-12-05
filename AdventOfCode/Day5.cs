namespace AdventOfCode;

public class Day5(string input)
{
    private readonly List<(long from, long to)> _ranges = [];
    private readonly List<long> _ids = [];

    public void Task1()
    {
        ParseInput();

        var fresh = 0;
        Parallel.ForEach(_ids, id =>
        {
            if (_ranges.Any(range => id <= range.to && id >= range.from))
            {
                Interlocked.Increment(ref fresh);
            }
        });
        
        Console.WriteLine($"{fresh} items are fresh.");
    }

    public void Task2()
    {
        ParseInput();

        var numberOfFreshItems = 0L;
        var ranges = _ranges.OrderBy(x => x.from).ThenBy(x => x.to);
        var currentRange = (from: -1L, to: -1L);
        foreach (var range in ranges)
        {
            if (currentRange.from == -1L)
            {
                currentRange.from = range.from;
                currentRange.to = range.to;
                continue;
            }
            
            // if the next range overlaps or touches the current range, extend the current range
            if (range.from <= currentRange.to + 1)
            {
                currentRange.to = Math.Max(currentRange.to, range.to);
            }
            else
            {
                numberOfFreshItems += currentRange.to - currentRange.from + 1;
                currentRange.from = range.from;
                currentRange.to = range.to;
            }
        }
        
        // add the last range
        numberOfFreshItems += currentRange.to - currentRange.from + 1;
        Console.WriteLine($"{numberOfFreshItems} items are fresh.");
    }

    private void ParseInput()
    {
        foreach (var line in input.Split(['\r', '\n'], StringSplitOptions.RemoveEmptyEntries))
        {
            if (line.IndexOf('-') != -1)
            {
                var parts = line.Split('-');
                var from = long.Parse(parts[0]);
                var to = long.Parse(parts[1]);

                _ranges.Add(from <= to ? (from, to) : (to, from));
            }
            else
            {
                _ids.Add(long.Parse(line));
            }
        }
    }
}