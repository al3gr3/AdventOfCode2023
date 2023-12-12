var dict = new Dictionary<string, long>();

var lines = File.ReadAllLines("TextFile1.txt");
Console.WriteLine(First(lines));
//Console.WriteLine(Second(lines));

long First(string[] lines)
{
    var result = 0L;
    foreach (var line in lines)
    {
        dict = new Dictionary<string, long>();
        result += Solve(line);
    }
    return result;
}

long Second(string[] lines)
{
    var result = 0L;
    foreach (var line in lines)
    {
        var splits = line.Split(' ');
        var newLine = string.Join("?", Enumerable.Range(0, 5).Select(x => splits.First()))
            + ' '
            + string.Join(",", Enumerable.Range(0, 5).Select(x => splits.Last()));
        dict = new Dictionary<string, long>(); 
        result += Solve(newLine);
    }
    return result;
}

long Solve(string l)
{
    var splits = l.Split(' ');
    var s = splits.First();
    var numbers = splits.Last().Split(',').Select(x => int.Parse(x)).ToList();

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

List<string> PrepareAllPosibilities(string s, int v)
{
    s = s.Trim('.');
    var result = new List<string>();
    for (int i = 0; i < s.Length; i++)
        if (CanFirstStartFrom(s, i, v))
            result.Add(i + v + 1 < s.Length ? s.Substring(i + v + 1) : "");

    return result;
}

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