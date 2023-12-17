var directions = new[]
{
    new Point { Y = 1, X = 0 },
    new Point { Y = 0, X = -1 },
    new Point { Y = 0, X = 1 },
    new Point { Y = -1, X = 0 },
}.ToList();

const int INFINITY = 100000000;

var lines = File.ReadAllLines("TextFile1.txt");
Console.WriteLine(Solve(lines, 1, 3));
Console.WriteLine(Solve(lines, 4, 7));

long Solve(string[] lines, int start, int length)
{
    var height = lines.Length;
    var width = lines.First().Length;

    var queue = Enumerable.Range(0, height).SelectMany(y => Enumerable.Range(0, width).SelectMany(x => directions.Select(d => new Path 
    { 
        Pos = new Point { X = x, Y = y },
        Dir = d.Clone(),
        Dist = INFINITY,
    }))).ToList();

    foreach (var path in queue.Where(p => p.Pos.IsEqual(new Point())))
        path.Dist = 0;

    var result = INFINITY;
    while (queue.Any())
    {
        var u = queue.Aggregate(queue.First(), (min, n) => min.Dist > n.Dist ? n : min);
        if (u.Dist == INFINITY)
            break;
        queue.Remove(u);

        foreach (var step in Enumerable.Range(start, length))
            foreach (var direction in directions.Where(x => !x.IsEqual(u.Dir) && !x.IsEqual(u.Dir.Multiply(-1))))
            {
                var newPos = u.Clone();
                var edge = 0;
                for (var i = 1; i <= step; i++)
                {
                    newPos.Pos.Add(newPos.Dir);
                    if (0 <= newPos.Pos.X && newPos.Pos.X < width &&
                        0 <= newPos.Pos.Y && newPos.Pos.Y < height)
                        edge += int.Parse("" + lines[newPos.Pos.Y][newPos.Pos.X]);
                    else
                        goto stop;
                }

                newPos.Dir = direction.Clone();

                var v = queue.FirstOrDefault(x => x.Pos.IsEqual(newPos.Pos) && x.Dir.IsEqual(newPos.Dir));
                if (v == null)
                    break;

                var alt = u.Dist + edge;
                if (alt < v.Dist)
                    v.Dist = alt;

                if (v.Pos.IsEqual(new Point { X = width - 1, Y = height - 1 }))
                    result = Math.Min(result, v.Dist);
            }
        stop:;
        //Console.WriteLine(queue.Count);
    }

    return result;
}

class Path
{
    internal Point Pos;
    internal Point Dir;
    internal int Dist;

    internal Path Clone() => new Path
    {
        Pos = Pos.Clone(),
        Dir = Dir.Clone(),
        Dist = Dist,
    };
}

class Point
{
    internal int X;
    internal int Y;

    internal void Add(Point point)
    {
        this.X += point.X;
        this.Y += point.Y;
    }

    internal Point Clone() => new Point { X = this.X, Y = this.Y };

    internal bool IsEqual(Point other) => this.X == other.X && this.Y == other.Y;

    internal Point Multiply(int i) => new Point { X = this.X * i, Y = this.Y * i };
}
