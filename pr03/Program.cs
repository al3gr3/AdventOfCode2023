using System.Drawing;

var directions = new[]
{
    new Point { Y = 0, X = -1 },
    new Point { Y = 0, X = 1 },

    new Point { Y = 1, X = 0 },
    new Point { Y = -1, X = 0 },

    new Point { Y = 1, X = 1 },
    new Point { Y = -1, X = 1 },

    new Point { Y = 1, X = -1 },
    new Point { Y = -1, X = -1 },
};

var lines = File.ReadAllLines("TextFile1.txt").ToList();

for (int i = 0; i < lines.Count; i++)
{
    lines[i] = '.' + lines[i] + '.';
}
var dummy = new string(Enumerable.Range(0, lines.First().Length).Select(x => '.').ToArray());
lines.Insert(0, dummy);
lines.Add(dummy);

var result = Second(lines);
Console.WriteLine(result);

int Second(List<string> lines)
{
    var result = 0;
    for (var lineNr = 0; lineNr < lines.Count; lineNr++)
    {
        for (int i = 0; i < lines[lineNr].Length; i++)
        {
            if (lines[lineNr][i] == '*')
            {
                var correctDirections = directions
                    .Select(direction => Find(lineNr, i, direction, lines))
                    .Where(x => x != null)
                    .GroupBy(x => x.Value + x.Left.X + x.Left.Y + x.Right.X + x.Right.Y)
                    .Select(grp => grp.First())
                    .ToList();

                if (correctDirections.Count == 2)
                    result += correctDirections.Aggregate(1, (s, n) => s *= n.Value);
            }
        }
    }
    return result;
}

Number Find(int lineNr, int i, Point point, List<string> lines)
{
    var seed = new Point
    {
        X = lineNr + point.X,
        Y = i + point.Y,
    };

    // find number in string that contains seed
    var number = "" + lines[seed.X][seed.Y];

    if (!"1234567890".Contains(number))
        return null;
        
    var l = seed.Y - 1;
    while ("1234567890".Contains(lines[seed.X][l]))
    {
        number = lines[seed.X][l] + number;
        l--;
    }

    var r = seed.Y + 1;
    while ("1234567890".Contains(lines[seed.X][r]))
    {
        number = number + lines[seed.X][r];
        r++;
    }

    return new Number
    {
        Value = int.Parse(number),
        Left = new Point
        {
            X = seed.X,
            Y = l + 1,
        },
        Right = new Point
        {
            X = seed.X,
            Y = r - 1,
        },
    };
}

int First(List<string> lines)
{
    for (int i = 0; i < lines.Count; i++)
    {
        lines[i] = '.' + lines[i] + '.';
    }
    var dummy = new string(Enumerable.Range(0, lines.First().Length).Select(x => '.').ToArray());
    lines.Insert(0, dummy);
    lines.Add(dummy);

    var result = 0;
    for (var lineNr = 0; lineNr < lines.Count; lineNr++)
    {
        var line = lines[lineNr];
        var number = 0;
        var isAdjacent = false;

        for (int i = 0; i < line.Length; i++)
        {
            if ("0123456789".Contains(line[i]))
            {
                number = 10 * number + int.Parse("" + line[i]);
                isAdjacent |= IsAdjacent(lineNr, i, lines);
            }
            else
            {
                if (isAdjacent)
                    result += number;

                number = 0;
                isAdjacent = false;
            }
        }
    }
    return result;
}

bool IsAdjacent(int lineNr, int i, List<string> lines) => 
    directions.Any(direction => 
        direction.X == 0 && !"0123456789.".Contains(lines[lineNr + direction.X][i + direction.Y]) ||
        direction.X != 0 && !".".Contains(lines[lineNr + direction.X][i + direction.Y]));

internal class Number
{
    internal Point Left;
    internal Point Right;
    internal int Value;
}
