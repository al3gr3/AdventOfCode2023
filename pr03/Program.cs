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

var lines = File.ReadAllLines("TextFile1.txt");
var result = First(lines.ToList());
Console.WriteLine(result);

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

