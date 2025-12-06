namespace AdventOfCode._2025.Day1;

using Position = (int x, int y);

public static class Day4
{
    private static readonly List<Position> RelativePositions =
    [
        (-1, -1), (-1, -0), (-1, +1),
        (-0, -1), (-0, +1),
        (+1, -1), (+1, -0), (+1, +1)
    ];

    public static void Part1()
    {
        var input = File.ReadAllLines("./Day4/input.txt");
        var grid = ToGrid(input);

        var accessibleItems = 0;
        for (var y = 0; y < grid.GetLength(0); y++)
        for (var x = 0; x < grid.GetLength(1); x++)
        {
            var isLiftable = grid[x, y] == '@'
                             && RelativePositions
                                 .Select(rp => new Position(x + rp.x, y + rp.y))
                                 .Where(ap => IsWithinBounds(ap, grid))
                                 .Count(p => grid[p.x, p.y] == '@') < 4;
            if (isLiftable) accessibleItems++;
        }

        Console.WriteLine(accessibleItems);
    }

    public static void Part2()
    {
        var input = File.ReadAllLines("./Day4/input.txt");
        var grid = ToGrid(input);

        var remainingItems = -1;
        var nextItemCount = CountItems(grid);
        var startingItemCount = nextItemCount;

        while (remainingItems != nextItemCount)
        {
            remainingItems = nextItemCount;
            grid = Tick(grid);
            nextItemCount = CountItems(grid);
        }

        Console.WriteLine(startingItemCount - remainingItems);
    }

    private static char[,] Tick(char[,] grid)
    {
        var shadowBoard = new char[grid.GetLength(0), grid.GetLength(1)];

        for (var y = 0; y < grid.GetLength(0); y++)
        for (var x = 0; x < grid.GetLength(1); x++)
        {
            if (grid[x, y] == '.')
            {
                shadowBoard[x, y] = '.';
                continue;
            }

            var isLiftable = grid[x, y] == '@'
                             && RelativePositions
                                 .Select(rp => new Position(x + rp.x, y + rp.y))
                                 .Where(ap => IsWithinBounds(ap, grid))
                                 .Count(p => grid[p.x, p.y] == '@') < 4;

            shadowBoard[x, y] = isLiftable ? '.' : '@';
        }

        return shadowBoard;
    }

    private static int CountItems(char[,] grid)
    {
        var itemCount = 0;
        for (var y = 0; y < grid.GetLength(0); y++)
        for (var x = 0; x < grid.GetLength(1); x++)
            if (grid[x, y] == '@')
                itemCount++;

        return itemCount;
    }

    private static bool IsWithinBounds((int x, int y) ap, char[,] grid)
    {
        return ap.y >= 0 && ap.y < grid.GetLength(1) && ap.x >= 0 && ap.x < grid.GetLength(0);
    }

    private static char[,] ToGrid(string[] input)
    {
        var grid = new char[input[0].Length, input.Length];
        for (var y = 0; y < input.Length; y++)
        for (var x = 0; x < input[0].Length; x++)
            grid[x, y] = input[y][x];

        return grid;
    }
}