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
        var area = 0L;
        var polygon = new Line[_points.Count];
        var previous = new Point(-1, -1);

        var i = 0;
        foreach (var point in _points)
        {
            if (previous.X == -1)
            {
                previous = point;
                continue;
            }

            polygon[i++] = new Line(previous, point);
            previous = point;
        }

        polygon[i] = new Line(previous, _points[0]);

        for (i = 0; i < _points.Count; i++)
        {
            for (var j = i + 1; j < _points.Count; j++)
            {
                var pointA = _points[i];
                var pointB = _points[j];
                var area2 = (Math.Abs(pointA.X - pointB.X) + 1L) * (Math.Abs(pointA.Y - pointB.Y) + 1);
                if (area2 <= area)
                {
                    // Don't bother checking smaller areas for intersections
                    continue;
                }
                
                var empty = true;
                var startX = Math.Min(pointA.X, pointB.X);
                var endX = Math.Max(pointA.X, pointB.X);
                var startY = Math.Min(pointA.Y, pointB.Y);
                var endY = Math.Max(pointA.Y, pointB.Y);
                
                for (var k = 0; k < polygon.Length; k++)
                {
                    var line = polygon[k];

                    if (line.Intersects(startX, startY, endX, endY))
                    {
                        empty = false;
                        break;
                    }
                }

                if (empty)
                {
                    area = area2;
                }
            }
        }
        
        Console.WriteLine($"The largest area is {area}");
    }

    private readonly record struct Line(Point Start, Point End)
    {
        public bool Intersects(int startX, int startY, int endX, int endY)
        {
            var start = Start;
            var end = End;

            if (start.X == end.X)
            {
                var x = start.X;
                if (x <= startX || x >= endX)
                {
                    return false;
                }

                var minY = Math.Min(start.Y, end.Y);
                var maxY = Math.Max(start.Y, end.Y);

                return maxY > startY && minY < endY;
            }

            if (start.Y != end.Y)
            {
                return true;
            }
            
            var y = start.Y;
            if (y <= startY || y >= endY)
            {
                return false;
            }

            var minX = Math.Min(start.X, end.X);
            var maxX = Math.Max(start.X, end.X);

            return maxX > startX && minX < endX;
        }
    }
}