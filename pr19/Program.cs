var lines = File.ReadAllLines("TextFile1.txt");

Console.WriteLine(First(lines));
Console.WriteLine(Second(lines));

int First(string[] lines)
{
    var rulesets = lines.TakeWhile(x => !string.IsNullOrWhiteSpace(x)).Select(RuleSet.Parse).ToDictionary(x => x.Name);
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
    var rulesets = lines.TakeWhile(x => !string.IsNullOrWhiteSpace(x)).Select(RuleSet.Parse).ToDictionary(x => x.Name);
    var all = "xmas".Select(c => Enumerable.Range(1, 4000).Select(i => "{" + c + "=" + i + "}").Select(Xmas.Parse).ToList()).ToList();
    var result = 1L;
    foreach (var xmass in all)
    {
        result *= xmass.Count(xmas =>
        {
            var result = true;
            foreach (var key in rulesets.Keys)
            {
                var next = key;

                var apply =  rulesets[next].Apply(xmas);
                if (next == "R")
                {
                    result = false;
                    break;
                }
            }

            return result;
        });
    }
    return result;
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
    internal static Rule Parse(string s) => new Rule
    {
        splits = s.Split(new[] { '<', '>', ':' }, StringSplitOptions.RemoveEmptyEntries),
        Name = s
    };

    internal string Apply(Xmas xmas)
    {
        if (splits.Length == 1)
            return splits[0];
        if (!xmas.Dict.ContainsKey(splits[0]))
            return "";
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

    internal string Apply(Xmas xmas) => Rules.Select(x => x.Apply(xmas)).FirstOrDefault(x => x != "");

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