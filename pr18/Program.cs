
var directions = new Dictionary<string, Point>
{
    { "D", new Point { Y = 1, X = 0 } },
    { "L",  new Point { Y = 0, X = -1 } },
    { "R",  new Point { Y = 0, X = 1 } },
    { "U",  new Point { Y = -1, X = 0 } },
};

Console.WriteLine(Solve(File.ReadAllLines("TextFile1.txt"), 10, 10, new Point { X = 0, Y = 0 }));
Console.WriteLine(Solve(File.ReadAllLines("TextFile2.txt"), 500, 600, new Point { X = 250, Y = 250 }));
//Console.WriteLine(Second(lines));


long Solve(string[] lines, int height, int width, Point pos)
{
    var starting = pos.Clone();

    var grid = Enumerable.Range(0, height).Select(x => Enumerable.Range(0, width).Select(x => 0).ToArray()).ToArray();

    grid[pos.Y][pos.X] = 1;
    var result = 1;
    foreach (var line in lines)
    {
        var splits = line.Split(' ');
        for (var i = 0; i < int.Parse(splits[1]); i++)
        {
            pos.Add(directions[splits[0]]);
            grid[pos.Y][pos.X] = 1;
            result++;
        }
    }

    Console.WriteLine(grid.Sum(x => x.Sum()));
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
        foreach (var p in directions.Values.Select(d => next.Clone().Add(d)).Where(p => grid[p.Y][p.X] == 0))
            if (!queue.Any(x => x.IsEqual(p)))
                queue.Enqueue(p);
    }
}

void Print(int[][] distances)
{
    for (int i = 0; i < distances.Length; i++)
    {
        for (int j = 0; j < distances[i].Length; j++)
            Console.Write(distances[i][j]);
        Console.WriteLine();
    }
    Console.WriteLine();
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

    internal Point Multiply(int i) => new Point { X = this.X * i, Y = this.Y * i };
}
