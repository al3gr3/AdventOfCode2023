var lines = File.ReadAllLines("TextFile1.txt");

Console.WriteLine(First(lines));

int First(string[] lines)
{
    var s = FindS(lines);

    var moves = new[]
    {
        new Move
        {
            Dir1 = new Point{ X = 1, Y = 0 },
            Symbol = '-',
            Dir2 = new Point{ X = -1, Y = 0 },
        },
        new Move
        {
            Dir1 = new Point{ X = -1, Y = 0 },
            Symbol = 'J',
            Dir2 = new Point{ X = 0, Y = -1 },
        },
        new Move
        {
            Dir1 = new Point{ X = -1, Y = 0 },
            Symbol = '7',
            Dir2 = new Point{ X = 0, Y = 1 },
        },
        new Move
        {
            Dir1 = new Point{ X = 1, Y = 0 },
            Symbol = 'L',
            Dir2 = new Point{ X = 0, Y = -1 },
        },
        new Move
        {
            Dir1 = new Point{ X = 1, Y = 0 },
            Symbol = 'F',
            Dir2 = new Point{ X = 0, Y = 1 },
        },
        new Move
        {
            Dir1 = new Point{ X = 0, Y = 1 },
            Symbol = '|',
            Dir2 = new Point{ X = 0, Y = -1 },
        },
    };
    const int INFINITY = 1000000000;
    var distances = Enumerable.Range(1, lines.Length)
        .Select(line => Enumerable.Range(1, lines.First().Length).Select(x => INFINITY).ToArray())
        .ToArray();
    distances[s.Y][s.X] = 0;
    var queue = new Queue<Point>();
    queue.Enqueue(s);
    //lines[s.Y] = lines[s.Y].Replace('S', 'F');
    lines[s.Y] = lines[s.Y].Replace('S', '|');

    var directions = new[]
    {
        new Point{ X = 0, Y = 1 },
        new Point{ X = 0, Y = -1 },
        new Point{ X = 1, Y = 0 },
        new Point{ X = -1, Y = 0 },
    }.ToList();

    while (queue.Count > 0)
    {
        var current = queue.Dequeue();
        var c = lines[current.Y][current.X];
        foreach (var direction in directions) 
        {
            foreach (var move in moves.Where(move => move.Symbol == c && move.Dir1.IsEqual(direction)))
            {
                var next = current.Clone().Add(move.Dir1);
                if (distances[next.Y][next.X] > (distances[current.Y][current.X] + 1))
                {
                    distances[next.Y][next.X] = (distances[current.Y][current.X] + 1);
                    queue.Enqueue(next);
                }
            }

            foreach (var move in moves.Where(move => move.Symbol == c && move.Dir2.IsEqual(direction)))
            {
                var next = current.Clone().Add(move.Dir2);
                if (distances[next.Y][next.X] > (distances[current.Y][current.X] + 1))
                {
                    distances[next.Y][next.X] = (distances[current.Y][current.X] + 1);
                    queue.Enqueue(next);
                }
            }
        }
    }

    return distances.SelectMany(x => x).Where(x => x != INFINITY).Max();
}

Point FindS(string[] lines)
{
    for (int i = 0; i < lines.Length; i++)
        for (int j = 0; j < lines[i].Length; j++)
            if (lines[i][j] == 'S')
                return new Point { X = j, Y = i };

    throw new Exception("not found");
}

class Move
{
    internal Point Dir1;
    internal Point Dir2;
    internal char Symbol;
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

    internal Point Clone()
    {
        return new Point { X = this.X, Y = this.Y, };
    }

    internal bool IsEqual(Point p) => (this.X, this.Y) == (p.X, p.Y);
}