namespace AdventOfCode;

public class Day2(string input)
{
    public void Task1()
    {
        var sumOfInvalidIds = 0L;
        foreach (var range in ParseRanges(input))
        {
            var value = range.Start;
            while (value <= range.End)
            {
                if (IsInvalidId(value))
                {
                    sumOfInvalidIds += value;
                }

                value++;
            }
        }
        
        Console.WriteLine($"Sum of invalid IDs: {sumOfInvalidIds}");
    }

    public void Task2()
    {
        var sumOfInvalidIds = 0L;
        foreach (var range in ParseRanges(input))
        {
            var value = range.Start;
            while (value <= range.End)
            {
                if (IsInvalidId2(value))
                {
                    sumOfInvalidIds += value;
                }

                value++;
            }
        }
        
        Console.WriteLine($"Sum of invalid IDs: {sumOfInvalidIds}");
    }

    private IEnumerable<(long Start, long End)> ParseRanges(string data)
    {
        var splits = data.Split([',', '\r', '\n'], StringSplitOptions.RemoveEmptyEntries);
        foreach (var split in splits)
        {
            var indexOfDash = split.IndexOf('-');
            yield return new(long.Parse(split[..indexOfDash]), long.Parse(split[(indexOfDash + 1)..]));
        }
    }

    private bool IsInvalidId(long value)
    {
        var numberOfDigits = Math.Floor(Math.Log10(value)) + 1;
        if (numberOfDigits % 2 != 0)
        {
            return false;
        }
        
        var divisor = (int)Math.Pow(10, numberOfDigits / 2);
        var firstHalf = value / divisor;
        var secondHalf = value % divisor;
        return firstHalf == secondHalf;
    }

    private bool IsInvalidId2(long value)
    {
        var strValue = value.ToString();
        var length = strValue.Length;
        for (var i = 1; i <= length / 2; i++)
        {
            if (length % i != 0)
            {
                continue;
            }
            
            var substring = strValue[..i];
            var repeated = string.Concat(Enumerable.Repeat(substring, length / i));
            if (string.Equals(repeated, strValue, StringComparison.Ordinal))
            {
                return true;
            }
        }

        return false;
    }
}