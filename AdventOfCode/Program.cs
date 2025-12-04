using System.Diagnostics;
using AdventOfCode;

var testInput = @"";

var input = @"";

var sw = new Stopwatch();
sw.Start();

var day = new Day4(input);
day.Task2();

sw.Stop();
Console.WriteLine($"Completed this task in {sw.ElapsedMilliseconds} ms");