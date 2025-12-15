using System.Collections;
using System.Text.RegularExpressions;

namespace AdventOfCode._2025.Day1;

public static class Day10
{
    public static void Part1()
    {
        IEnumerable<(BitArray indicatorLights, IEnumerable<BitArray> buttons)> machines = File
            .ReadAllLines("./Day10/input.txt")
            .Select(line =>
            {
                var indicatorLightsInput = Regex.Match(line, @"\[.*\]")
                    .Value
                    .Replace("[", "")
                    .Replace("]", "")
                    .Select(it => it switch
                    {
                        '#' => true,
                        '.' => false,
                        _ => throw new Exception("Parsing error")
                    })
                    .ToArray();

                var indicatorLights = new BitArray(indicatorLightsInput);


                var buttons = Regex.Match(line, @"\(.*\)")
                    .Value
                    .Split(' ')
                    .ToList()
                    .Select(m => m
                        .Replace("(", "")
                        .Replace(")", "")
                        .Split(',')
                        .Select(int.Parse)
                        .ToList())
                    .ToList()
                    .Select(it =>
                    {
                        var foo = new bool[indicatorLights.Length];
                        var buttonResult = new BitArray(foo);
                        foreach (var buttonIndex in it) buttonResult[buttonIndex] = true;

                        return buttonResult;
                    });


                return (indicatorLights, buttonsInput: buttons);
            });

        var foo = machines.Select(machine =>
        {
            var currentState = new BitArray(machine.indicatorLights.Length);
            return Bfs(currentState, machine);
        });

        Console.WriteLine(foo.Sum(it => it.stepCount));
    }

    private static (bool, int stepCount) Bfs(BitArray currentState,
        (BitArray indicatorLights, IEnumerable<BitArray> buttons) machine)
    {
        var visitedStates = new HashSet<BitArray>();

        var queue =
            new Queue<(BitArray currentState, int
                numberOfSteps)>();

        queue.Enqueue((currentState, numberOfSteps: 0));

        while (queue.Count != 0)
        {
            var (bitArray, stepCount) = queue.Dequeue();
            foreach (var button in machine.buttons)
            {
                var newState = ((BitArray)bitArray.Clone()).Xor(button);
                if (machine.indicatorLights.IsEqualTo(newState))
                {
                    return (true, stepCount: stepCount + 1);
                }

                if (visitedStates.Any(s => s.IsEqualTo(newState)))
                {
                    continue;
                }

                visitedStates.Add(newState);
                queue.Enqueue((newState, stepCount + 1));
            }
        }

        throw new Exception("No solution found");
    }
}

public static class BitArrayExtensions
{
    public static bool IsEqualTo(this BitArray array1, BitArray array2)
    {
        for (var i = 0; i < array1.Length; i++)
        {
            if (array1[i] != array2[i])
                return false;
        }

        return true;
    }
}