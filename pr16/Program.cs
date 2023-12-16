var lines = File.ReadAllLines("TextFile2.txt");

long Solve(string[] lines, Beam initial)
{
    var beams = new Queue<Beam>();
    beams.Enqueue(initial);

    var light = lines.Select(l => Enumerable.Repeat(' ', l.Length).ToArray()).ToList();

    var nr = 0;
    while (beams.Any())
    {
        if (nr++ > 1_000_000)
            break;

        var beam = beams.Dequeue();

        if (0 <= beam.Pos.X && beam.Pos.X < lines.First().Length &&
            0 <= beam.Pos.Y && beam.Pos.Y < lines.Length)
        {
            light[beam.Pos.Y][beam.Pos.X] = '#';

            var c = lines[beam.Pos.Y][beam.Pos.X];
            if (c == '.')
            {
                beam.Pos.Add(beam.Vector);
                beams.Enqueue(beam);
            }
            else if (c == '\\')
            {
                var newVector = new Point
                {
                    X = beam.Vector.Y,
                    Y = beam.Vector.X
                };
                beams.Enqueue(new Beam
                {
                    Pos = beam.Pos.Add(newVector),
                    Vector = newVector
                });
            }
            else if (c == '/')
            {
                var newVector = new Point
                {
                    X = -1 * beam.Vector.Y,
                    Y = -1 * beam.Vector.X
                };
                beams.Enqueue(new Beam
                {
                    Pos = beam.Pos.Add(newVector),
                    Vector = newVector
                });
            }
            else if (c == '|')
            {
                if (beam.Vector.X == 0)
                {
                    beam.Pos.Add(beam.Vector);
                    beams.Enqueue(beam);
                }
                else
                {
                    beams.Enqueue(new Beam { Pos = new Point { X = beam.Pos.X, Y = beam.Pos.Y - 1 }, Vector = new Point { X = 0, Y = -1 } });
                    beams.Enqueue(new Beam { Pos = new Point { X = beam.Pos.X, Y = beam.Pos.Y + 1 }, Vector = new Point { X = 0, Y = 1 } });
                }
            }
            else if (c == '-')
            {
                if (beam.Vector.Y == 0)
                {
                    beam.Pos.Add(beam.Vector);
                    beams.Enqueue(beam);
                }
                else
                {
                    beams.Enqueue(new Beam { Pos = new Point { X = beam.Pos.X + 1, Y = beam.Pos.Y }, Vector = new Point { X = 1, Y = 0 } });
                    beams.Enqueue(new Beam { Pos = new Point { X = beam.Pos.X - 1, Y = beam.Pos.Y }, Vector = new Point { X = -1, Y = 0 } });
                }
            }
        }
    }
    return light.SelectMany(x => x).Count(x => x == '#');
}

Console.WriteLine(First(lines));
Console.WriteLine(Second(lines));

long First(string[] lines)
{
    var initial = new Beam
    {
        Pos = new Point { X = 0, Y = 0 },
        Vector = new Point { X = 1, Y = 0 },
    };
    return Solve(lines, initial);
}

long Second(string[] lines)
{
    var max = 0L;
    for (var i = 0; i < lines.Length; i++)
    {
        max = Math.Max(max, Solve(lines, new Beam
        {
            Pos = new Point
            { 
                X = 0,
                Y = i,
            },
            Vector = new Point
            {
                X = 1,
                Y = 0
            }
        }));

        max = Math.Max(max, Solve(lines, new Beam
        {
            Pos = new Point
            {
                X = 0,
                Y = lines.Length - 1 - i,
            },
            Vector = new Point
            {
                X = -1,
                Y = 0
            }
        }));
    }

    for (var i = 0; i < lines.First().Length; i++)
    {
        max = Math.Max(max, Solve(lines, new Beam
        {
            Pos = new Point
            {
                X = i,
                Y = 0,
            },
            Vector = new Point
            {
                X = 0,
                Y = 1
            }
        }));

        max = Math.Max(max, Solve(lines, new Beam
        {
            Pos = new Point
            {
                X = lines.First().Length - 1 - i,
                Y = 0,
            },
            Vector = new Point
            {
                X = -1,
                Y = 0
            }
        }));
    }
    return max;
}

class Beam
{
    internal Point Pos;
    internal Point Vector;
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