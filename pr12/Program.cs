var dict = new Dictionary<string, long>();

var lines = File.ReadAllLines("TextFile1.txt");
Console.WriteLine(First(lines));
Console.WriteLine(Second(lines));

long First(string[] lines) => lines.Sum(Solve);

long Second(string[] lines) => lines.Select(line =>
{
    var splits = line.Split(' ');
    return string.Join("?", Enumerable.Repeat(splits.First(), 5))
        + ' '
        + string.Join(",", Enumerable.Repeat(splits.Last(), 5));
}).Sum(Solve);

long Solve(string l)
{
    var splits = l.Split(' ');
    var s = splits.First();
    var numbers = splits.Last().Split(',').Select(x => int.Parse(x)).ToList();

    dict = new Dictionary<string, long>();

    return Recurse(s, numbers);
}

long Recurse(string s, List<int> numbers)
{
    var keyIntoDict = "" + s.Length + "|" + numbers.Count;
    
    if (dict.ContainsKey(keyIntoDict))
        return dict[keyIntoDict];

    if (numbers.Any() && s.All(x => x == '.'))
        return 0;

    if (!numbers.Any() && s.Any(x => x == '#'))
        return 0;

    if (!numbers.Any() && s.All(x => x == '.' || x == '?'))
        return 1;

    var possibilities = PrepareAllPosibilities(s, numbers.First());

    var result = possibilities.Sum(x => Recurse(x, numbers.Skip(1).ToList()));

    dict[keyIntoDict] = result;
    return result;
}

IEnumerable<string> PrepareAllPosibilities(string s, int v) => Enumerable
    .Range(0, s.Length)
    .Where(i => CanFirstStartFrom(s, i, v))
    .Select(i => i + v + 1 < s.Length ? s.Substring(i + v + 1) : "");

bool CanFirstStartFrom(string s, int i, int v)
{
    if (i + v > s.Length)
        return false;

    if (i + v < s.Length && s[i + v] == '#')
        return false;

    if (!s.Skip(i).Take(v).All(x => x == '?' || x == '#'))
        return false;

    if (s.IndexOf('#') > -1 && s.IndexOf('#') < i)
        return false;

    return true;
}