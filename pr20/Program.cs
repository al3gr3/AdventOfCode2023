var lines = File.ReadAllLines("TextFile3.txt");
var modules = Parse(lines);

var broadcaster = modules.First(x => x.Name == "roadcaster");

var countHigh = 0;
var countLow = 0;
for (var i = 0; i < 1000; i++)
    Low(broadcaster);

Console.WriteLine(countHigh);
Console.WriteLine(countLow);

Console.WriteLine(countLow * countHigh);

void Low(Module seed)
{
    countLow++;
    var wave = new List<Module>();
    wave.Add(seed);
    var nextWave = new List<Module>();

    while (wave.Any())
    {
        foreach (var module in wave)
        {
            var isSendingHigh = false;
            if (module.Type == '%')
                isSendingHigh = module.IsOn;
            if (module.Type == '&')
                isSendingHigh = !module.ReceivesFrom.Values.All(x => x);

            foreach (var send in module.SendsTo)
            {
                if (isSendingHigh)
                    countHigh++;
                else
                    countLow++;
                if (isSendingHigh)
                {
                    if (send.Type == '%')
                        continue;
                }
                else
                {
                    if (send.Type == '%')
                    {
                        send.IsOn = !send.IsOn;
                        nextWave.Add(send);
                    }
                }

                if (send.Type == '&')
                {
                    send.ReceivesFrom[module] = isSendingHigh;
                    nextWave.Add(send);
                }
            }
        }
        (wave, nextWave) = (nextWave, wave);
        nextWave.Clear();
    }
    
}

List<Module> Parse(string[] lines)
{
    var result = new List<Module>();
    foreach (var line in lines)
    {
        // &inv -> b
        var splits = line.Split(new[] { '-', '>', ' ', ',' }, StringSplitOptions.RemoveEmptyEntries);

        var name = new string(splits[0].Skip(1).ToArray());

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

Console.WriteLine(First(lines));
Console.WriteLine(Second(lines));

long First(string[] lines)
{
return 0;
}

long Second(string[] lines)
{
return 0;
}

class Module
{
    internal char Type;

    internal string Name;

    internal List<Module> SendsTo = new List<Module>();
    internal Dictionary<Module, bool> ReceivesFrom = new Dictionary<Module, bool>();

    internal bool IsOn;
}