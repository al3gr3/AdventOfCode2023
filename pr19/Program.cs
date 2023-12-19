var lines = File.ReadAllLines("TextFile2.txt");
var rulesets = lines.TakeWhile(x => !string.IsNullOrWhiteSpace(x)).Select(RuleSet.Parse).ToDictionary(x => x.Name);
var xmass = lines.Skip(rulesets.Count + 1).Select(Xmas.Parse).Where(xmas =>
{
    var next = "in";

    while (!"AR".Contains(next))
        next = rulesets[next].Apply(xmas);

    return next == "A";
}).Sum(xmas => xmas.Sum());


Console.WriteLine(xmass);
Console.WriteLine(Second(lines));

long First(string[] lines)
{
    return 0;
}

long Second(string[] lines)
{
    return 0;
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
    internal string Name;
    internal static Rule Parse(string s)
    {
        var result = new Rule
        {
            Name = s,
        };
        return result;
    }

    internal string Apply(Xmas xmas)
    {
        var splits = Name.Split(new[] { '<', '>', ':' }, StringSplitOptions.RemoveEmptyEntries);
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