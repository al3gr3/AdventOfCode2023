var lines = File.ReadAllLines("TextFile1.txt");

var result = Second(lines);
Console.WriteLine(result);

long First(string[] lines)
{
    var times = lines
        .First()
        .Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries)
        .Skip(1)
        .Select(x => int.Parse(x))
        .ToList();

    var distances = lines
        .Last()
        .Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries)
        .Skip(1)
        .Select(x => int.Parse(x))
        .ToList();

    var tds = times.Zip(distances);

    var result = tds
        .Select(td => CountWays(td.First, td.Second))
        .Aggregate(1L, (s, n) => s *= n);
    return result;
}

long Second(string[] lines)
{
    var time = long.Parse(string.Join("",
        lines
            .First()
            .Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries)
            .Skip(1)));

    var distance = long.Parse(string.Join("",
        lines
            .Last()
            .Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries)
            .Skip(1)));

    return CountWays(time, distance);
}

long CountWays(long time, long distance)
{
    var result = 0;
    for (long t = 1; t < time; t++)
        if (t * (time - t) > distance)
            result++;
    return result;
}
