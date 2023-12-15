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
        var isRemoval = split.Contains("-");
        var hash = Hash(parts.First());
        if (isRemoval)
        {
            var itemToRemove = boxes[hash].FirstOrDefault(x => x.Name == parts.First());
            if (itemToRemove != null)
                boxes[hash].Remove(itemToRemove);
        }
        else
        {
            var itemToAdd = boxes[hash].FirstOrDefault(x => x.Name == parts.First());
            if (itemToAdd == null)
            {
                itemToAdd = new Entry { Name = parts.First() };
                boxes[hash].Add(itemToAdd);
            }

            itemToAdd.FocalLength = int.Parse(parts.Last());
        }
    }
    var result = boxes.Select((b, i) => (i + 1) * b.Select((box, index) => box.FocalLength * (index + 1)).Sum()).Sum();
    return result;
}

class Entry
{
    internal string Name; 
    internal int FocalLength; 
}