var lines = File.ReadAllLines("TextFile2.txt");

var g = new Dictionary<string, List<string>>();

foreach (var line in lines)
{
    var splits = line.Split(new[] { ':', ' ' }, StringSplitOptions.RemoveEmptyEntries);
    var from = splits.First();
    foreach (var to in splits.Skip(1))
    {
        //Console.WriteLine($"{from} -> {to}");
        Add(to, from);
        Add(from, to);
    }
}

//https://dreampuf.github.io/GraphvizOnline 
//neato

Remove("bbg", "htb");
Remove("dlk", "pjj");
Remove("pcc", "htj");

void Remove(string v1, string v2)
{
    g[v1].Remove(v2);
    g[v2].Remove(v1);
}

var color = new List<string>();

var queue = new Queue<string>();
queue.Enqueue(g.Keys.First());
color.Add(g.Keys.First());

while (queue.Any())
    foreach (var s in g[queue.Dequeue()])
        if (!color.Contains(s))
        {
            color.Add(s);
            queue.Enqueue(s);
        }

Console.WriteLine(color.Count() * (g.Keys.Count - color.Count()));

void Add(string from, string to)
{
    if (!g.ContainsKey(from))
        g[from] = new List<string>();

    g[from].Add(to);
}