var lines = File.ReadAllLines("TextFile2.txt").ToList();
Console.WriteLine(First(lines));
//Console.WriteLine(Second(lines));

int First(List<string> lines) => Transpose(lines).Sum(CalculateLine);

int CalculateLine(string arg)
{
    var splits = arg.Split('#');
    var result = 0;
    var startIndex = 0;
    foreach(var split in splits)
    { 
        result += Enumerable.Range(0, split.Count(c => c == 'O')).Sum(x => arg.Length - startIndex - x);
        startIndex += split.Length + 1;
    }
    return result;
}
List<string> Transpose(List<string> set) => set.First().Select((_, i) =>
    new string(set.Select(x => x[i]).ToArray())).ToList();

long Second(string[] lines)
{
    return 0;
}