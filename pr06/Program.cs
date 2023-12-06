var lines = File.ReadAllLines("TextFile1.txt");
var result = First(lines);
Console.WriteLine(result);

long First(string[] lines)
{
    /*
Time:      7  15   30
Distance:  9  40  200     
     */
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

    var result = tds.Select(td =>
    {
        var records = Enumerable.Range(1, td.First - 1)
            .Select(t => t * (td.First - t))
            .Where(d => d > td.Second);
        return records.Count();
    }).Aggregate(1, (s, n) => s *= n);
    return result;

}