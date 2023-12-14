var lines = File.ReadAllLines("TextFile2.txt").ToList();
Console.WriteLine(First(lines));
Console.WriteLine(Second(lines));

int First(List<string> lines) => RotateCounterClockWise(lines).Select(Tilt).Sum(CalculateLine);

int CalculateLine(string arg) => arg.Select((c, i) => c == 'O' ? arg.Length - i : 0).Sum();

string Tilt(string arg)
{
    var splits = arg.Split('#');
    var result = "";
    foreach (var split in splits)
    {
        result += new string(Enumerable.Repeat('O', split.Count(c => c == 'O')).ToArray()) +
            new string(Enumerable.Repeat('.', split.Count(c => c == '.')).ToArray()) + "#";
    }

    var diff = result.Reverse().TakeWhile(x => x == '#').Count() -  arg.Reverse().TakeWhile(x => x == '#').Count();
    result = result.Substring(0, result.Length - diff);
    return result;
}

long Second(List<string> lines)
{
    var dict = new Dictionary<string, int>();

    var cycle = 0;
    var cycleLength = 0;
    while (true)
    {
        var key = lines.Aggregate("", (s, n) => s + n);

        if (dict.ContainsKey(key))
        {
            cycleLength = cycle - dict[key];
            Console.WriteLine($"cycle: {cycle}, previous: {dict[key]}");
            break;
        }

        dict[key] = cycle++;
        lines = Cycle(lines);
        Console.WriteLine("" + cycle + " " + lines.Sum(CalculateLine));
    }

    var remains = (1000000000 - cycle) % (cycleLength);

    for (var i = 0; i < remains +1; i++)
    {
        lines = Cycle(lines);
    }

    return lines.Sum(CalculateLine);
}

List<string> Cycle(List<string> lines)
{
    // north
    lines = RotateCounterClockWise(lines);
    lines = lines.Select(x => Tilt(x)).ToList();

    // west
    lines = RotateClockWise(lines);
    lines = lines.Select(x => Tilt(x)).ToList();

    //south
    lines = RotateClockWise(lines);
    lines = lines.Select(x => Tilt(x)).ToList();

    //east
    lines = RotateClockWise(lines);
    lines = lines.Select(x => Tilt(x)).ToList();

    lines = RotateClockWise(lines);
    lines = RotateClockWise(lines);
    return lines;
}

void Print(List<string> lines)
{
    for (int i = 0; i < lines.Count; i++)
    {
        for (int j = 0; j < lines[i].Length; j++)
            Console.Write(""+ lines[i][j] + ' ');
        Console.WriteLine();
    }
    Console.WriteLine();
}

List<string> RotateCounterClockWise(List<string> set) => set.First().Select((_, i) =>
    new string(set.Select(x => x[set.First().Length - 1 - i]).ToArray())).ToList();

List<string> RotateClockWise(List<string> set) => set.First().Select((_, i) =>
    new string(set.Select(x => x[i]).Reverse().ToArray())).ToList();
