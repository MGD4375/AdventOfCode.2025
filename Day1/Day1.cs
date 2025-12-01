namespace AdventOfCode._2025.Day1;

public static class Day1
{
    public static void Part1()
    {
        var input = File
            .ReadAllLines("./Day1/input.txt")
            .Select(line => (line[..1], int.Parse(line[1..])));

        const int startingPosition = 50;
        const int modValue = 100;

        var part1Answer = Positions(input, startingPosition, modValue)
            .Count(it => it == 0);

        Console.WriteLine($"Part 1: {part1Answer}");
    }

    private static IEnumerable<int> Positions(
        IEnumerable<(string, int)> instructions,
        int startingPosition,
        int modValue)
    {
        var currentValue = startingPosition;
        foreach (var instruction in instructions)
        {
            var move = instruction.Item1 switch
            {
                "L" => instruction.Item2 * -1,
                "R" => instruction.Item2,
                _ => throw new Exception("Invalid instruction")
            };

            currentValue += move;

            yield return currentValue % modValue;
        }
    }


    public static void Part2()
    {
        var input = File
            .ReadAllLines("./Day1/input.txt")
            .Select(line => (line[..1], int.Parse(line[1..])));

        const int startingPosition = 50;
        const int modValue = 100;

        var part2 = Positions2(input, startingPosition, modValue)
            .Sum();

        Console.WriteLine($"Part 2: {part2}");
    }

    private static IEnumerable<int> Positions2(
        IEnumerable<(string, int)> instructions,
        int startingPosition,
        int modValue)
    {
        var currentValue = startingPosition;
        foreach (var (direction, distance) in instructions)
        {
            var isLeft = direction == "L";

            var timesCrossedZero = 0;

            for (var i = 0; i < distance; i++)
            {
                if (isLeft)
                {
                    currentValue--;
                    if (currentValue < 0) currentValue += modValue;
                }
                else
                {
                    currentValue++;
                    if (currentValue >= modValue) currentValue -= modValue;
                }

                if (currentValue == 0)
                {
                    timesCrossedZero++;
                }
            }

            yield return timesCrossedZero;
        }
    }
}