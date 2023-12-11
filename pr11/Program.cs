var lines = File.ReadAllLines("TextFile1.txt");
lines = Expand(lines);
Print(lines);

string[] Expand(string[] lines)
{
    var result = lines.SelectMany(x => x.Contains('#') ? new[] { x } : new[] { x, x }).ToArray();

    for (var i = lines.First().Length - 1; i >= 0; i--)
        if (lines.All(x => x[i] == '.'))
            for (var j = 0; j < result.Length; j++)
                result[j] = result[j].Insert(i, ".");
    return result;
}

Console.WriteLine(First(lines));

int First(string[] lines)
{
    var galaxies = new List<(int, int)>();
    for (int i = 0; i < lines.Length; i++)
        for (int j = 0; j < lines[i].Length; j++)
            if (lines[i][j] == '#')
                galaxies.Add((i, j));

    var result = 0;
    foreach (var g in galaxies)
        foreach (var f in galaxies)
            result += Math.Abs(g.Item1 - f.Item1) + Math.Abs(g.Item2 - f.Item2);

    return result/2;
}


void Print(string[] distances)
{
    for (int i = 0; i < distances.Length; i++)
    {
        for (int j = 0; j < distances[i].Length; j++)
            Console.Write(distances[i][j]);
        Console.WriteLine();
    }
    Console.WriteLine();
}
//Console.WriteLine(Second(lines));