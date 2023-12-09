var lines = File.ReadAllLines("TextFile1.txt")
    .Select(line => line.Split(' ').Select(x => int.Parse(x)).ToList()).ToList();

Console.WriteLine(First(lines));
Console.WriteLine(Second(lines));
Console.WriteLine(Second2(lines));

int First(List<List<int>> lines) => lines.Select(x => Solve(x)).Sum();
int Second(List<List<int>> lines) => lines.Select(x => SolveSecond(x)).Sum();
int Second2(List<List<int>> lines) => lines.Select(x => Solve(x.AsEnumerable().Reverse().ToList())).Sum(); // from reddit

int Solve(List<int> line) => CreateLines(line).Select(x => x.Last()).Sum();

int SolveSecond(List<int> line) => CreateLines(line)
    .AsEnumerable().Reverse() // needed for LINQ reverse
    .Aggregate(0, (s, n) => s = n.First() - s);

List<List<int>> CreateLines(List<int> line)
{
    var current = line;
    var all = new List<List<int>>();
    all.Add(line);

    while (!current.All(x => x == 0))
    {
        var next = current.Skip(1).Zip(current).Select(p => p.First - p.Second).ToList();

        all.Add(next);
        current = next;
    }

    return all;
}