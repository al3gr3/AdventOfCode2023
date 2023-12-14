var lines = File.ReadAllLines("TextFile1.txt").ToList();
Console.WriteLine(First(lines));
Console.WriteLine(Second(lines));

int First(List<string> lines) => RotateCounterClockWise(lines).Select(Tilt).Sum(CalculateLine);

int CalculateLine(string arg) => arg.Select((c, i) => c == 'O' ? arg.Length - i : 0).Sum();

string Tilt(string arg) => string.Join("#", arg.Split('#')
    .Select(split => new string(split.OrderByDescending(c => c).ToArray())));

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
            break;
        }

        dict[key] = cycle++;
        lines = Cycle(lines);
    }

    var remains = (1000000000 - cycle) % cycleLength;

    for (var i = 0; i < remains; i++)
        lines = Cycle(lines);

    return RotateCounterClockWise(lines).Sum(CalculateLine);
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

List<string> RotateCounterClockWise(List<string> set) => set.First().Select((_, i) =>
    new string(set.Select(x => x[set.First().Length - 1 - i]).ToArray())).ToList();

List<string> RotateClockWise(List<string> set) => set.First().Select((_, i) =>
    new string(set.Select(x => x[i]).Reverse().ToArray())).ToList();