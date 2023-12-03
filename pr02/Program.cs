using System.Drawing;

var lines = File.ReadAllLines("TextFile1.txt");
var result = Second(lines);

Console.WriteLine(result);

int Second(string[] lines)
{
    var otvet = 0;
    foreach (var line in lines)
    {
        var popytki = line
            .Split(new[] { ":" }, StringSplitOptions.RemoveEmptyEntries)
            .Last()
            .Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries);

        var reds = 0;
        var greens = 0;
        var blues = 0;
        foreach (var popytka in popytki)
        {
            //10 red, 20 green
            var colors = popytka.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries);

            foreach (var color in colors)
            {
                var splits = color.Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries);
                var amount = int.Parse(splits.First());
                if (color.Contains("green"))
                    greens = Math.Max(greens, amount);
                if (color.Contains("red"))
                    reds = Math.Max(reds, amount);
                if (color.Contains("blue"))
                    blues = Math.Max(blues, amount);
            }
        }
        otvet += reds * greens * blues;
    }
    return otvet;
}
/*

//Game 1: 3 blue, 4 red; 10 red, 20 green, 6 blue; 2 green
int First(string[] lines)
{
    var otvet = 0;
    var nomerStroki = 1;
    foreach (var line in lines)
    {
        var popytki = line.Split(new[] { ":" }, StringSplitOptions.RemoveEmptyEntries)
            .Last()
            .Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries);

        var chestnaja = popytki.All(x => Possible(x));
        if (chestnaja)
            otvet += nomerStroki;

        nomerStroki++;
    }
    return otvet;   
}

//10 red, 20 green
bool Possible(string arg)
{
    var colors = arg.Split(new[] { ","}, StringSplitOptions.RemoveEmptyEntries);
    
    foreach(var color in colors)
    {
        var splits = color.Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries);
        var amount = int.Parse(splits.First());
        if (color.Contains("green") && amount > 13)
            return false;
        if (color.Contains("red") && amount > 12)
            return false;
        if (color.Contains("blue") && amount > 14)
            return false;
    }
    return true;
}
*/
