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
}).ToList(); // if not for this, MakeStrongestPossible does not alter the hand!!!!!

var ordered = Second(inputBids);
var result = ordered
    .Select((x, i) => x.Amount * (i + 1))
    .Sum();
Console.WriteLine(result);

IList<Bid> First(IEnumerable<Bid> bids) =>
    bids.Order(Comparer<Bid>.Create((a, b) => a.Compare(b))).ToList();

IList<Bid> Second(IEnumerable<Bid> bids)
{
    foreach (var bid in bids)
        bid.MakeStrongestPossible();
    var ordered = bids
        .Order(Comparer<Bid>.Create((a, b) => a.CompareSecond(b))).ToList();
    return ordered;
}

internal class Bid
{
    public string Hand { get; set; }
    public string Original { get; set; }
    public int Amount { get; set; }

    public int Type()
    {
        /*
        7 Five of a kind, where all five cards have the same label: AAAAA
        6 Four of a kind, where four cards have the same label and one card has a different label: AA8AA
        5 Full house, where three cards have the same label, and the remaining two cards share a different label: 23332
        4 Three of a kind, where three cards have the same label, and the remaining two cards are each different from any other card in the hand: TTT98
        3 Two pair, where two cards share one label, two other cards share a second label, and the remaining card has a third label: 23432
        2 One pair, where two cards share one label, and the other three cards have a different label from the pair and each other: A23A4
        1 High card, where all cards' labels are distinct: 23456
        */

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

    internal int Compare(Bid b)
    {
        var result = this.Type() - b.Type();

        if (result != 0)
            return result;

        var cards = "23456789TJQKA";

        foreach(var p in this.Hand.Zip(b.Hand))
        {
            result = cards.IndexOf(p.First) - cards.IndexOf(p.Second);
            if (result != 0)
                return result;
        }

        return 0;
    }

    internal int CompareSecond(Bid b)
    {
        var result = this.Type() - b.Type();

        if (result != 0)
            return result;

        var cards = "J23456789TQKA";

        foreach (var p in this.Original.Zip(b.Original))
        {
            result = cards.IndexOf(p.First) - cards.IndexOf(p.Second);
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
        }).Order(Comparer<Bid>.Create((a, b) => a.CompareSecond(b))).ToList();
        var best = possibilities.Last();

        this.Hand = best.Hand;
    }
}