namespace AdventOfCode._2025.Day1;

public static class Day7
{
    public static void Part1()
    {
        var input = File.ReadAllLines("./Day7/input.txt");
        var output = new char[input[0].Length, input.Length];
        var count = 0;

        for (var x = 0; x < input[0].Length; x++)
        {
            output[x, 0] = input[0][x];
        }

        for (var y = 1; y < input.Length; y += 1)
        {
            var currentLine = input[y].Select(it => it).ToList();

            for (var x = 0; x < currentLine.Count; x++)
                if (output[x, y - 1] is 'S' or '|' && currentLine[x] == '.')
                {
                    output[x, y] = '|';
                }
                else if (output[x, y - 1] == '|' && currentLine[x] == '^')
                {
                    count++;
                    output[x - 1, y] = '|';
                    output[x + 1, y] = '|';
                    output[x, y] = currentLine[x];
                    x++;
                }
                else
                {
                    output[x, y] = currentLine[x];
                }

            // Print(output);
        }

        Console.WriteLine(count);
    }

    private static void Print(char[,] output)
    {
        for (var y = 0; y < output.GetLength(1); y++)
        {
            for (var x = 0; x < output.GetLength(0); x++) Console.Write(output[x, y]);

            Console.WriteLine();
        }
    }

    private static void Print2(char[,] output)
    {
        for (var y = 0; y < output.GetLength(1); y++)
        {
            for (var x = 0; x < output.GetLength(0); x++)
            {
                if (output[x, y] == 0) Console.Write($" .".PadRight(4));
                else Console.Write($" {(long)output[x, y]}".PadRight(4));
            }

            Console.WriteLine();
        }
    }

    public static void Part2()
    {
        // This is a path counting problem
        // I can solve it without remembering every path by using numbers instead of
        // .......S.......
        // .......1.......
        // ......1^1......
        // ......1.1......
        // .....1^2^1.....
        // .....1.2.1.....
        // ....1^3^3^1....
        // ....1.2.3.1....
        // ...1^3^231^1...
        // Each branch is an addition of the previous two.

        var input = File.ReadAllLines("./Day7/input.txt");
        var output = new long[input[0].Length, input.Length];

        for (var x = 0; x < input[0].Length; x++)
        {
            output[x, 0] = input[0][x] == 'S' ? 1 : 0;
        }

        for (var y = 1; y < input.Length; y += 1)
        {
            var currentLine = input[y].Select(it => it).ToList();

            for (var x = 0; x < currentLine.Count; x++)
            {
                if (output[x, y] > 0)
                {
                    continue;
                }

                if (output[x, y - 1] > 0 && currentLine[x] == '.')
                {
                    output[x, y] = output[x, y - 1];
                }
                else if (output[x, y - 1] > 0 && currentLine[x] == '^')
                {
                    var leftFeedLeft = x - 2 > 0 && currentLine[x - 2] == '^' ? output[x - 2, y - 1] : 0;
                    var leftFeedRight = output[x - 0, y - 1];
                    var centralFeedLeft = output[x - 1, y - 1];
                    output[x - 1, y] = leftFeedLeft + leftFeedRight + centralFeedLeft;

                    var rightFeedLeft = output[x + 0, y - 1];
                    var rightFeedRight = x + 2 < output.GetLength(0) && currentLine[x + 2] == '^'
                        ? output[x + 2, y - 1]
                        : 0;
                    var centralFeedRight = output[x + 1, y - 1];
                    output[x + 1, y] = rightFeedRight + rightFeedLeft + centralFeedRight;
                }
            }
        }

        var result = 0L;
        for (var x = 0; x < input[0].Length; x++)
        {
            result += output[x, output.GetLength(1) - 1];
        }

        Console.WriteLine(result);
    }
}

//2088060
// 27055852018812