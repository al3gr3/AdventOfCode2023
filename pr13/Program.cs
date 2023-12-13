var lines = File.ReadAllLines("TextFile1.txt").ToList();
lines.Add("");
Console.WriteLine(Solve(lines, 0));
Console.WriteLine(Solve(lines, 1));

long Solve(List<string> lines, int numberOfDiffs)
{
    var result = 0L;
    var set = new List<string>();
    foreach (var line in lines)
        if (string.IsNullOrEmpty(line))
        {
            result += 100 * FindHorisontal(set, numberOfDiffs);
            result += FindHorisontal(Transpose(set), numberOfDiffs);
            set.Clear();
        }
        else
            set.Add(line);
    return result;
}

List<string> Transpose(List<string> set) => set.First().Select((_, i) =>
    new string(set.Select(x => x[i]).ToArray())).ToList();

int FindHorisontal(List<string> set, int numberOfDiffs) => set
    .Select((_, i) => i) // .Skip(1) is not needed, because it will add 0 to sum anyway
    .Where(i => set.Take(i).Reverse().Zip(set.Skip(i)).Sum(NumberOfDiffs) == numberOfDiffs)
    .Sum();

int NumberOfDiffs((string First, string Second) x) => 
    x.First.Zip(x.Second).Count(p => p.First != p.Second);