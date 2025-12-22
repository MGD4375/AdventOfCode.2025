using System.Collections;
using System.Diagnostics;
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

        var sw = new Stopwatch();
        sw.Start();
        var foo = machines.Select(machine =>
        {
            var currentState = new BitArray(machine.indicatorLights.Length);
            return Bfs(currentState, machine);
        });

        Console.WriteLine(foo.Sum(it => it.stepCount));
        sw.Stop();

        // Performance with Hashset.Any(it=> it isEqualTo)
        // Elapsed=00:00:00.7730786
        // Elapsed=00:00:00.8031957
        // Elapsed=00:00:00.7872786

        // Performance with ToKey
        // Elapsed=00:00:00.2306710
        // Elapsed=00:00:00.2283748
        // Elapsed=00:00:00.2568954

        // Removing IsEqualsTo altogether for key checks
        // Elapsed=00:00:00.2136613
        // Elapsed=00:00:00.1959746
        // Elapsed=00:00:00.2577227
        Console.WriteLine("Elapsed={0}", sw.Elapsed);
    }

    private static (bool, int stepCount) Bfs(BitArray currentState,
        (BitArray indicatorLights, IEnumerable<BitArray> buttons) machine)
    {
        var visitedStates = new HashSet<string>();
        visitedStates.Add(currentState.ToKey());

        var targetState = machine.indicatorLights.ToKey();

        var queue = new Queue<(
            BitArray currentState,
            int numberOfSteps
            )>();

        queue.Enqueue((currentState, numberOfSteps: 0));

        while (queue.Count != 0)
        {
            var (bitArray, stepCount) = queue.Dequeue();
            foreach (var button in machine.buttons)
            {
                var newState = ((BitArray)bitArray.Clone()).Xor(button);
                var key = newState.ToKey();

                if (targetState == key) return (true, stepCount: stepCount + 1);

                if (visitedStates.Contains(key)) continue;

                visitedStates.Add(key);
                queue.Enqueue((newState, stepCount + 1));
            }
        }

        throw new Exception("No solution found");
    }
}

public static class BitArrayExtensions
{
    public static string ToKey(this BitArray ba)
    {
        return string.Join("", ba.Cast<bool>().Select(b => b ? '1' : '0'));
    }
}