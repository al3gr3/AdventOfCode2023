var lines = File.ReadAllLines("TextFile2.txt");
var rulesets = lines.TakeWhile(x => !string.IsNullOrWhiteSpace(x)).Select(RuleSet.Parse).ToDictionary(x => x.Name);

Console.WriteLine(First(lines));
Console.WriteLine(Second(lines));

int First(string[] lines)
{
    var xmass = lines.Skip(rulesets.Count + 1).Select(Xmas.Parse).ToList();
    var result = xmass.Where(xmas =>
    {
        var next = "in";

        while (!"AR".Contains(next))
            next = rulesets[next].Apply(xmas);

        return next == "A";
    }).Sum(xmas => xmas.Sum());
    return result;
}

long Second(string[] lines)
{
    var ranges = "xmas".Select(c => new Range { Start = 1, Finish = 4000, Name = "" + c }).ToList();
    var result = Recurse("in", ranges);
    return result;
}

long Recurse(string ruleName, List<Range> ranges, int? skip = null)
{
    if (ranges.Any(x => x.IsEmpty()))
        return 0;

    if (ruleName == "A")
        return ranges.Select(x => x.Finish - x.Start + 1).Aggregate(1L, (s, n) => s *= n);

    if (ruleName == "R")
        return 0;

    var rule = rulesets[ruleName].Rules.Skip(skip ?? 0).First();
    if (rule.splits.Length == 1)
        return Recurse(rule.splits[0], ranges);
    var twoRanges = rule.TwoRanges();

    var trueRanges = ranges.Select(x => x.Intersect(twoRanges.Item1)).ToList();
    var falseRanges = ranges.Select(x => x.Intersect(twoRanges.Item2)).ToList();

    var a = Recurse(rule.splits.Last(), trueRanges);
    var b = Recurse(ruleName, falseRanges, (skip ?? 0) + 1);

    return a + b;
}

class Range
{
    internal string Name;
    internal int Start, Finish;

    internal Range Intersect(Range range)
    {
        if (range.Name != this.Name)
            return this;

        return new Range { Start = Math.Max(Start, range.Start), Finish = Math.Min(Finish, range.Finish), Name = this.Name };
    }

    internal bool IsEmpty() => this.Start > this.Finish;
}

class Xmas
{
    internal Dictionary<string, int> Dict;

    internal static Xmas Parse(string s)
    {
        var splits = s.Split(new[] { ',', '}', '{' }, StringSplitOptions.RemoveEmptyEntries);
        var result = new Xmas
        {
            Dict = new Dictionary<string, int>()
        };
        foreach (var line in splits)
        {
            var lineSplits = line.Split(new[] { '=' }, StringSplitOptions.RemoveEmptyEntries);
            result.Dict[lineSplits[0]] = int.Parse(lineSplits[1]);
        }
        return result;
    }

    internal int Sum() => Dict.Values.Sum();
}

class Rule
{
    internal string[] splits;
    internal string Name;
    internal string C => splits[0];
    internal static Rule Parse(string s) => new Rule
    {
        splits = s.Split(new[] { '<', '>', ':' }, StringSplitOptions.RemoveEmptyEntries),
        Name = s
    };

    internal (Range, Range) TwoRanges()
    {
        return Name.Contains("<")
            ? (new Range { Start = 1, Finish = int.Parse(splits[1]) - 1, Name = C }, new Range { Start = int.Parse(splits[1]), Finish = 4000, Name = C })
            : (new Range { Start = int.Parse(splits[1]) + 1, Finish = 4000, Name = C }, new Range { Start = 1, Finish = int.Parse(splits[1]), Name = C });
    }

    internal string Apply(Xmas xmas)
    {
        if (splits.Length == 1)
            return splits[0];
        if (Name.Contains("<") && xmas.Dict[splits[0]] < int.Parse(splits[1]))
            return splits[2];
        if (Name.Contains(">") && xmas.Dict[splits[0]] > int.Parse(splits[1]))
            return splits[2];

        return "";
    }
}

class RuleSet
{
    internal List<Rule> Rules;
    internal string Name;

    internal string Apply(Xmas xmas) => Rules.Select(x => x.Apply(xmas)).First(x => x != "");

    internal static RuleSet Parse(string s)
    {
        var splits = s.Split(new[] { ',', '}', '{' }, StringSplitOptions.RemoveEmptyEntries);
        var result = new RuleSet
        {
            Rules = splits.Skip(1).Select(Rule.Parse).ToList(),
            Name = splits[0],
        };
        return result;
    }
}