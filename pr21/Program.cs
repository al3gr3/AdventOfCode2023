var directions = new[]
{
    new Point { Y = 1, X = 0 },
    new Point { Y = 0, X = -1 },
    new Point { Y = 0, X = 1 },
    new Point { Y = -1, X = 0 },
}.ToList();

var lines = File.ReadAllLines("TextFile2.txt");

var amounts = File.ReadLines("TextFile3.txt").Select(x => int.Parse(x.Split(' ').Last())).ToArray();
int GetAmount(int step) => step < 129 ? amounts[step] : amounts[129 + (step - 129) % 2];

// 65 + 1 + 65
// 5 x 11 x 481843
var steps = 26501365;
Console.WriteLine(steps / 131); // 202300
Console.WriteLine(steps % 131); // 65

var totalSquares = 1L;
for (var i = 1; i < 202300; i++)
    totalSquares += (4 * i - 4);

var result = totalSquares * (GetAmount(131)) + (4 * 202300 - 4) * GetAmount(65);
Console.WriteLine(result);
// 609121738999234 is too low

long First(string[] lines)
{
    for (var i = 0; i < lines.Length; i++)
        lines[i] = "#" + lines[i] + '#';

    var border = new string(Enumerable.Repeat('#', lines.First().Length).ToArray());

    var temp = lines.ToList();
    temp.Insert(0, border);
    temp.Add(border);
    lines = temp.ToArray();

    var startY = Array.FindIndex(lines, x => x.Contains('S'));
    var start = new Point
    {
        X = lines[startY].IndexOf('S'),
        Y = startY,
    };

    var wave = new List<Point> { start };
    for (var i = 1; i <= 200; i++)
    {
        var nextWave = new List<Point>();
        foreach (var point in wave)
            foreach(var dir in directions)
            {
                var next = point.Clone().Add(dir);
                if (lines[next.Y][next.X] != '#' && !nextWave.Any(x => x.IsEqual(next)))
                    nextWave.Add(next);
            }

        wave = nextWave;
        Console.WriteLine($"{i} {wave.Count}");
    }
    return wave.Count;
}

class Point
{
    internal int X;
    internal int Y;

    internal Point Add(Point point)
    {
        this.X += point.X;
        this.Y += point.Y;
        return this;
    }

    internal Point Clone() => new Point { X = this.X, Y = this.Y };

    internal bool IsEqual(Point other) => this.X == other.X && this.Y == other.Y;
}