using System.Drawing;

namespace AdventOfCode;

public class Day7(string input)
{
    private int _numberOfSplits;
    private long _numberOfTimelines;
    
    public void Task1()
    {
        var grid = ParseInput();
        var startPos = FindStartPosition(grid);
        SimulateBeam(grid, startPos);
        Console.WriteLine($"Number of tachyon beam splits: {_numberOfSplits}");
    }
    
    public void Task2()
    {
        var grid = ParseInput();
        var startPos = FindStartPosition(grid);
        SimulateBeam(grid, startPos);
        Console.WriteLine($"Number of tachyon beam timelines: {_numberOfTimelines}");
    }
    
    private char[,] ParseInput()
    {
        var lines = input.Split(['\r', '\n'], StringSplitOptions.RemoveEmptyEntries);
        var maxY = lines.Length;
        var maxX = lines[0].Length;
        
        var grid = new char[maxX, maxY];
        for (var y = 0; y < maxY; y++)
        {
            for (var x = 0; x < maxX; x++)
            {
                grid[x, y] = lines[y][x];
            }
        }
        return grid;
    }
    
    private Point FindStartPosition(char[,] grid)
    {
        var maxX = grid.GetLength(0);
        
        for (var x = 0; x < maxX; x++)
        {
            if (grid[x, 0] == 'S')
            {
                return new(x, 0);
            }
        }

        throw new Exception("Start position not found");
    }
    
    private void SimulateBeam(char[,] grid, Point start)
    {
        var splitCounter = 0;
        var timelineCounter = 0L;
        var currentBeams = new Dictionary<Point, long> { { start, 1 } };

        var maxX = grid.GetLength(0);
        var maxY = grid.GetLength(1);
        
        while (currentBeams.Count > 0)
        {
            var newBeams = new Dictionary<Point, long>();
            foreach (var (beamPos, depth) in currentBeams)
            {
                var nextPos = beamPos with { Y = beamPos.Y + 1 };
                if (nextPos.X < 0 || nextPos.X >= maxX ||
                    nextPos.Y < 0 || nextPos.Y >= maxY)
                {
                    timelineCounter += depth;
                    continue;
                }

                List<Point> newPositions;
                switch (grid[nextPos.X, nextPos.Y])
                {
                    case '^':
                        splitCounter++;
                        newPositions = [nextPos with { X = nextPos.X - 1}, nextPos with { X = nextPos.X + 1}];
                        break;
                    default:
                        newPositions = [nextPos];
                        break;
                }

                foreach (var newPos in newPositions.Where(newPos => !newBeams.TryAdd(newPos, depth)))
                {
                    newBeams[newPos] += depth;
                }
            }

            currentBeams = newBeams;
        }

        _numberOfSplits = splitCounter;
        _numberOfTimelines = timelineCounter;
    }
}