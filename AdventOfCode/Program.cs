using System.Diagnostics;
using AdventOfCode;

var testInput = @"";

var input = @"";

var sw = new Stopwatch();
sw.Start();

var day = new Day12(input);
day.Task1();

sw.Stop();
Console.WriteLine($"Completed this task in {sw.ElapsedMilliseconds} ms");