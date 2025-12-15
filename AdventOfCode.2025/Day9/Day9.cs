namespace AdventOfCode._2025.Day1;

public static class Day9
{
    public static void Part1()
    {
        var input = File.ReadAllLines("./Day9/input.txt")
            .Select(it =>
            {
                var split = it.Split(",");
                return (X: long.Parse(split[0]), Y: long.Parse(split[1]));
            })
            .ToList();

        var maxArea = GetAreas(input).Max(a => a.Area);
        Console.WriteLine(maxArea);
    }

    private static IEnumerable<((long X, long Y) positionA, (long X, long Y) positionB, long Area)> GetAreas(
        IEnumerable<(long X, long Y)> input)
    {
        var tiles = input.ToList();

        foreach (var positionA in tiles)
        foreach (var positionB in tiles)
        {
            if (positionA == positionB) continue;

            var width = Math.Abs(positionA.X - positionB.X) + 1;
            var height = Math.Abs(positionA.Y - positionB.Y) + 1;
            var area = width * height;

            yield return (positionA, positionB, area);
        }
    }
}