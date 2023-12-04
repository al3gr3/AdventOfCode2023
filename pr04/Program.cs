var lines = File.ReadAllLines("TextFile1.txt");
var result = First(lines);
Console.WriteLine(result);



int First(string[] lines) => lines.Select(line =>
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
    Console.WriteLine(amount);
    return amount > 0
        ? Enumerable.Range(0, amount - 1).Aggregate(1, (s, n) => s *= 2)
        : 0;
}
).Sum();
