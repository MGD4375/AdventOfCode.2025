using System.Numerics;

namespace AdventOfCode._2025.Day1;

public static class Day8
{
    public static void Part1()
    {
        const int numberToTake = 1000;

        var boxes = File.ReadAllLines("./Day8/input.txt")
            .Select(line =>
            {
                var split = line.Split(',');
                return new Vector3(int.Parse(split[0]), int.Parse(split[1]), int.Parse(split[2]));
            }).ToList();

        var allPairs = new List<(Vector3 boxA, Vector3 boxB, float distance)>();
        for (var i = 0; i < boxes.Count; i++)
        for (var j = i + 1; j < boxes.Count; j++)
        {
            var distance = Vector3.Distance(boxes[i], boxes[j]);
            allPairs.Add((boxes[i], boxes[j], distance));
        }

        var connections = allPairs
            .OrderBy(pair => pair.distance)
            .Take(numberToTake)
            .ToList();

        //  I originally built a directional adjacency list by accident. I still think this is kind of an acyclic graph, but at least you can travel both ways now 
        var adjacencyList = new Dictionary<Vector3, List<Vector3>>();
        foreach (var connection in connections)
        {
            if (!adjacencyList.ContainsKey(connection.boxA)) adjacencyList[connection.boxA] = [];
            adjacencyList[connection.boxA].Add(connection.boxB);

            if (!adjacencyList.ContainsKey(connection.boxB)) adjacencyList[connection.boxB] = [];
            adjacencyList[connection.boxB].Add(connection.boxA);
        }

        // I need to do a search to find connected nodes
        var visited = new List<Vector3>();
        var components = new List<List<Vector3>>();

        foreach (var box in boxes)
            if (!visited.Contains(box))
            {
                var component = new List<Vector3>();
                DepthFirstSearch(box, adjacencyList, visited, component);
                components.Add(component);
            }

        var count = components
            .OrderByDescending(c => c.Count)
            .Take(3)
            .Aggregate(1, (acc, list) => acc * list.Count);

        Console.WriteLine(count);
    }

    private static void DepthFirstSearch(
        Vector3 node,
        Dictionary<Vector3, List<Vector3>> adjacencyList,
        List<Vector3> visited,
        List<Vector3> component
    )
    {
        visited.Add(node);
        component.Add(node);

        if (adjacencyList.TryGetValue(node, out var value))
            foreach (var neighbor in value.Where(neighbor => !visited.Contains(neighbor)))
                DepthFirstSearch(neighbor, adjacencyList, visited, component);
    }
}