var lines = File.ReadAllLines("TextFile1.txt");
var dict = new Dictionary<string, Node>();
foreach (var line in lines.Skip(2))
{
    //AAA = (BBB, CCC)
    var splits = line.Split(new[] { ' ', '=', '(', ')', ',' }, StringSplitOptions.RemoveEmptyEntries);

    dict[splits[0]] = new Node
    {
        Left = splits[1],
        Right = splits[2],
    };
}

var result = First(dict, lines.First());
Console.WriteLine(result);

int First(Dictionary<string, Node> dict, string path)
{
    var current = "AAA";
    var result = 0;
    while (current != "ZZZ")
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

internal class Node
{
    internal string Left;
    internal string Right;
}