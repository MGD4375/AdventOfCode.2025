namespace AdventOfCode._2025.Day1;

public static class Day2
{
    public static void Part1()
    {
        var ranges = File
            .ReadAllText("./Day2/test-input.txt")
            .Split(",")
            .Select(item =>
            {
                var range = item.Split("-");
                return (long.Parse(range[0]), long.Parse(range[1]));
            });

        var foo = ranges
            .SelectMany(it => it.Expand())
            .Where(it =>
            {
                var strId = it.ToString();
                var l = strId.Length / 2;
                var left = strId[..l];
                var right = strId[l..];
                return left == right;
            })
            .Sum();

        Console.WriteLine(foo);
    }

    public static void Part2()
    {
        var ranges = File
            .ReadAllText("./Day2/input.txt")
            .Split(",")
            .Select(item =>
            {
                var range = item.Split("-");
                return (long.Parse(range[0]), long.Parse(range[1]));
            });

        var foo = ranges
            .SelectMany(it => it.Expand())
            .Where(it =>
            {
                var strId = it.ToString();
                var len = strId.Length;

                for (var patternLen = 1; patternLen <= len / 2; patternLen++)
                {
                    if (len % patternLen != 0) continue;
                    var isRepeating = true;
                    for (var i = patternLen; i < len; i++)
                    {
                        if (strId[i] == strId[i % patternLen]) continue;
                        isRepeating = false;
                        break;
                    }

                    if (isRepeating) return true;
                }

                return false;
            })
            .Sum();

        Console.WriteLine(foo);
    }

    private static IEnumerable<long> Expand(this (long start, long end) range)
    {
        for (var i = range.start; i <= range.end; i++) yield return i;
    }
}