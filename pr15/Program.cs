var lines = File.ReadAllLines("TextFile2.txt");
Console.WriteLine(First(lines));
Console.WriteLine(Second(lines));

int First(string[] lines) => lines.First().Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(Hash).Sum();

int Hash(string split) => split.Aggregate(0, (s, n) => s = (s + (byte)n) * 17 % 256);

int Second(string[] lines)
{
    var boxes = Enumerable.Range(0, 256).Select(x => new List<Entry>()).ToArray();
    foreach (var split in lines.First().Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
    {
        var parts = split.Split(new[] { "=", "-" }, StringSplitOptions.RemoveEmptyEntries);
        var hash = Hash(parts.First());
        if (split.Contains('-'))
            boxes[hash].RemoveAll(x => x.Name == parts.First());
        else
        {
            var box = boxes[hash].FirstOrDefault(x => x.Name == parts.First());
            if (box == null)
            {
                box = new Entry { Name = parts.First() };
                boxes[hash].Add(box);
            }
            box.FocalLength = int.Parse(parts.Last());
        }
    }
    return boxes.SelectMany((box, bi) => box.Select((lens, li) => lens.FocalLength * (li + 1) * (bi + 1))).Sum();
}

class Entry
{
    internal string Name; 
    internal int FocalLength; 
}