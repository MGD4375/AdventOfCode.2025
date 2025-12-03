namespace AdventOfCode._2025.Day1;

public static class Day3
{
    public static void Part1()
    {
        var result = File
            .ReadAllLines("./Day3/input.txt")
            .Select(HighestValue)
            .Sum();

        Console.WriteLine(result);
    }

    private static int HighestValue(string bank)
    {
        var leftSide = bank[..^1];
        var indexOf = -1;
        for (var i = 9; i >= 0; i--)
        {
            indexOf = leftSide.IndexOf(i.ToString(), StringComparison.Ordinal);
            if (indexOf != -1)
            {
                break;
            }
        }

        var rightSide = bank[(indexOf + 1)..];
        var indexOfRightItem = -1;
        for (var i = 9; i >= 0; i--)
        {
            indexOfRightItem = rightSide.IndexOf(i.ToString(), StringComparison.Ordinal);
            if (indexOfRightItem != -1)
            {
                break;
            }
        }

        var stringRes = $"{leftSide[indexOf]}{rightSide[indexOfRightItem]}";
        return int.Parse(stringRes);
    }

    public static void Part2()
    {
        var result = File
            .ReadAllLines("./Day3/test-input.txt")
            .Select(HighestValue2)
            .Sum();

        Console.WriteLine(result);
    }

    private static long HighestValue2(string bank)
    {
        var workingJoltage = "";
        var remainingSearchSpace = bank[..^11];
        var accumulatedIndex = 0;
        for (var i = 1; i <= 12; i++)
        {
            var indexOf = -1;
            for (var j = 9; j >= 0; j--)
            {
                indexOf = remainingSearchSpace.IndexOf(j.ToString(), StringComparison.Ordinal);
                if (indexOf != -1)
                {
                    break;
                }
            }

            workingJoltage += remainingSearchSpace[indexOf];

            if (i >= 12) continue;
            accumulatedIndex += (indexOf + 1);
            remainingSearchSpace = bank[accumulatedIndex..^(11 - i)];
        }

        return long.Parse(workingJoltage);
    }
}
