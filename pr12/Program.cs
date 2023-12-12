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
    var numbers = splits.Last().Split(',').Select(x => int.Parse(x)).ToList();

    dict = new Dictionary<string, long>();

    return Recurse(splits.First() + '.', numbers);
}

long Recurse(string s, List<int> numbers)
{
    var keyIntoDict = $"{s.Length}|{numbers.Count}";
    
    if (dict.ContainsKey(keyIntoDict))
        return dict[keyIntoDict];

    // these are actually not needed
    /*
    if (numbers.Any() && s.All(x => x == '.'))
        return 0;

    if (!numbers.Any() && s.Any(x => x == '#'))
        return 0;
    */
    
    if (!numbers.Any() && !s.Any(x => x == '#'))
        return dict[keyIntoDict] = 1;

    // if numbers is empty, numbers.FirstOrDefault will return 0 and PrepareAllPosibilities will return empty list, thus making its sum 0
    var possibilities = PrepareAllPosibilities(s, numbers.FirstOrDefault()); 

    var result = possibilities.Sum(x => Recurse(x, numbers.Skip(1).ToList()));

    return dict[keyIntoDict] = result;
}

IEnumerable<string> PrepareAllPosibilities(string s, int v) => Enumerable
    .Range(0, s.Length)
    .Where(i => CanFirstStartFrom(s, i, v))
    .Select(i => s[(i + v + 1)..]); // can leave out the "index<length" check because we appended '.' to initial s

bool CanFirstStartFrom(string s, int i, int v)
{
    if (i + v > s.Length)
        return false;
    
    if (i + v < s.Length && s[i + v] == '#') // if there is # right after v
        return false;

    if (!s.Skip(i).Take(v).All(x => x == '?' || x == '#'))
        return false;

    if (s.Take(i).Any(x => x == '#')) // if there is # anywhere before v
        return false;

    return true;
}