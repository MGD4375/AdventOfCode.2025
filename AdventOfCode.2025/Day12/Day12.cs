using System.Text.RegularExpressions;

namespace AdventOfCode._2025.Day1;

public static class Day12
{
    public static void Part1()
    {
        var input = File.ReadAllText("./Day12/test-input.txt");
        var shapes = Regex.Matches(input, @"\d+:\r?\n[#.]{3}\r?\n[#.]{3}\r?\n[#.]{3}")
            .Select(match =>
            {
                var shape = new bool[3, 3];
                var lines = match.Value.Split('\n');

                // Skip the first line (the label like "0:")
                for (var y = 1; y < lines.Length; y++)
                {
                    var line = lines[y].Trim();
                    for (var x = 0; x < line.Length; x++)
                    {
                        shape[y - 1, x] = line[x] == '#' ? true
                            : line[x] == '.' ? false
                            : throw new Exception("Invalid shape");
                    }
                }

                return shape;
            })
            .ToList();

        var regions = Regex.Matches(input, @"[0-9]....*")
            .ToList()
            .Select(it =>
            {
                var spl = it.Value.Split(':');
                var leftSpl = spl[0].Split('x');
                var width = int.Parse(leftSpl[0]);
                var length = int.Parse(leftSpl[1]);

                var shapeCounts = spl[1]
                    .Trim()
                    .Split(' ')
                    .Select(int.Parse)
                    .ToList();

                return (width, length, shapeCounts);
            });

        // I want a backtracking algorithm which tries all shapes, flipped and rotated
        var answers = regions.Select(r => Solve(r, shapes)).ToList();
        Console.WriteLine("Fits: " + answers.Count(it => it));
    }

    private static bool Solve(
        (int width, int length, List<int> shapeCounts) region,
        List<bool[,]> shapes
    )
    {
        var grid = new bool[region.length, region.width];
        List<int> rotationOptions = [0, 1, 2, 3];
        List<(int, int)> flipOptions =
        [
            (1, 1), (-1, 1), (1, -1), (-1, -1)
        ];

        var result = SolveInner(
            (region.width, region.length),
            region.shapeCounts,
            shapes,
            rotationOptions,
            flipOptions,
            grid);


        return result;
    }

    private static bool SolveInner(
        (int width, int length) region,
        List<int> shapeCounts,
        List<bool[,]> shapes,
        List<int> rotationOptions,
        List<(int, int)> flipOptions,
        bool[,] grid)
    {
        // Base case: if all shapes have been placed, we're done!
        if (shapeCounts.All(c => c == 0))
            return true;

        // Try each shape type
        for (var shapeIndex = 0; shapeIndex < shapeCounts.Count; shapeIndex++)
        {
            var count = shapeCounts[shapeIndex];
            if (count == 0) continue; // Skip if we don't need this shape

            var shape = shapes[shapeIndex]; // FIX: was shapes[shapeCounts[shapeIndex]]

            // Try all transformations
            for (var rotationIndex = 0; rotationIndex < rotationOptions.Count; rotationIndex++)
            {
                var rotatedShape = Rotate(shape, rotationOptions[rotationIndex]);

                for (var flipIndex = 0; flipIndex < flipOptions.Count; flipIndex++)
                {
                    var finalShape = Flip(rotatedShape, flipOptions[flipIndex]);

                    // Try all positions
                    for (var y = 0; y < region.length; y++)
                    for (var x = 0; x < region.width; x++) // FIX: was region.length
                    {
                        var placementResult = PlaceShape(grid, finalShape, y, x);
                        if (!placementResult.IsSuccess)
                            continue;

                        // Recursively try to place remaining shapes
                        var newShapeList =
                            ReduceShapeList(shapeCounts, shapeIndex, count - 1); // FIX: was eachShapeCount - 1
                        if (SolveInner(region, newShapeList, shapes, rotationOptions, flipOptions,
                                placementResult.Grid))
                            return true; // Success! Propagate upward
                    }
                }
            }
        }

        return false; // Couldn't place all shapes
    }

    private static List<int> ReduceShapeList(List<int> shapeCounts, int shapeIndex, int newValue)
    {
        return shapeCounts.Select((t, i) => i == shapeIndex ? newValue : t).ToList();
    }

    private static (bool[,] Grid, bool IsSuccess) PlaceShape(bool[,] grid, bool[,] finalShape, int y, int x)
    {
        int gridRows = grid.GetLength(0);
        int gridCols = grid.GetLength(1);
        int shapeRows = finalShape.GetLength(0);
        int shapeCols = finalShape.GetLength(1);

        // First pass: check if placement is valid
        for (int shapeY = 0; shapeY < shapeRows; shapeY++)
        {
            for (int shapeX = 0; shapeX < shapeCols; shapeX++)
            {
                // Only check cells where the shape has 'true'
                if (!finalShape[shapeY, shapeX])
                    continue;

                int targetY = y + shapeY;
                int targetX = x + shapeX;

                // Check if out of bounds
                if (targetY < 0 || targetY >= gridRows || targetX < 0 || targetX >= gridCols)
                {
                    return (grid, false); // Can't place shape outside grid
                }

                // Check collision: can't place true on true
                if (grid[targetY, targetX])
                {
                    return (grid, false);
                }
            }
        }

        // Create a copy of the grid for the new state
        var newGrid = (bool[,])grid.Clone();

        // Second pass: place the shape
        for (int shapeY = 0; shapeY < shapeRows; shapeY++)
        {
            for (int shapeX = 0; shapeX < shapeCols; shapeX++)
            {
                if (finalShape[shapeY, shapeX])
                {
                    int targetY = y + shapeY;
                    int targetX = x + shapeX;
                    newGrid[targetY, targetX] = true;
                }
            }
        }

        return (newGrid, true);
    }

    public static bool[,] Flip(bool[,] rotatedShape, (int, int) flipOption)
    {
        var rows = rotatedShape.GetLength(0);
        var cols = rotatedShape.GetLength(1);

        var flipped = new bool[rows, cols];

        for (var row = 0; row < rows; row++)
        for (var col = 0; col < cols; col++)
        {
            var newRow = flipOption.Item1 == -1 ? rows - 1 - row : row;
            var newCol = flipOption.Item2 == -1 ? cols - 1 - col : col;

            flipped[newRow, newCol] = rotatedShape[row, col];
        }

        return flipped;
    }

    public static bool[,] Rotate(bool[,] src, int rotationOption)
    {
        var output = (bool[,])src.Clone();
        for (var i = 0; i < rotationOption; i++) output = RotateInner(output);

        return output;
    }

    private static bool[,] RotateInner(bool[,] src)
    {
        var width = src.GetUpperBound(0) + 1;
        var height = src.GetUpperBound(1) + 1;
        var dst = new bool[height, width];

        for (var row = 0; row < height; row++)
        for (var col = 0; col < width; col++)
        {
            var newRow = col;
            var newCol = height - (row + 1);

            dst[newCol, newRow] = src[col, row];
        }

        return dst;
    }
}