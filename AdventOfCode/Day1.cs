namespace AdventOfCode;

public class Day1(string input)
{
    public void Task1()
    {
        var dial = new Dial();
        foreach (var line in input.Split(['\r', '\n']))
        {
            dial.Move(line);
        }
        
        Console.WriteLine($"The dial was in position 0: {dial.CountZero} times.");
    }
    
    public void Task2()
    {
        var dial = new Dial(true);
        foreach (var line in input.Split(['\r', '\n']))
        {
            dial.Move(line);
        }
        
        Console.WriteLine($"The dial was in position 0: {dial.CountZero} times.");
    }

    public class Dial(bool countAllZeros = false)
    {
        private int _position = 50;
        private int _countZero;

        public int CountZero => _countZero;
        public int Position => _position;
        
        public void Move(string input)
        {
            var direction = input[0];
            var steps = int.Parse(input[1..]);
            if (steps == 0)
            {
                if (_position == 0)
                {
                    _countZero++;
                }
                return;
            }

            if (direction == 'L')
            {
                while (steps-- > 0)
                {
                    _position--;

                    if (_position < 0)
                    {
                        _position = 100 + _position;
                    }
                    
                    if (countAllZeros && _position == 0)
                    {
                        _countZero++;
                    }
                }
            }
            else if (direction == 'R')
            {
                while (steps-- > 0)
                {
                    _position++;
                    if (_position >= 100)
                    {
                        _position -= 100;
                    }
                    
                    if (countAllZeros && _position == 0)
                    {
                        _countZero++;
                    }
                }
            }
            
            if (!countAllZeros && _position == 0)
            {
                _countZero++;
            }
        }
    }
}