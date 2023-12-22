var directions = new[]
{
    new Point { Y = 1, X = 0 },
    new Point { Y = 0, X = -1 },
    new Point { Y = 0, X = 1 },
    new Point { Y = -1, X = 0 },
}.ToList();

var lines = File.ReadAllLines("TextFile2.txt");

// 65 + 1 + 65
// 26501365 = 202300 * 131 + 65
var oranges = 0; // even
var reds = 0; // odd
var blues = 0;
WalkOnInfinite(lines, 2);

Console.WriteLine(Solve(2));
Console.WriteLine(Solve(4));
Console.WriteLine(Solve(6));
Console.WriteLine(Solve(8));
Console.WriteLine(Solve(10));
Console.WriteLine(Solve(202300));

long Solve(long n)
{
    // n        n * 131 + 65        result from WalkOnInfinite
    // 2        327                 93356 
    // 4        589                 302126 
    // 6        851                 630080
    // 8        1113                1077218
    // 10       1375                1643540
    // 202300   609708004316870

    var tops = 93356 - 4 * oranges - 1 * reds - 1 * blues; // from the n = 2 
    var greens = (302126 - 16 * oranges - 9 * reds - 3 * blues - tops) / 2; // from the n = 4
    return n * n * oranges + (n - 1) * (n - 1) * reds + (n - 1) * blues + tops + (n - 2) * greens;
}

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
            foreach (var dir in directions)
            {
                var next = point.Clone().Add(dir);
                if (lines[next.Y][next.X] != '#' && !nextWave.Any(x => x.IsEqual(next)))
                    nextWave.Add(next);
            }

        wave = nextWave;
    }
    return wave.Count;
}

void WalkOnInfinite(string[] lines, int n)
{
    var w = lines.First().Length;
    var h = lines.Length;

    var startY = Array.FindIndex(lines, x => x.Contains('S'));
    var start = new Point
    {
        X = lines[startY].IndexOf('S'),
        Y = startY,
    };

    var wave = new List<Point> { start };
    var steps = n * 131 + 65;
    for (var i = 1; i <= steps; i++)
    {
        var nextWave = new HashSet<Point>(new PointComparer());
        foreach (var point in wave)
            foreach (var dir in directions)
            {
                var next = point.Clone().Add(dir);
                var check = new Point
                {
                    X = Mod(next.X, w),
                    Y = Mod(next.Y, h),
                };

                if (lines[check.Y][check.X] != '#' && !nextWave.Contains(next))
                    nextWave.Add(next);
            }
        wave = nextWave.ToList();
        //Console.WriteLine($"{i} {wave.Count}");
    }

    reds = wave.Count(p =>
        0 <= p.X && p.X < w &&
        0 <= p.Y && p.Y < h);

    var blueSquares = new[]
    {
        new Square
        {
            LeftUpper = new Point { X = w, Y = h },
            RightDown = new Point { X = 2 * w, Y = 2 * h },
        },
        new Square
        {
            LeftUpper = new Point { X = -w, Y = -h },
            RightDown = new Point { X = 0, Y = 0 },
        },
        new Square
        {
            LeftUpper = new Point { X = -w, Y = h },
            RightDown = new Point { X = 0, Y = 2 * h },
        },
        new Square
        {
            LeftUpper = new Point { X = w, Y = -h },
            RightDown = new Point { X = 2 * w , Y = 0 },
        },
    };
    blues = wave.Count(p => blueSquares.Any(x => x.Contains(p)));
    
    oranges = wave.Count(p =>
        0 <= p.X && p.X < w &&
        h <= p.Y && p.Y < 2 * h);

    Console.WriteLine("reds " + reds);
    Console.WriteLine("oranges " + oranges);
    Console.WriteLine("blues " + blues);
}

int Mod(int x, int m)
{
    var res = x % m;
    if (res < 0)
        res += m;
    return res;
}

public class PointComparer : IEqualityComparer<Point>
{
    public bool Equals(Point one, Point two) => one.IsEqual(two);

    public int GetHashCode(Point item) => item.X * item.Y;
}

public class Square
{
    public Point LeftUpper;
    public Point RightDown;
    public bool Contains(Point p) =>
        LeftUpper.X <= p.X && p.X < RightDown.X &&
        LeftUpper.Y <= p.Y && p.Y < RightDown.Y;
}

public class Point
{
    public int X;
    public int Y;

    internal Point Add(Point point)
    {
        this.X += point.X;
        this.Y += point.Y;
        return this;
    }

    internal Point Clone() => new Point { X = this.X, Y = this.Y };

    public bool IsEqual(Point other) => this.X == other.X && this.Y == other.Y;
}