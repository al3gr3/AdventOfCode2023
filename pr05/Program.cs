var lines = File.ReadAllLines("TextFile1.txt");
var result = Second(lines);
Console.WriteLine(result);

long Second(string[] lines)
{
    var seeds = lines
        .First()
        .Split(new[] { "seeds:", " " }, StringSplitOptions.RemoveEmptyEntries)
        .Select((x, i) => new { Index = i, X = long.Parse(x) })
        .GroupBy(el => el.Index / 2)
        .Select(grp => grp.Select(x => x.X).ToList())
        .ToList();

    var currentGroupName = "";
    var currentGroup = new List<string>();
    lines.Skip(2).Where(x => !string.IsNullOrEmpty(x)).ToList().ForEach(line =>
    {
        if (line.Contains("-to-"))
        {
            seeds = HandleGroup2(currentGroup, seeds);
            currentGroup = new List<string>();
            currentGroupName = line;
        }
        else
            currentGroup.Add(line);
    });
    seeds = HandleGroup2(currentGroup, seeds);

    return seeds.Select(x => x.First()).Min();
}

List<List<long>> HandleGroup2(List<string> currentGroup, List<List<long>> seeds)
{
    var result = seeds.Select(x => x.Select(y => y).ToList()).ToList();
    currentGroup.ForEach(map =>
    {
        var splits = map.Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries)
            .Select(x => long.Parse(x))
            .ToList();

        var dest = splits[0];
        var source = splits[1];
        var range = splits[2];
        
        for (int i = 0; i < seeds.Count; i++)
        {
            var a = seeds[i].First();
            var b = seeds[i].First() + seeds[i].Last();

        }
    });
    return result;
}

long First(string[] lines)
{
    var seeds = lines
        .First()
        .Split(new[] { "seeds:", " " }, StringSplitOptions.RemoveEmptyEntries)
        .Select(x => long.Parse(x))
        .ToList();

    var currentGroupName = "";
    var currentGroup = new List<string>();
    lines.Skip(2).Where(x => !string.IsNullOrEmpty(x)).ToList().ForEach(line =>
    {
        if (line.Contains("-to-"))
        {
            seeds = HandleGroup(currentGroup, seeds);
            currentGroup = new List<string>();
            currentGroupName = line;
        }
        else
            currentGroup.Add(line);
    });
    seeds = HandleGroup(currentGroup, seeds);

    return seeds.Min();
}

List<long> HandleGroup(List<string> currentGroup, List<long> seeds)
{
    var result = seeds.Select(x => x).ToList();
    currentGroup.ForEach(map =>
    {
        var splits = map.Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries)
            .Select(x => long.Parse(x))
            .ToList();

        var dest = splits[0];
        var source = splits[1];
        var range = splits[2];

        for (int i = 0; i < seeds.Count; i++)
        {
            if (source <= seeds[i] && seeds[i] <= source + range)
                result[i] = seeds[i] + (dest - source); 
        }
    });
    return result;
}