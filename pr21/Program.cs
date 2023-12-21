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
//WalkOnInfinite(lines, 10);

Console.WriteLine(Solve(2));
Console.WriteLine(Solve(4));
Console.WriteLine(Solve(6));
Console.WriteLine(Solve(8)); // this fits with WalkOnInfinite(lines, 8)!!
Console.WriteLine(Solve(10)); // this fits with WalkOnInfinite(lines, 10)!!
Console.WriteLine(Solve(202300)); 


long Solve(long n)
{
    // n * 131 + 65, result from WalkOnInfinite
    // 2 327  93356 
    // 4 589  302126 
    // 6 851  630080
    // 8 1113 1077218
    // 10 1375 1643540
    // solved on https://www.wolframalpha.com/input?i=system+equation+calculator&assumption=%7B%22F%22%2C+%22SolveSystemOf4EquationsCalculator%22%2C+%22equation1%22%7D+-%3E%2293356+%3D+5+f+%2B+a+%2B+t%22&assumption=%7B%22F%22%2C+%22SolveSystemOf4EquationsCalculator%22%2C+%22equation4%22%7D+-%3E%22%22&assumption=%22FSelect%22+-%3E+%7B%7B%22SolveSystemOf3EquationsCalculator%22%7D%7D&assumption=%7B%22F%22%2C+%22SolveSystemOf4EquationsCalculator%22%2C+%22equation2%22%7D+-%3E%22302126+%3D+25+f+%2B+3+a+%2B+t%22&assumption=%7B%22F%22%2C+%22SolveSystemOf4EquationsCalculator%22%2C+%22equation3%22%7D+-%3E%22630080+%3D+61+f+%2B+5+a+%2B+t%22
    // 93356 = 9*7456 + 4*7442 - 3 d + 2 v
    // 302126 = 25*7456 + 16*7442 - 5 d + 4 v

    var even = 7442;
    var odd = 7456;
    var d = 3686;
    var v = 3771;
    // the formula is from https://github.com/villuna/aoc23/blob/main/rust/src/day21.rs
    //return (n + 1) * (n + 1) * odd + n * n * even - (n + 1) * d + n * v;

    var t = 0;
    return n * n * even + (n - 1) * (n - 1) * odd + (n - 1) * blues + tops + (n - 2) * greens;
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

void WalkOnInfinite(string[] lines, int n)
{
    var startY = Array.FindIndex(lines, x => x.Contains('S'));
    var start = new Point
    {
        X = lines[startY].IndexOf('S'),
        Y = startY,
    };

    var wave = new List<Point> { start };
    for (var i = 1; i <= n * 131 + 65; i++)
    {
        if (i == 200 || i == 201)
            Console.WriteLine("" + i + " " + wave.Where(p => 0 <= p.X && p.X < lines.First().Length && 0 <= p.Y && p.Y < lines.Length).Count());

        var nextWave = new HashSet<Point>(new PointComparer());
        foreach (var point in wave)
            foreach (var dir in directions)
            {
                var next = point.Clone().Add(dir);
                var check = new Point
                {
                    X = Mod(next.X, lines.First().Length),
                    Y = Mod(next.Y, lines.Length),
                };

                if (lines[check.Y][check.X] != '#' && !nextWave.Contains(next))
                    nextWave.Add(next);
            }

        wave = nextWave.ToList();

        Console.WriteLine($"{i} {wave.Count}");
    }
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