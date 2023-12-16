//var input = new string[]
//{
//    "O....#....",
//    "O.OO#....#",
//    ".....##...",
//    "OO.#O....O",
//    ".O.....O#.",
//    "O.#..O.#.#",
//    "..O..#O..O",
//    ".......O..",
//    "#....###..",
//    "#OO..#...."
//};

var input = File.ReadAllLines("values.txt");
var uniqueTotals = new Dictionary<int, int>();

//PartOne(input);
PartTwo(input);

void PartOne(string[] input) 
{
    var grid = new char[input.Length, input[0].Length];
    for (var row = 0; row < input.Length; row++)
    {
        for (var col = 0; col < input[row].Length; col++)
        {
            grid[row, col] = input[row][col];
        }
    }

    WriteGrid(grid);    

    Console.WriteLine("\nShifting grid:\n");
    ShiftGridUp(grid);
    WriteGrid(grid);

    // count all of the 0s in each row and mulitply them by the number
    // of rows from the bottom of the array, then sum them all together
    int total = 0;
    for (var row = 0; row < grid.GetLength(0); row++)
    {
        int count = 0;
        for (var col = 0; col < grid.GetLength(1); col++)
        {
            if (grid[row, col] == 'O')
            {
                count++;
            }
        }
        total += count * (grid.GetLength(0) - row);
    }

    Console.WriteLine("total: " + total);
}

void PartTwo(string[] input)
{
    var grid = new char[input.Length, input[0].Length];
    for (var row = 0; row < input.Length; row++)
    {
        for (var col = 0; col < input[row].Length; col++)
        {
            grid[row, col] = input[row][col];
        }
    }

    WriteGrid(grid);

    Console.WriteLine("\nShifting grid:\n");
    Console.WriteLine("\nCycling:\n");
    //1000000000
    for (int i = 0; i < 1000000; i++)
    {
        if (i % 100000 == 0)
        {
            Console.WriteLine("i: " + i);
        }

        Cycle(grid);
        //WriteGrid(grid);
        //CalculateTotal(grid);
        int total = GetTotal(grid);
        if (uniqueTotals.ContainsKey(total))
        {
            uniqueTotals[total]++;
            //Console.WriteLine("Found a match at " + i);
        }
        else
            uniqueTotals.Add(total, 1);

        //if (hash.Contains(grid))
        //{
        //    Console.WriteLine("Found a match at " + i);
        //    break;
        //}
        //else
        //    hash.Add(grid);


        // I just realized theres only about a dozen or so loads that this cycles through
        // I couldnt figure out the pattern but I was able to guess by process of elimination
        // based on those handful of repeating weights lol
    }

    foreach (var uniqueTotal in uniqueTotals)
        Console.WriteLine("Total: " + uniqueTotal.Key + " Times: " + uniqueTotal.Value);

    //Console.WriteLine("uniqueTotals: " + String.Join(",", uniqueTotals));

    WriteGrid(grid);
}

int GetTotal(char[,] grid)
{
    int total = 0;
    for (var row = 0; row < grid.GetLength(0); row++)
    {
        int count = 0;
        for (var col = 0; col < grid.GetLength(1); col++)
        {
            if (grid[row, col] == 'O')
                count++;
        }
        total += count * (grid.GetLength(0) - row);
    }

    return total;
}

void CalculateTotal(char[,] grid)
{
    int total = GetTotal(grid);
    //Console.WriteLine("total: " + total);
    Console.Write(total + ", ");
}

void Cycle(char[,] grid)
{
    ShiftGridUp(grid);
    //WriteGrid(grid);

    ShiftGridLeft(grid);
    //WriteGrid(grid);

    ShiftGridDown(grid);
    //WriteGrid(grid);

    ShiftGridRight(grid);
    //WriteGrid(grid);

    //WriteGrid(grid);
}

void WriteGrid(char[,] grid)
{
    Console.WriteLine();
    for (var row = 0; row < grid.GetLength(0); row++)
    {
        for (var col = 0; col < grid.GetLength(1); col++)
        {
            Console.Write(grid[row, col]);
        }
        Console.WriteLine();
    }
    Console.WriteLine();
}

void ShiftGridUp(char[,] grid)
{
    int rows = grid.GetLength(0);
    int cols = grid.GetLength(1);

    for (int col = 0; col < cols; col++)
    {
        int nextAvailableRow = 0; // Track the next available row for an 'O' in this column

        for (int row = 0; row < rows; row++)
        {
            if (grid[row, col] == 'O')
            {
                // Shift 'O' up to the next available row
                if (row != nextAvailableRow)
                {
                    grid[nextAvailableRow, col] = 'O';
                    grid[row, col] = '.';
                }
                nextAvailableRow++; // Update the next available row
            }
            else if (grid[row, col] == '#')
            {
                // Update the next available row based on the '#' barrier
                nextAvailableRow = row + 1;
            }
        }
    }
}

void ShiftGridDown(char[,] grid)
{
    int rows = grid.GetLength(0);
    int cols = grid.GetLength(1);

    for (int col = 0; col < cols; col++)
    {
        int nextAvailableRow = rows - 1; // Track the next available row for an 'O' in this column

        for (int row = rows - 1; row >= 0; row--)
        {
            if (grid[row, col] == 'O')
            {
                // Shift 'O' down to the next available row
                if (row != nextAvailableRow)
                {
                    grid[nextAvailableRow, col] = 'O';
                    grid[row, col] = '.';
                }
                nextAvailableRow--; // Update the next available row
            }
            else if (grid[row, col] == '#')
            {
                // Update the next available row based on the '#' barrier
                nextAvailableRow = row - 1;
            }
        }
    }
}

void ShiftGridLeft(char[,] grid)
{
    int rows = grid.GetLength(0);
    int cols = grid.GetLength(1);

    for (int row = 0; row < rows; row++)
    {
        int nextAvailableCol = 0; // Track the next available column for an 'O' in this row

        for (int col = 0; col < cols; col++)
        {
            if (grid[row, col] == 'O')
            {
                // Shift 'O' left to the next available column
                if (col != nextAvailableCol)
                {
                    grid[row, nextAvailableCol] = 'O';
                    grid[row, col] = '.';
                }
                nextAvailableCol++; // Update the next available column
            }
            else if (grid[row, col] == '#')
            {
                // Update the next available column based on the '#' barrier
                nextAvailableCol = col + 1;
            }
        }
    }
}

void ShiftGridRight(char[,] grid)
{
    int rows = grid.GetLength(0);
    int cols = grid.GetLength(1);

    for (int row = 0; row < rows; row++)
    {
        int nextAvailableCol = cols - 1; // Track the next available column for an 'O' in this row

        for (int col = cols - 1; col >= 0; col--)
        {
            if (grid[row, col] == 'O')
            {
                // Shift 'O' right to the next available column
                if (col != nextAvailableCol)
                {
                    grid[row, nextAvailableCol] = 'O';
                    grid[row, col] = '.';
                }
                nextAvailableCol--; // Update the next available column
            }
            else if (grid[row, col] == '#')
            {
                // Update the next available column based on the '#' barrier
                nextAvailableCol = col - 1;
            }
        }
    }
}

