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

// n * 131 + 65
// 0  65   3770
// 1 196  33665
// 2 327  93356 = 5 full + (a+b+c+d) + tops
// 3 458 182843
// 4 589 302126 = 25 full + 3(a+b+c+d) + tops
// 6 851 630080 = 61 full + 5(a+b+c+d) + tops

// solved on https://www.wolframalpha.com/input?i=system+equation+calculator&assumption=%7B%22F%22%2C+%22SolveSystemOf4EquationsCalculator%22%2C+%22equation1%22%7D+-%3E%2293356+%3D+5+f+%2B+a+%2B+t%22&assumption=%7B%22F%22%2C+%22SolveSystemOf4EquationsCalculator%22%2C+%22equation4%22%7D+-%3E%22%22&assumption=%22FSelect%22+-%3E+%7B%7B%22SolveSystemOf3EquationsCalculator%22%7D%7D&assumption=%7B%22F%22%2C+%22SolveSystemOf4EquationsCalculator%22%2C+%22equation2%22%7D+-%3E%22302126+%3D+25+f+%2B+3+a+%2B+t%22&assumption=%7B%22F%22%2C+%22SolveSystemOf4EquationsCalculator%22%2C+%22equation3%22%7D+-%3E%22630080+%3D+61+f+%2B+5+a+%2B+t%22

/*
     3
    323
   32123
  3210123
   32123
    323
     3 
*/
/*
1 1
2 5
3 13
4 25
5 41
6 61
7 85
8 113
9 145
10 181
11 221
*/

Console.WriteLine(Solve(2));
Console.WriteLine(Solve(4));
Console.WriteLine(Solve(6));
Console.WriteLine(Solve(8)); // this fits with WalkOnInfinite(lines, 8)!!
Console.WriteLine(Solve(10)); // this fits with WalkOnInfinite(lines, 10)!!
Console.WriteLine(Solve(202300)); // 609703709349574 is not the right answer ???

long Solve(int n)
{
    var totalSquares = 1L;
    for (var i = 2; i <= n; i++)
        totalSquares += (4 * i - 4);
    //f = 7449 and t = 26216 and a = 29895
    return totalSquares * 7449 + (n - 1) * 29895 + 26216;
}

// 609121738999234 is too low 
// 609129005334242 is too low 
// 609135027400642 is too low 
// 610280941185056 That's not the right answer 
// 609714032047670 is not the right answer; 81850984601L * 7449 + (202300L - 1) * 29895 + 26216

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