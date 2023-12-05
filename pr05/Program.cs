using System.Data;

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

    var groups = Prepare(lines);
    groups.ForEach(grp =>
    {
        seeds = HandleGroup2(grp, seeds);
    });

    return seeds.Select(x => x.First()).Min();
}

List<List<long>> HandleGroup2(List<Mapping> currentGroup, List<List<long>> seeds)
{
    var result = new List<List<long>>();

    var unmappedSeeds = seeds.Select(x => x.Select(y => y).ToList()).ToList();

    currentGroup.ForEach(map =>
    {
        var nextUnmappedSeeds = new List<List<long>>();
        unmappedSeeds.ForEach(unmappedSeed =>
        {
            var a = unmappedSeed.First();
            var b = unmappedSeed.First() + unmappedSeed.Last() - 1;

            var sourceA = map.Source;
            var sourceB = map.Source + map.Range - 1;

            if (sourceA > b || a > sourceB)
            {
                // no intersection
                nextUnmappedSeeds.Add(unmappedSeed);
            }
            else if (sourceA > a && sourceB >= b)
            {
                // 01234567
                // a     b
                //   source
                result.Add(new long[] { map.Dest, b - sourceA + 1 }.ToList());
                nextUnmappedSeeds.Add(new long[] { a, sourceA - a  }.ToList());
            }
            else if (sourceA <= a && sourceB < b)
            {
                //   01234567
                //   a      b
                //   source
                result.Add(new long[] { a + (map.Dest - map.Source), sourceB - a + 1 }.ToList());
                nextUnmappedSeeds.Add(new long[] { sourceB + 1, b - sourceB }.ToList());
            }
            else if (sourceA <= a && b <= sourceB)
            {
                //   01234567
                //    a  b
                //   source
                result.Add(new long[] { a + (map.Dest - map.Source), b - a + 1 }.ToList());
            }
            else if (a < sourceA && sourceB < b)
            {
                //    0123456789 11
                //    a          b
                //       source
                result.Add(new long[] { map.Dest, sourceB - sourceA + 1 }.ToList());
                nextUnmappedSeeds.Add(new long[] { a, sourceA - a }.ToList());
                nextUnmappedSeeds.Add(new long[] { sourceB + 1, b - sourceB }.ToList());
            }
        });
        unmappedSeeds = nextUnmappedSeeds.Select(x => x.Select(y => y).ToList()).ToList();
    });

    result.AddRange(unmappedSeeds);

    return result;
}

bool Intersects(List<long> seed, Mapping map)
{
    var a = seed.First();
    var b = seed.First() + seed.Last();

    var sourceA = map.Source;
    var sourceB = map.Source + map.Range;

    return !(sourceA > b || a > sourceB);
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
            if (currentGroup.Any())
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