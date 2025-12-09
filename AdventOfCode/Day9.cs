using System.Drawing;

namespace AdventOfCode;

public class Day9
{
    private readonly List<Point> _points = [];
    
    public Day9(string input)
    {
        foreach (var line in input.Split(['\r', '\n'], StringSplitOptions.RemoveEmptyEntries))
        {
            var parts = line.Split(',');
            _points.Add(new Point(int.Parse(parts[0]), int.Parse(parts[1])));
        }
    }
    
    public void Task1()
    {
        var area = 0L;
        for (var i = 0; i < _points.Count; i++)
        {
            for (var j = i + 1; j < _points.Count; j++)
            {
                var area2 = (Math.Abs(_points[i].X - _points[j].X) + 1L) * (Math.Abs(_points[i].Y - _points[j].Y) + 1);
                if (area2 > area)
                {
                    area = area2;
                }
            }
        }

        Console.WriteLine($"The largest area is {area}");
    }

    public void Task2()
    {
        
    }

}