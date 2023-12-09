﻿var lines = File.ReadAllLines("TextFile1.txt")
    .Select(line => line.Split(' ').Select(x => int.Parse(x)).ToList()).ToList();

var result = Second(lines);
Console.WriteLine(result);

int First(List<List<int>> lines) => lines.Select(x => Solve(x)).Sum();
int Second(List<List<int>> lines) => lines.Select(x => SolveSecond(x)).Sum();

int Solve(List<int> line)
{
    var all = CreateLines(line);

    var result = all.Select(x => x.Last()).Sum();
    return result;
}

List<List<int>> CreateLines(List<int> line)
{
    var current = line;
    var all = new List<List<int>>();
    all.Add(line);

    while (!current.All(x => x == 0))
    {
        var next = new List<int>();
        for (int i = 1; i < current.Count; i++)
            next.Add(current[i] - current[i - 1]);

        all.Add(next);
        current = next;
    }

    return all;
}

int SolveSecond(List<int> line)
{
    var all = CreateLines(line);
    all.Reverse();
    var result = all.Aggregate(0, (s, n) => s = n.First() - s);
    return result;
}

