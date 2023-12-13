var lines = File.ReadAllLines("TextFile1.txt").ToList();
lines.Add("");
Console.WriteLine(Solve(lines, 0));
Console.WriteLine(Solve(lines, 1));

long Solve(List<string> lines, int numberOfErrors)
{
    var result = 0L;
    var set = new List<string>();
    foreach (var line in lines)
    {
        if (string.IsNullOrEmpty(line))
        {
            result += 100 * FindHorisontal(set, numberOfErrors);
            set = Transpose(set);
            result += FindHorisontal(set, numberOfErrors);
            set = new List<string>();
        }
        else
            set.Add(line);
    }
    return result;
}

List<string> Transpose(List<string> set) =>
    Enumerable.Range(0, set.First().Length)
        .Select(i => new string(set.Select(x => x[i]).ToArray())).ToList();

long FindHorisontal(List<string> set, int numberOfErrors)
{
    var result = 0L;
    for (var i = 1; i < set.Count; i++)
        if (set.Take(i).Reverse().Zip(set.Skip(i))
            .Select(NumberOfErrors)
            .Sum() == numberOfErrors)
            result += i;

    return result;
}

int NumberOfErrors((string First, string Second) x) => 
    x.First.Zip(x.Second).Count(p => p.First != p.Second);