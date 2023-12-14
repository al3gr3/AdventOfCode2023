var lines = File.ReadAllLines("TextFile2.txt").ToList();
Console.WriteLine(First(lines));
//Console.WriteLine(Second(lines));

int First(List<string> lines)
{
    lines = Transpose(lines);

    var result = lines.Sum(CalculateLine);
    return result;
}

int CalculateLine(string arg)
{
    var splits = arg.Split('#');
    var result = 0;
    var startIndex = 0;
    foreach(var nextSplit in splits)
    { 
        result += Enumerable.Range(0, nextSplit.Count(x => x == 'O')).Select(x => arg.Length - startIndex - x).Sum();
        startIndex += nextSplit.Length + 1;
    }
    return result;
}
List<string> Transpose(List<string> set) => set.First().Select((_, i) =>
    new string(set.Select(x => x[i]).ToArray())).ToList();

long Second(string[] lines)
{
    return 0;
}