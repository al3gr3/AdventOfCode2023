var lines = File.ReadAllLines("TextFile1.txt");
var result = First(lines);
Console.WriteLine(result);

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

        for (int i = 0; i < result.Count; i++)
        {
            if (splits[1] <= seeds[i] && seeds[i] <= splits[1] + splits[2])
                result[i] = seeds[i] + (splits[0] - splits[1]); 
        }
    });
    return result;
}