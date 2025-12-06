namespace AdventOfCode._2025.Day1;

public static class Day6
{
    public static void Part1()
    {
        var input = File.ReadAllLines(@".\Day6\input.txt");
        var line1 = input[0].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(long.Parse);
        var line2 = input[1].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(long.Parse);
        var line3 = input[2].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(long.Parse);
        var line4 = input[3].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(long.Parse);
        var line5 = input[4].Split(' ', StringSplitOptions.RemoveEmptyEntries);

        var problems = Zip(line1, line2, line3, line4, line5)
            .Select(p =>
            {
                return p.Item5 switch
                {
                    '*' => p.Item1 * p.Item2 * p.Item3 * p.Item4,
                    '+' => p.Item1 + p.Item2 + p.Item3 + p.Item4,
                    _ => throw new Exception()
                };
            })
            .Sum();

        Console.WriteLine(problems);
    }

    public static IEnumerable<(long, long, long, long, char)> Zip(
        IEnumerable<long> line1,
        IEnumerable<long> line2,
        IEnumerable<long> line3,
        IEnumerable<long> line4,
        string[] line5
    )
    {
        using var e1 = line1.GetEnumerator();
        using var e2 = line2.GetEnumerator();
        using var e3 = line3.GetEnumerator();
        using var e4 = line4.GetEnumerator();

        var index = 0;
        while (e1.MoveNext() && e2.MoveNext() && e3.MoveNext() && e4.MoveNext() && index < line5.Length)
        {
            yield return (e1.Current, e2.Current, e3.Current, e4.Current, line5[index][0]);
            index++;
        }
    }


    public static IEnumerable<(string, string, string, string, char)> Zip2(
        IEnumerable<string> line1,
        IEnumerable<string> line2,
        IEnumerable<string> line3,
        IEnumerable<string> line4,
        string[] line5
    )
    {
        using var e1 = line1.GetEnumerator();
        using var e2 = line2.GetEnumerator();
        using var e3 = line3.GetEnumerator();
        using var e4 = line4.GetEnumerator();

        var index = 0;
        while (e1.MoveNext() && e2.MoveNext() && e3.MoveNext() && e4.MoveNext() && index < line5.Length)
        {
            yield return (e1.Current, e2.Current, e3.Current, e4.Current, line5[index][0]);
            index++;
        }
    }

    public static void Part2()
    {
        var input = File.ReadAllLines(@".\Day6\input.txt");
        var line1 = input[0];
        var line2 = input[1];
        var line3 = input[2];
        var line4 = input[3];
        var line5 = input[4];

        var numberStore = new List<long>();
        var answers = new List<long>();
        for (var i = line5.Length - 1; i >= 0; i--)
        {
            var numberStr = $"{line1[i]}{line2[i]}{line3[i]}{line4[i]}".Trim();
            if (numberStr.Length > 0) numberStore.Add(long.Parse(numberStr));

            if (line5[i] == '+')
            {
                answers.Add(numberStore.Sum());
                numberStore.Clear();
            }
            else if (line5[i] == '*')
            {
                var answer = numberStore.Aggregate((a, b) => a * b);
                answers.Add(answer);
                numberStore.Clear();
            }
        }

        Console.WriteLine(answers.Sum());
    }
}