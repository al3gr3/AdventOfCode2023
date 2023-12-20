var lines = File.ReadAllLines("TextFile3.txt");
var modules = Parse(lines);
First();

void First()
{
    var countHigh = 0;
    var countLow = 0;
    var broadcaster = modules.First(x => x.Name == "roadcaster");
    for (var i = 1; i <= 5000; i++)
    {
        countLow++;
        var wave = new List<Module> { broadcaster };

        while (wave.Any())
        {
            var nextWave = new List<Module>();
            foreach (var module in wave)
            {
                var isSendingHigh = false;
                if (module.Type == '%')
                    isSendingHigh = module.IsOn;
                if (module.Type == '&')
                {
                    isSendingHigh = !module.ReceivesFrom.Values.All(x => x);
                    if (!isSendingHigh)
                        module.Cycle = i;
                }

                foreach (var send in module.SendsTo)
                {
                    if (isSendingHigh)
                        countHigh++;
                    else
                        countLow++;

                    if (!isSendingHigh && send.Type == '%')
                    {
                        send.IsOn = !send.IsOn;
                        nextWave.Add(send);
                    }

                    if (send.Type == '&')
                    {
                        send.ReceivesFrom[module] = isSendingHigh;
                        nextWave.Add(send);
                    }
                }
            }
            wave = nextWave;
        }
        if (i == 1000)
            Console.WriteLine(countHigh * countLow);
    }

    foreach (var item in modules.Where(x => x.Type == '&'))
        Console.WriteLine($"{item.Name} {item.Cycle}");

    var result = modules
        .Where(x => x.Type == '&')
        .Select(x => x.Cycle)
        .Where(x => x != 0 && x != 5000)
        .Aggregate((s, n) => s *= n);

    Console.WriteLine(result);
}

List<Module> Parse(string[] lines)
{
    var result = new List<Module>();
    foreach (var line in lines)
    {
        var splits = line.Split(new[] { '-', '>', ' ', ',' }, StringSplitOptions.RemoveEmptyEntries);

        var name = splits[0][1..];

        var module = FindOrAdd(name, result);
        module.Type = splits[0][0];

        module.SendsTo = splits.Skip(1).Select(split => FindOrAdd(split, result)).ToList();
    }
    foreach (var module in result)
        foreach (var send in module.SendsTo)
            send.ReceivesFrom[module] = false;
    return result;
}

Module FindOrAdd(string s, List<Module> l)
{
    var m = l.FirstOrDefault(x => x.Name == s);
    if (m == null)
    {
        m = new Module { Name = s };
        l.Add(m);
    }
    return m;
}

class Module
{
    internal char Type;

    internal string Name;

    internal List<Module> SendsTo = new();
    internal Dictionary<Module, bool> ReceivesFrom = new();

    internal bool IsOn;

    internal long Cycle;
}