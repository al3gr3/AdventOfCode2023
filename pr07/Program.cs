var lines = File.ReadAllLines("TextFile1.txt");

var inputBids = lines.Select(line =>
{
    var splits = line.Split(' ');
    return new Bid
    {
        Hand = splits.First(),
        Original = splits.First(),
        Amount = int.Parse(splits.Last())
    };
}).ToList(); // if not for this, MakeStrongestPossible does not alter Hand!!!!!

var ordered = Second(inputBids);
var result = ordered
    .Select((x, i) => x.Amount * (i + 1))
    .Sum();
Console.WriteLine(result);

IList<Bid> First(IEnumerable<Bid> bids) =>
    bids.Order(Comparer<Bid>.Create((a, b) => a.Compare(b, "23456789TJQKA"))).ToList();

IList<Bid> Second(IEnumerable<Bid> bids)
{
    foreach (var bid in bids)
        bid.MakeStrongestPossible();
    var ordered = bids
        .Order(Comparer<Bid>.Create((a, b) => a.Compare(b, "J23456789TQKA"))).ToList();
    return ordered;
}

internal class Bid
{
    public string Hand { get; set; }
    public string Original { get; set; }
    public int Amount { get; set; }

    public int Type()
    {
        var amounts = Hand.GroupBy(c => c).Select(grp => grp.Count()).ToArray();

        var result = amounts.Max();

        if (result == 2)
            if (amounts.Count(x => x == 2) == 2)
                return 3;

        if (result == 3)
        {
            if (amounts.Any(x => x == 2))
                return 5;
            else
                return 4;
        }

        if (result == 4)
            return 6;

        if (result == 5)
            return 7;

        return result;
    }

    internal int Compare(Bid b, string cardsOrder)
    {
        var result = this.Type() - b.Type();

        if (result != 0)
            return result;

        foreach(var p in this.Original.Zip(b.Original))
        {
            result = cardsOrder.IndexOf(p.First) - cardsOrder.IndexOf(p.Second);
            if (result != 0)
                return result;
        }

        return 0;
    }

    internal void MakeStrongestPossible()
    {
        var possibilities = "23456789TQKA".Select(c => new Bid
        {
            Hand = this.Original.Replace('J', c),
            Original = this.Original,
        }).Order(Comparer<Bid>.Create((a, b) => a.Compare(b, "J23456789TQKA"))).ToList();
        var best = possibilities.Last();

        this.Hand = best.Hand;
    }
}