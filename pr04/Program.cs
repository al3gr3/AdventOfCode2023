var lines = File.ReadAllLines("TextFile1.txt");
var result = Second(lines);
Console.WriteLine(result);

int Second(string[] lines)
{
    var amounts = Enumerable.Range(0, lines.Length).Select(x => 1).ToArray();

    for (int i = 0; i < lines.Length; i++)
    {
        var win = ParseAmount(lines[i]);
        for (int j = 1; j <= win; j++)
            amounts[i + j] += amounts[i];
    }

    return amounts.Sum();
}

int First(string[] lines) => lines.Select(line => ParseAmount(line))
    .Select(amount =>
    {
        return amount > 0
            ? Enumerable.Range(0, amount - 1).Aggregate(1, (s, n) => s *= 2)
            : 0;
    }
    ).Sum();


int ParseAmount(string line)
{
    var parts = line.Split(new[] { "|" }, StringSplitOptions.RemoveEmptyEntries);
    var winning = parts
        .First()
        .Split(new[] { ":" }, StringSplitOptions.RemoveEmptyEntries)
        .Last()
        .Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries)
        .ToList();

    var ticket = parts
        .Last()
        .Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries)
        .ToList();

    var amount = ticket.Intersect(winning).Count();
    return amount;
}

