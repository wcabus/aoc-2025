namespace AdventOfCode;

public class Day4(string input)
{
    private char[,]? _grid;

    private void ParseGrid()
    {
        var lines = input.Split(['\r', '\n'], StringSplitOptions.RemoveEmptyEntries);
        var lineLength = lines[0].Length;
        _grid = new char[lines.Length, lineLength];

        var y = 0;
        foreach (var line in lines)
        {
            var x = 0;
            foreach (var c in line)
            {
                _grid[y, x++] = c;
            }
            y++;
        }
    }
    
    public void Task1()
    {
        ParseGrid();
        var accessible = 0;
        for (var x = 0; x < _grid!.GetLength(0); x++)
        {
            for (var y = 0; y < _grid.GetLength(1); y++)
            {
                if (_grid[x, y] == '@' && CountNeighbors(x, y) <= 3)
                {
                    accessible++;
                }
            }
        }
        
        Console.WriteLine($"There are {accessible} accessible rolls of paper.");
    }
    
    public void Task2()
    {
        ParseGrid();
        
        var removed = 0;
        var toRemove = new List<(int x, int y)>();
        var loop = true;
        
        while (loop)
        {
            toRemove.Clear();
            for (var x = 0; x < _grid!.GetLength(0); x++)
            {
                for (var y = 0; y < _grid.GetLength(1); y++)
                {
                    if (_grid[x, y] == '@' && CountNeighbors(x, y) <= 3)
                    {
                        toRemove.Add((x, y));
                    }
                }
            }

            if (toRemove.Count > 0)
            {
                removed += toRemove.Count;
                foreach (var (x, y) in toRemove)
                {
                    _grid[x, y] = '.';
                }
            }
            else
            {
                loop = false;
            }
        }
        
        Console.WriteLine($"Removed {removed} rolls of paper.");
    }

    private int CountNeighbors(int x, int y)
    {
        var count = 0;
        for (var dx = -1; dx <= 1; dx++)
        {
            for (var dy = -1; dy <= 1; dy++)
            {
                if (dx == 0 && dy == 0) continue;

                var nx = x + dx;
                var ny = y + dy;

                if (nx < 0 || nx >= _grid!.GetLength(0) || ny < 0 || ny >= _grid.GetLength(1))
                {
                    continue;
                }
                
                if (_grid[nx, ny] == '@')
                {
                    count++;
                }
            }
        }
        return count;
    }
}