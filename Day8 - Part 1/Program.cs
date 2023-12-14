using MathNet.Numerics;

//var inputs = new string[]
//{
//    "LLR",
//    "",
//    "AAA = (BBB, BBB)",
//    "BBB = (AAA, ZZZ)",
//    "ZZZ = (ZZZ, ZZZ)"
//};


//var inputs = new string[]
//{
//    "LR",
//    "",
//    "11A = (11B, XXX)",
//    "11B = (XXX, 11Z)",
//    "11Z = (11B, XXX)",
//    "22A = (22B, XXX)",
//    "22B = (22C, 22C)",
//    "22C = (22Z, 22Z)",
//    "22Z = (22B, 22B)",
//    "XXX = (XXX, XXX)"
//};

var inputs = File.ReadAllLines("values.txt");

//PartOne(inputs);
PartTwo(inputs);
void PartOne(string[] inputs)
{
    var directionLine = inputs[0];
    var directionQueue = new Queue<char>();
    
    var nodeLines = inputs.Skip(2).Take(inputs.Length).ToArray();
    var mapping = new Dictionary<string, Node>();
    for (int i = 0; i < nodeLines.Length; i++)
    {
        //RTF = (TRM, KNP)
        string key = nodeLines[i].Substring(0, 3);
        string left = nodeLines[i].Substring(7, 3);
        string right = nodeLines[i].Substring(12, 3);
        var node = new Node(key, left, right);
        mapping.Add(key, node);
    }

    var currentNode = mapping["AAA"];
    var targetNode = mapping["ZZZ"];
    int stepCount = 0;
    while (currentNode.Key != targetNode.Key)
    {
        if (directionQueue.Count == 0)
            foreach (char d in directionLine)
                directionQueue.Enqueue(d);

        char direction = directionQueue.Dequeue();
        if (direction == 'R')
            currentNode = mapping[currentNode.Right];
        else
            currentNode = mapping[currentNode.Left];

        stepCount++;
    }

    Console.WriteLine("Step Count: " + stepCount);
}

void PartTwo(string[] inputs)
{
    var directionLine = inputs[0];
    var directionQueue = new Queue<char>();

    var nodeLines = inputs.Skip(2).Take(inputs.Length).ToArray();
    var mapping = new Dictionary<string, Node>();
    for (int i = 0; i < nodeLines.Length; i++)
    {
        string key = nodeLines[i].Substring(0, 3);
        string left = nodeLines[i].Substring(7, 3);
        string right = nodeLines[i].Substring(12, 3);
        var node = new Node(key, left, right);
        mapping.Add(key, node);
    }

    var currentNodes = mapping.Values.Where(x => x.Key.EndsWith('A'));
    var targetNodes = mapping.Values.Where(x => x.Key.EndsWith('Z'));

    var cycleCounts = new Dictionary<string, long>();
    foreach (var node in currentNodes)
    {
        cycleCounts.Add(node.Key, 0);
        long stepCount = 0;
        var currentNode = node;
        // so we basically want to count how long it takes for 
        // each individual node to get to its target node.
        // we can then use the LCM of all of those values to
        // determine how long it will take for all of them to   
        // reach their target nodes at the same time
        while (!currentNode.Key.EndsWith('Z'))
        {
            if (directionQueue.Count == 0)
                foreach (char d in directionLine)
                    directionQueue.Enqueue(d);

            char direction = directionQueue.Dequeue();
            if (direction == 'R')
                currentNode = mapping[currentNode.Right];
            else
                currentNode = mapping[currentNode.Left];

            stepCount++;
        }
        cycleCounts[node.Key] = stepCount;
    }

    // ok ok, so I know I just searched for a nuget package to do the
    // LCM for me...sorry...I'm too buzzed to think that out at the moment
    Console.WriteLine(Euclid.LeastCommonMultiple(cycleCounts.Values.ToArray()));
}

class Node
{
    public Node(string key, string left, string right)
    {
        Key = key;
        Left = left;
        Right = right;
    }

    public string Key { get; }
    public string Left { get; }
    public string Right { get; }
}

