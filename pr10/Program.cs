var lines = File.ReadAllLines("TextFile1.txt");
const int INFINITY = 1000000000;
//Console.WriteLine(First(lines));
Console.WriteLine(Second(lines));

int[][] CalculateDistances(string[] lines)
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
    
    var distances = Enumerable.Range(1, lines.Length)
        .Select(line => Enumerable.Range(1, lines.First().Length).Select(x => INFINITY).ToArray())
        .ToArray();
    distances[s.Y][s.X] = 0;
    var queue = new Queue<Point>();
    queue.Enqueue(s);
    //lines[s.Y] = lines[s.Y].Replace('S', '7');
    lines[s.Y] = lines[s.Y].Replace('S', '|');

    var isFirst = true;
    while (queue.Count > 0)
    {
        var current = queue.Dequeue();
        var c = lines[current.Y][current.X];
        foreach (var move in moves.Where(move => move.Symbol == c))
        {
            var nexts = isFirst
                ? new[] { current.Clone().Add(move.Dir1) }
                : new[] { current.Clone().Add(move.Dir1), current.Clone().Add(move.Dir2) };
            isFirst = false;
            foreach (var next in nexts)
            {
                if (distances[next.Y][next.X] > (distances[current.Y][current.X] + 1))
                {
                    distances[next.Y][next.X] = (distances[current.Y][current.X] + 1);
                    queue.Enqueue(next);
                }
            }
        }
        
    }
    //Print(distances);

    return distances;
}

void Print(int[][] distances)
{
    for (int i = 0; i < distances.Length; i++)
    {
        for (int j = 0; j < distances[i].Length; j++)
            Console.Write(distances[i][j] == INFINITY ? "." : "" + distances[i][j]);
        Console.WriteLine();
    }
    Console.WriteLine();
}

int First(string[] lines)
{
    var distances = CalculateDistances(lines);

    return distances.SelectMany(x => x).Where(x => x != INFINITY).Max();
}

int Second(string[] lines)
{
    var distances = CalculateDistances(lines);

    var max = distances.SelectMany(x => x).Where(x => x != INFINITY).Max();

    var result = 0;
    for (int i = 1; i < distances.Length - 1; i++)
    {
        for (int j = 1; j < distances[i].Length - 1; j++)
        {
            if (distances[i][j] != INFINITY)
                continue;

            // https://en.wikipedia.org/wiki/Nonzero-rule
            var windings = 0; 
            for (int ray = j; ray < distances[i].Length; ray++)
            {
                if (distances[i][ray] != INFINITY)
                {
                    if (distances[i + 1][ray] == (distances[i][ray] + 1) || (distances[i + 1][ray], distances[i][ray]) == (0, max))
                        windings++;
                    if (distances[i + 1][ray] == (distances[i][ray] - 1) || (distances[i + 1][ray], distances[i][ray]) == (max, 0))
                        windings--;
                }
            }

            if (windings != 0)
                result++;
        }
    }
    return result;
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