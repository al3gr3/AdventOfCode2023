var lines = File.ReadAllLines("TextFile2.txt");
Console.WriteLine(First(lines));
Console.WriteLine(Second(lines));

int First(string[] lines)
{
    return lines.First().Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(split => Hash(split)).Sum();
}

int Hash(string split)
{
    return split.Aggregate(0, (s, n) => s = (s + (byte)n) * 17 % 256);
}

int Second(string[] lines)
{
    return 0;
}