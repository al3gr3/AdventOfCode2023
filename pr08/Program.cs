var lines = File.ReadAllLines("TextFile1.txt");
var dict = new Dictionary<string, Node>();
foreach (var line in lines.Skip(2))
{
    var splits = line.Split(new[] { ' ', '=', '(', ')', ',' }, StringSplitOptions.RemoveEmptyEntries);

    dict[splits[0]] = new Node
    {
        Left = splits[1],
        Right = splits[2],
    };
}

var result = Second(dict, lines.First());
Console.WriteLine(result);

long First(Dictionary<string, Node> dict, string path) =>
    CountCycle(dict, path, "AAA", "ZZZ");

long Second(Dictionary<string, Node> dict, string path)
{
    var current = dict.Keys.Where(x => x.EndsWith("A")).ToList();
    var cycles = current.Select(x => CountCycle(dict, path, x, "Z"));
    return cycles.Aggregate((s, n) => s = LCM(s, n));
}

long CountCycle(Dictionary<string, Node> dict, string path, string current, string finish)
{
    var result = 0;
    while (!current.EndsWith(finish))
    {
        var next = path[result % path.Length];
        result++;
        if (next == 'R')
            current = dict[current].Right;
        else
            current = dict[current].Left;
    }
    return result;
}

long LCM(long a, long b) => a * b / GCD(a, b);

long GCD(long a, long b)
{
    (a, b) = (Math.Min(a, b), Math.Max(a, b));

    while (b != 0)
        (a, b) = (b, a % b);

    return a;
}

internal class Node
{
    internal string Left;
    internal string Right;
}