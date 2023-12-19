var directions = new Dictionary<string, Point>
{
    { "D", new Point { Y = 1, X = 0 } },
    { "L",  new Point { Y = 0, X = -1 } },
    { "R",  new Point { Y = 0, X = 1 } },
    { "U",  new Point { Y = -1, X = 0 } },
};

Console.WriteLine(SolveWithFill(File.ReadAllLines("TextFile1.txt"), 10, 10, new Point { X = 0, Y = 0 }));
Console.WriteLine(SolveWithFill(File.ReadAllLines("TextFile2.txt"), 500, 600, new Point { X = 250, Y = 250 }));

Console.WriteLine(Smart(File.ReadAllLines("TextFile1.txt"), ParseFirst));

Console.WriteLine(Smart(File.ReadAllLines("TextFile2.txt"), ParseFirst));
Console.WriteLine(Smart(File.ReadAllLines("TextFile2.txt"), ParseSecond));

Point ParseFirst(string line)
{
    var splits = line.Split(' ');
    return directions[splits[0]].Clone().Multiply(int.Parse(splits[1]));
}

Point ParseSecond(string line)
{
    var splits = line.Split(' ');
    var multiplier = Convert.ToInt64(new string(splits[2].Skip(2).Take(5).ToArray()), 16);
    var direction = "" + "RDLU"[int.Parse("" + splits[2][7])];
    return directions[direction].Clone().Multiply(multiplier);
}

long Smart(string[] lines, Func<string, Point> parse)
{
    var area = 0L;
    var dy = 0L;
    var p = 0L;
    foreach (var d in lines.Select(parse))
    {
        area += d.X * dy;
        dy += d.Y;
        p += Math.Max(0, d.X) + Math.Max(0, d.Y); // adding only for D and R
    }
    return Math.Abs(area) + p + 1;
}

long SolveWithFill(string[] lines, int height, int width, Point pos)
{
    var starting = pos.Clone();

    var grid = Enumerable.Range(0, height).Select(x => Enumerable.Range(0, width).Select(x => 0).ToArray()).ToArray();

    grid[pos.Y][pos.X] = 1;
    foreach (var line in lines)
    {
        var splits = line.Split(' ');
        for (var i = 0; i < int.Parse(splits[1]); i++)
        {
            pos.Add(directions[splits[0]]);
            grid[pos.Y][pos.X] = 1;
        }
    }

    Fill(grid, starting.Add(new Point { X = 1, Y = 1 }));

    return grid.Sum(x => x.Sum());
}

void Fill(int[][] grid, Point seed)
{
    var queue = new Queue<Point>();
    queue.Enqueue(seed);
    while (queue.Any())
    {
        var next = queue.Dequeue();
        grid[next.Y][next.X] = 1;
        foreach (var p in directions.Values
            .Select(d => next.Clone().Add(d))
            .Where(p => grid[p.Y][p.X] == 0)
            .Where(p => !queue.Any(x => x.IsEqual(p))))
            queue.Enqueue(p);
    }
}

class Point
{
    internal long X;
    internal long Y;

    internal Point Add(Point point)
    {
        this.X += point.X;
        this.Y += point.Y;
        return this; 
    }

    internal Point Clone() => new Point { X = this.X, Y = this.Y };

    internal bool IsEqual(Point other) => this.X == other.X && this.Y == other.Y;

    internal Point Multiply(long i) => new Point { X = this.X * i, Y = this.Y * i };
}