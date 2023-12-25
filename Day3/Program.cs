bool useTest = false;
string fileName = useTest ? "sample.txt" : "input.txt";

var input = File.ReadAllLines(fileName);
PartOne(input);
//between 535294 and 535353
void PartOne(string[] input)
{
    char[,] grid = new char[input.Length, input[0].Length];
    for (int row = 0; row < input.Length; row++)
    {
        for (int col = 0; col < input[row].Length; col++)
        {
            grid[row, col] = input[row][col];
        }
    }

    Print(grid);
    var seenParts = new List<int>();
    var parts = new List<int>();
    for (int row = 0; row < input.Length; row++)
    {
        for (int col = 0; col < input[row].Length; col++)
        {
            string number = string.Empty;
            if (char.IsNumber(grid[row, col]))
            {
                int startIndex = col;
                int endIndex = col;
                number += grid[row, col];
                while (col < input[row].Length - 1)
                {
                    col++;
                    if (!char.IsNumber(grid[row, col]))
                        break;

                    number += grid[row, col];
                    endIndex = col;
                }

                var partNum = new PartNumber()
                {
                    Row = row,
                    StartColumn = startIndex,
                    EndColumn = endIndex,
                    Value = int.Parse(number)
                };
                if (IsValidPartNumber(partNum, grid))
                    parts.Add(partNum.Value);
            }
        }
    }


    foreach (var part in parts.OrderBy(p => p))
        Console.WriteLine(part);



    Console.WriteLine();
    Console.WriteLine("Sum of parts: " + parts.Sum());// /*parts.Sum(p => p.Value)*/);
}

bool IsValidPartNumber(PartNumber partNumber, char[,] grid)
{
    int leftColIndex = partNumber.StartColumn - 1;
    int rightColIndex = partNumber.EndColumn + 1;
    if (leftColIndex > 0)
        if (grid[partNumber.Row, leftColIndex] != '.' && !char.IsNumber(grid[partNumber.Row, leftColIndex]))
            return true;
    if (rightColIndex < grid.GetLength(1))
        if (grid[partNumber.Row, rightColIndex] != '.' && !char.IsNumber(grid[partNumber.Row, rightColIndex]))
            return true;

    int topRow = partNumber.Row - 1;
    if (topRow >= 0)
    {
        for (int col = leftColIndex; col <= rightColIndex; col++)
        {
            if (col < 0 || col >= grid.GetLength(1))
                continue;
           
            if (grid[topRow, col] != '.' && !char.IsNumber(grid[topRow, col]))
                return true;
        }
    }

    int bottomRow = partNumber.Row + 1;
    if (bottomRow < grid.GetLength(0))
    {
        for (int col = leftColIndex; col <= rightColIndex; col++)
        {
            if (col < 0 || col >= grid.GetLength(1))
                continue;
           
            if (grid[bottomRow, col] != '.' && !char.IsNumber(grid[bottomRow, col ]))
                return true;
        }
    }

    return false;
}

void Print(char[,] grid)
{
    for (int row = 0; row < grid.GetLength(0); row++)
    {
        for (int col = 0; col < grid.GetLength(1); col++)
        {
            Console.Write(grid[row, col]);
        }
        Console.WriteLine();
    }

    Console.WriteLine();
}

class Symbol
{
    public int Row { get; set; }
    public int Column { get; set; }
    public string Value { get; set; }
}

class PartNumber
{
    public int Row { get; set; }
    public int StartColumn { get; set; }
    public int EndColumn { get; set; }
    public int Value { get; set; }
}