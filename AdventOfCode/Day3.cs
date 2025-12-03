namespace AdventOfCode;

public class Day3(string input)
{
    public void Task1()
    {
        var banks = ParseBanks();
        var sum = 0L;
        foreach (var bank in banks)
        {
            sum += bank.FindMaxJoltage(2);
        }
        
        Console.WriteLine($"Sum of largest possible joltage: {sum}");
    }
    
    public void Task2()
    {
        var banks = ParseBanks();
        var sum = 0L;
        Parallel.ForEach(banks, bank =>
        {
            var result = bank.FindMaxJoltage(12);
            Interlocked.Add(ref sum, result);
        });
        
        Console.WriteLine($"Sum of largest possible joltage: {sum}");
    }

    private IEnumerable<Bank> ParseBanks()
    {
        return input.Split(['\r', '\n'], StringSplitOptions.RemoveEmptyEntries)
            .Select(x => new Bank(x));
    }
    
    private class Bank(string data)
    {
        public long FindMaxJoltage(int count)
        {
            var answer = "";
            var start = 0;
            for (var pos = 0; pos < count; pos++)
            {
                var maxValue = '\0';
                for (var i = start; i <= data.Length - (count - pos); i++)
                {
                    if (data[i] > maxValue)
                    {
                        maxValue = data[i];
                        start = i + 1;
                    }
                }
                answer += maxValue;
            }
            
            return long.Parse(answer);
        }
    }
}