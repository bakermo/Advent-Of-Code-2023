//var inputs = new string[]
//{
//    "LLR",
//    "",
//    "AAA = (BBB, BBB)",
//    "BBB = (AAA, ZZZ)",
//    "ZZZ = (ZZZ, ZZZ)"
//};

var inputs = File.ReadAllLines("values.txt");

PartOne(inputs);
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

