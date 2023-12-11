var lines = File.ReadAllLines("TextFile1.txt");
Console.WriteLine(Second(lines, 2));
Console.WriteLine(Second(lines, 1000_000));

long Second(string[] lines, long age)
{
    var agedRows = Enumerable.Range(0, lines.Length).ToList();
    var agedCols = Enumerable.Range(0, lines.First().Length).ToList();

    var galaxies = new List<(int, int)>();
    for (int i = 0; i < lines.Length; i++)
        for (int j = 0; j < lines[i].Length; j++)
            if (lines[i][j] == '#')
            {
                agedRows.Remove(i);
                agedCols.Remove(j);
                galaxies.Add((i, j));
            }

    var result = 0L;
    foreach (var g in galaxies)
        foreach (var f in galaxies)
        {
            var agedRowsCount = agedRows.Count(r => Math.Min(g.Item1, f.Item1) < r && r < Math.Max(g.Item1, f.Item1));
            var agedColsCount = agedCols.Count(c => Math.Min(g.Item2, f.Item2) < c && c < Math.Max(g.Item2, f.Item2));
            result += Math.Abs(g.Item1 - f.Item1) + Math.Abs(g.Item2 - f.Item2) + (agedRowsCount + agedColsCount) * (age - 1);
        }

    return result / 2;
}