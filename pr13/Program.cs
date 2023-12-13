var lines = File.ReadAllLines("TextFile2.txt").ToList();
lines.Add("");
Console.WriteLine(First(lines));

long First(List<string> lines)
{
    var result = 0L;
    var set = new List<string>();
    foreach (var line in lines)
    {
        if (string.IsNullOrEmpty(line))
        {
            result += 100 * FindHorisontal(set);
            set = Rotate(set);
            result += FindHorisontal(set);
            set = new List<string>();
        }
        else
            set.Add(line);
    }
    return result;
}

List<string> Rotate(List<string> set) =>
    Enumerable.Range(0, set.First().Length)
        .Select(i => new string(set.Select(x => x[i]).ToArray())).ToList();

long FindHorisontal(List<string> set)
{
    var result = 0L;
    // look for horisontal
    for (var i = 1; i < set.Count - 1; i++)
        if (set.Take(i).Reverse().Zip(set.Skip(i)).TakeWhile(x => !string.IsNullOrEmpty(x.First) && !string.IsNullOrEmpty(x.Second)).All(x => x.First == x.Second))
            result = Math.Max(i, result);

    return result;
}