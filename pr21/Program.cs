using System.Reflection;

var directions = new[]
{
    new Point { Y = 1, X = 0 },
    new Point { Y = 0, X = -1 },
    new Point { Y = 0, X = 1 },
    new Point { Y = -1, X = 0 },
}.ToList();

var lines = File.ReadAllLines("TextFile2.txt");
for (var i = 0; i < lines.Length; i++)
    lines[i] = "#" + lines[i] + '#';

var border = new string(Enumerable.Repeat('#', lines.First().Length).ToArray());

var temp = lines.ToList();
temp.Insert(0, border);
temp.Add(border);
lines = temp.ToArray();
Print(lines);

var startY = Array.FindIndex(lines, x => x.Contains('S'));
var start = new Point
{
    X = lines[startY].IndexOf('S'),
    Y = startY,
};

Console.WriteLine(First());
Console.WriteLine(Second(lines));

void Print(string[] distances)
{
    for (int i = 0; i < distances.Length; i++)
    {
        for (int j = 0; j < distances[i].Length; j++)
            Console.Write(distances[i][j]);
        Console.WriteLine();
    }
    Console.WriteLine();
}

long First()
{
    var wave = new List<Point> { start };
    for (var i = 1; i <= 64; i++)
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
    }
    return wave.Count;
}

long Second(string[] lines)
{
    return 0;
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