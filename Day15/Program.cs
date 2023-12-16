var fileInput = File.ReadAllText("values.txt");

string[] input = fileInput// "rn=1,cm-,qp=3,cm=2,qp-,pc=4,ot=9,ab=5,pc-,pc=6,ot=7"
    .Trim()
    .Replace("\n", "")
    .Split(",");

//PartOne(input);
PartTwo(input);

void PartOne(string[] inputs)
{
    int sumOfValues = 0;
    foreach (var input in inputs)
        sumOfValues += Hash(input);

    Console.WriteLine("sum of values: " + sumOfValues);
}

void PartTwo(string[] inputs)
{
    var boxes = new Dictionary<int, LinkedList<string>>();
    foreach (var input in inputs)
    {
        char operation = input.Contains('-') ? '-' : '=';
        string[] splits = input.Split(operation);
        string label = splits[0];
        int? focalLength = null;
        if (operation == '=')
            focalLength = int.Parse(splits[1]);

        var value = $"{label} {focalLength}".Trim();

        int boxNumber = Hash(label);
        if (!boxes.ContainsKey(boxNumber))
            boxes.Add(boxNumber, new LinkedList<string>());

        var lenses = boxes[boxNumber];
        if (operation == '-')
        {
            foreach (var lens in lenses)
            {
                if (lens.StartsWith(label))
                {
                    lenses.Remove(lens);
                    break;
                }
            }
        }
        else if (operation == '=')
        {
            bool foundNode = false;
            foreach (var lens in lenses)
            {
                if (lens.StartsWith(label))
                {
                    lenses.AddAfter(lenses.Find(lens), value);
                    lenses.Remove(lens);
                    foundNode = true;
                    break;
                }
            }

            if (!foundNode)
                lenses.AddLast(value);
        }
    }

    foreach (var box in boxes)
    {
        Console.Write("Box: " + box.Key + " ");
        foreach (var lens in box.Value)
            Console.Write(lens + " ");

        Console.WriteLine();
    }

    Console.WriteLine("Focusing Power: " + FocusingPower(boxes));
}

int FocusingPower(Dictionary<int, LinkedList<string>> boxes)
{
    int totalFocusingPower = 0;
    foreach (var box in boxes.Where(x => x.Value.Count > 0))
    {
        for (int i = 0; i < box.Value.Count; i++)
        {
            var lens = box.Value.ElementAt(i);
            int focalLength = int.Parse(lens.Split(" ")[1]);
            int lensPower = (1 + box.Key) * (i + 1) * focalLength;
            totalFocusingPower += lensPower;
        }
    }
    return totalFocusingPower;
}


int Hash(string input)
{
    int currentValue = 0;
    foreach (var c in input.ToCharArray())
    {
        int ascii = (int)c;
        currentValue += ascii;
        currentValue *= 17;
        int remainder = currentValue % 256;
        currentValue = remainder;
    }
    return currentValue;
}
