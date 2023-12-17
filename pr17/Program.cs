var directions = new[]
{
    new Point { Y = 1, X = 0 },
    new Point { Y = 0, X = -1 },
    new Point { Y = 0, X = 1 },
    new Point { Y = -1, X = 0 },
}.ToList();

var lines = File.ReadAllLines("TextFile1.txt");
Console.WriteLine(First(lines));

long First(string[] lines)
{
    var height = lines.Length;
    var width = lines.First().Length;

    var dist = Enumerable.Range(0, height).Select(y => Enumerable.Range(0, width).Select(x => directions.Select(d => 100000000).ToArray()).ToArray()).ToArray();
    var queue = Enumerable.Range(0, height).SelectMany(y => Enumerable.Range(0, width).SelectMany(x => directions.Select(d =>
        new Path 
        { 
            Pos = new Point { X = x, Y = y },
            Dir = d.Clone(),
            Heat = 100000000,
        }))).ToList();

    foreach (var path in queue.Where(p => p.Pos.X == 0 && p.Pos.Y == 0))
        path.Heat = 0;

    dist[0][0][0] = 0;
    dist[0][0][1] = 0;
    dist[0][0][2] = 0;
    dist[0][0][3] = 0;

    while (queue.Any())
    {
        var u = queue.Aggregate(queue.First(), (min, n) => min.Heat > n.Heat ? n : min);
        queue.Remove(u);

        foreach(var step in Enumerable.Range(1, 3))
            foreach(var direction in directions.Where(x => !(x.X == u.Pos.X && x.Y == u.Pos.Y) && !(x.X == -1 * u.Pos.X && x.Y == -1 * u.Pos.Y)))
            {
                var newPos = u.Clone();
                var heat = 0;
                for (var i = 1; i <= step; i++)
                {
                    newPos.Pos.Add(newPos.Dir);
                    if (0 <= newPos.Pos.X && newPos.Pos.X < width &&
                        0 <= newPos.Pos.Y && newPos.Pos.Y < height)
                        heat += int.Parse("" + lines[newPos.Pos.Y][newPos.Pos.X]);
                }

                newPos.Dir = direction.Clone();

                if (0 <= newPos.Pos.X && newPos.Pos.X < width &&
                    0 <= newPos.Pos.Y && newPos.Pos.Y < height)
                {
                    var v = queue.FirstOrDefault(x => x.Pos.IsEqual(newPos.Pos) && x.Dir.Equals(newPos.Dir));
                    if (v == null)
                        break;

                    var alt = dist[u.Pos.Y][u.Pos.X][directions.FindIndex(x => x.IsEqual(u.Dir))] + heat;
                    if (alt < dist[v.Pos.Y][v.Pos.X][directions.FindIndex(x => x.IsEqual(v.Dir))])
                    {
                        dist[v.Pos.Y][v.Pos.X][directions.FindIndex(x => x.IsEqual(v.Dir))] = alt;
                        v.Heat = alt;
                    }
                }
            }
    }

    var result = Enumerable.Range(0, 4).Select(x => dist[height - 1][width - 1][x]).Min();
    return result;
}


class Path
{
    internal Point Pos;
    internal Point Dir;
    internal int Heat;

    internal Path Clone() => new Path
    {
        Pos = Pos.Clone(),
        Dir = Dir.Clone(),
        Heat = Heat,
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

    internal Point Clone()
    {
        return new Point { X = this.X, Y = this.Y };
    }

    internal bool IsEqual(Point other) => this.X == other.X && this.Y == other.Y;
}
