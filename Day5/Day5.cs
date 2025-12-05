using System.Runtime.InteropServices.JavaScript;

namespace AdventOfCode._2025.Day1;

public static class Day5
{
    public static void Part1()
    {
        var input = File.ReadAllText("./Day5/input.txt").Split("\r\n\r\n");
        var ranges = input[0].Split("\r\n").Select(range =>
        {
            var split = range.Split('-');
            var left = long.Parse(split[0]);
            var right = long.Parse(split[1]);

            return (left, right);
        });

        var ids = input[1]
            .Split("\r\n")
            .Select(long.Parse);


        var freshIngredientsCount = ids.Count(id => ranges.Any(r =>
        {
            var start = r.left;
            var end = r.right;
            return id >= start && id <= end;
        }));

        Console.WriteLine(freshIngredientsCount);
    }

    public static void Part2()
    {
        var input = File.ReadAllText("./Day5/input.txt").Split("\r\n\r\n");
        var ranges = input[0].Split("\r\n").Select(range =>
        {
            var split = range.Split('-');
            var left = long.Parse(split[0]);
            var right = long.Parse(split[1]);
            return (left, right);
        }).OrderBy(r => r.left);

        var adjustedRanges = ranges
            .Aggregate(
                seed: (ranges: new List<(long left, long right)>(), currentMin: 0L),
                func: (acc, range) =>
                {
                    if (range.right < acc.currentMin)
                        return acc;

                    var min = Math.Max(range.left, acc.currentMin);
                    var max = range.right;
                    acc.ranges.Add((min, max));

                    return (acc.ranges, max + 1);
                })
            .ranges;

        var count = adjustedRanges.Sum(r => (r.right + 1) - r.left);
        Console.WriteLine(count);
    }
}

