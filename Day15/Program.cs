var fileInput = File.ReadAllText("values.txt");

string[] input = fileInput//"rn=1,cm-,qp=3,cm=2,qp-,pc=4,ot=9,ab=5,pc-,pc=6,ot=7"
    .Trim()
    .Replace("\n", "")
    .Split(",");

PartOne(input);

void PartOne(string[] inputs)
{
    int sumOfValues = 0;
    foreach (var input in inputs)
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
        Console.WriteLine("current value: " + currentValue);
        sumOfValues += currentValue;
    }

    Console.WriteLine("sum of values: " + sumOfValues);
}
