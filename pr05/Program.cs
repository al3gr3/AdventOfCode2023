using System.Data;

var lines = File.ReadAllLines("TextFile1.txt");
var result = First(lines);
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

    var groups = Prepare(lines);
    groups.ForEach(grp =>
    {
        seeds = HandleGroup2(grp, seeds);
    });

    return seeds.Select(x => x.First()).Min();
}

List<List<long>> HandleGroup2(List<Mapping> currentGroup, List<List<long>> seeds)
{
    var result = seeds.Select(x => x.Select(y => y).ToList()).ToList();
    currentGroup.ForEach(map =>
    {
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

    var groups = Prepare(lines);
    groups.ForEach(grp =>
    {
        seeds = HandleGroup(grp, seeds);
    });

    return seeds.Min();
}

List<List<Mapping>> Prepare(string[] lines)
{
    var result = new List<List<Mapping>>();
    var currentGroup = new List<Mapping>();
    lines.Skip(2).Where(x => !string.IsNullOrEmpty(x)).ToList().ForEach(line =>
    {
        if (line.Contains("-to-"))
        {
            result.Add(currentGroup);
            currentGroup = new List<Mapping>();
        }
        else
            currentGroup.Add(ParseMapping(line));
    });
    result.Add(currentGroup);
    return result;
}

Mapping ParseMapping(string line)
{
    var splits = line.Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries)
        .Select(x => long.Parse(x))
        .ToList();
    return new Mapping
    {
        Dest = splits[0],
        Source = splits[1],
        Range = splits[2],
    };
}

List<long> HandleGroup(List<Mapping> currentGroup, List<long> seeds)
{
    var result = seeds.Select(x => x).ToList();
    currentGroup.ForEach(map =>
    {
        for (int i = 0; i < seeds.Count; i++)
        {
            if (map.Source <= seeds[i] && seeds[i] <= map.Source + map.Range)
                result[i] = seeds[i] + (map.Dest - map.Source); 
        }
    });
    return result;
}

class Mapping
{
    internal long Dest;
    internal long Source;
    internal long Range;
}