bool useTest = false;
string fileName = useTest ? "sample.txt" : "input.txt";

var input = File.ReadAllLines(fileName);
//PartOne(input);
//Test(input)
PartTwo(input);

void PartOne(string[] input)
{
    var boards = GetBoards(input);
    foreach (var board in boards)
    {
        board.Print();
    }
    int total = CalculateBoards(boards);
    Console.WriteLine("Total: " + total);
}

int CalculateBoards(List<Board> boards)
{
    // it will always be an odd number of unmappable ones
    // because it has to match between lines, not on a line
    // which means the number of total rows must be even

    int colCount = 0;
    int rowCount = 0;
    foreach (var board in boards)
    {
        rowCount += board.ReflectionLineHorizontal.GetValueOrDefault();
        colCount += board.ReflectionLineVertical.GetValueOrDefault();
    }
    return (rowCount * 100) + colCount;
}

void PartTwo(string[] input)
{
    int rowCount = 0;
    int colCount = 0;
    var boards = GetBoards(input);
    int boardCount = boards.Count;
    int boardsSmudged = 0;
    int runningTotal = 0;

    int iteration = 1;

    foreach (var board in boards)
    {
        Console.WriteLine("Iteration: " + iteration++);
        // this is disgusting. I am truly ashamed of myself
        bool foundNewLine = false;
        int smudgedBoardsFound = 0;
        HashSet<int> foundHorizontalSmudge = new HashSet<int>();
        HashSet<int> foundVerticalSmudge = new HashSet<int>();
        for (int row = 0; row < board.Rows.Count; row++)
        {
            for (int col = 0; col < board.Columns.Count; col++)
            {
                var smudged = Smudge(board, row, col);
                if (smudged.ReflectionLineHorizontal.HasValue ||
                    smudged.ReflectionLineVertical.HasValue)
                {
                    if (smudged.ReflectionLineHorizontal.HasValue && !smudged.ReflectingRows.Contains(row))
                        continue;

                    if (smudged.ReflectionLineVertical.HasValue && !smudged.ReflectingColumns.Contains(col))
                        continue;

                    if (smudged.ReflectionLineHorizontal.HasValue)
                    {
                        if (foundHorizontalSmudge.Contains(smudged.ReflectionLineHorizontal.Value))
                            continue;
                        else
                            foundHorizontalSmudge.Add(smudged.ReflectionLineHorizontal.Value);
                    }

                    if (smudged.ReflectionLineVertical.HasValue)
                    {
                        if (foundVerticalSmudge.Contains(smudged.ReflectionLineVertical.Value))
                            continue;
                        else
                            foundVerticalSmudge.Add(smudged.ReflectionLineVertical.Value);
                    }

                    if (smudged.ReflectionLineHorizontal.HasValue &&
                        smudged.ReflectionLineVertical.HasValue)
                        Console.WriteLine("Found a match on line " + (row + 1) + " and " + (col + 1));

                    if (smudged.ReflectionLineHorizontal.HasValue &&
                        board.ReflectionLineHorizontal.HasValue
                        && board.ReflectionLineHorizontal == smudged.ReflectionLineHorizontal)
                        Console.WriteLine("Found a match on line " + (row + 1) + " and " + (col + 1));

                    if (smudged.ReflectionLineVertical.HasValue &&
                        board.ReflectionLineVertical.HasValue
                        && board.ReflectionLineVertical == smudged.ReflectionLineVertical)
                        Console.WriteLine("Found a match on line " + (row + 1) + " and " + (col + 1));

                    rowCount += smudged.ReflectionLineHorizontal.GetValueOrDefault();
                    colCount += smudged.ReflectionLineVertical.GetValueOrDefault();
                    smudgedBoardsFound++;
                    boardsSmudged++;
                    foundNewLine = true;

                    Console.WriteLine("Board: ");
                    board.Print(row, col);
                    Console.WriteLine("Board score: " + board.GetScore() +
                        " H: " + board.ReflectionLineHorizontal.GetValueOrDefault() +
                        " V: " + board.ReflectionLineVertical.GetValueOrDefault());

                    Console.WriteLine("After smudge: ");
                    smudged.Print(row, col);
                   
                    runningTotal += smudged.GetScore();
                    if (smudgedBoardsFound > 1)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("FOUND MORE THAN ONE SMUDGED BOARD");
                        Console.ForegroundColor = ConsoleColor.White;
                    }


                    Console.WriteLine($"Smudged score: {smudgedBoardsFound}" + smudged.GetScore() + 
                        " H: " + smudged.ReflectionLineHorizontal.GetValueOrDefault() + 
                        " V: " + smudged.ReflectionLineVertical.GetValueOrDefault());
                    Console.WriteLine("Running total: " + runningTotal);
                    break;
                }

            }
            if (foundNewLine)
                break;
        }
        Console.WriteLine();
    }
    Console.WriteLine("Total: Part 2 " + runningTotal);
    Console.WriteLine("Total Boards: " + boardCount);
    Console.WriteLine("Boards Smudged: " + boardsSmudged);
}

Board Smudge(Board board, int row, int col)
{
    // clone the grid with one of the characters swapped
    var newGrid = new char[board.Grid.Length][];
    for (int i = 0; i < board.Grid.Length; i++)
    {
        newGrid[i] = new char[board.Grid[i].Length];
        for (int j = 0; j < board.Grid[i].Length; j++)
        {
            newGrid[i][j] = board.Grid[i][j];
        }
    }
    if (newGrid[row][col] == '#')
        newGrid[row][col] = '.';
    else
        newGrid[row][col] = '#';


    return new Board(newGrid.Select(x => new string(x)).ToArray(),
        board.ReflectionLineHorizontal,
        board.ReflectionLineVertical);
}

List<Board> GetBoards(string[] input)
{
    Queue<string> queue = new Queue<string>();
    foreach (var line in input)
        queue.Enqueue(line);

    var boards = new List<Board>();
    var boardInput = new List<string>();
    while (queue.Count > 0)
    {
        var line = queue.Dequeue();
        if (string.IsNullOrEmpty(line))
        {
            boards.Add(new Board(boardInput.ToArray()));
            boardInput.Clear();
        }
        else
        {
            if (queue.Count == 0)
            {
                boardInput.Add(line);
                boards.Add(new Board(boardInput.ToArray()));
                boardInput.Clear();
            }
            else
            {
                boardInput.Add(line);
            }
        }
    }

    return boards;
}   

class Board
{
    public Board(string[] input, int? horizontalExclusion = null, int? verticalExclusion = null)
    {
        Grid = new char[input.Length][];
        for (int i = 0; i < input.Length; i++)
        {
            Grid[i] = input[i].ToCharArray();
        }

        for (int row = 0; row < Grid.Length; row++)
        {
            string rowValue = string.Empty;
            for (int col = 0; col < Grid[row].Length; col++)
            {
                rowValue += Grid[row][col];
            }
            Rows.Add(row, rowValue);
        }
        ReflectionLineHorizontal = FindMatchLine(Rows, out var reflectingRows, horizontalExclusion);
        ReflectingRows = reflectingRows;

        for (int col = 0; col < Grid[0].Length; col++)
        {
            string colValue = string.Empty;
            for (int row = 0; row < Grid.Length; row++)
            {
                colValue += Grid[row][col];
            }
            Columns.Add(col, colValue);
        }

        ReflectionLineVertical = FindMatchLine(Columns, out var reflectingColumns, verticalExclusion);
        ReflectingColumns = reflectingColumns;
    }
    public int GetScore()
    {   
        return (ReflectionLineHorizontal.GetValueOrDefault() * 100) +
               ReflectionLineVertical.GetValueOrDefault();
    }
    public Dictionary<int, string> Rows { get; } = new Dictionary<int, string>();
    public Dictionary<int, string> Columns { get; } = new Dictionary<int, string>();
    public HashSet<int> ReflectingRows { get; set; }
    public HashSet<int> ReflectingColumns { get; set; }
    public char[][] Grid { get; } 

    public void Print(int? highlightRow = null, int? highlightCol = null)
    {
        // print with reflection line
        if (ReflectionLineHorizontal.HasValue)
        {
            for (int i = 0; i < Grid.Length; i++)
            {
                Console.ForegroundColor = ConsoleColor.White;
                if (ReflectionLineHorizontal == i)
                    Console.WriteLine("--------------------");

                for (int j = 0; j < Grid[i].Length; j++)
                {
                    Console.ForegroundColor = ConsoleColor.White;
                    if (ReflectingRows.Contains(i))
                        Console.ForegroundColor = ConsoleColor.Green;
                    if (highlightRow.HasValue && highlightRow.Value == i &&
                                               highlightCol.HasValue && highlightCol.Value == j)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write(Grid[i][j]);
                    }

                    else
                        Console.Write(Grid[i][j]);
                }
                Console.WriteLine();
            }
        }
        else
        {
            // do the same if its vertical
            if (ReflectionLineVertical.HasValue)
            {
                for (int i = 0; i < Grid.Length; i++)
                {
                    Console.ForegroundColor = ConsoleColor.White;
                    for (int j = 0; j < Grid[i].Length; j++)
                    {
                        Console.ForegroundColor = ConsoleColor.White;
                        if (ReflectingColumns.Contains(j))
                            Console.ForegroundColor = ConsoleColor.Green;
                        if (highlightRow.HasValue && highlightRow.Value == i &&
                                                   highlightCol.HasValue && highlightCol.Value == j)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                        }

                        if (ReflectionLineVertical == j)
                            Console.Write("|" + Grid[i][j]);
                        else
                            Console.Write(Grid[i][j]);
                    }
                    Console.WriteLine();
                }
            }
        }
        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine();
    }

    public int? ReflectionLineHorizontal { get; }
    public int? ReflectionLineVertical { get; }
    private int? FindMatchLine(Dictionary<int, string> lines, out HashSet<int> reflectingLines,
        int? exclusionLine = null)
    {
        reflectingLines = new HashSet<int>();
        int lastIndex = lines.Count - 1;
        for (int i = 0; i < lines.Count; i++)
        {

            bool allLinesInSubsetMatch = false;
            for (int j = lastIndex; j > i; j--)
            {
                int startLineIndex = i;
                int endLineIndex = j;

                while (startLineIndex < endLineIndex)
                {
                    var start = lines[startLineIndex];
                    var end = lines[endLineIndex];
                    if (start == end)
                    {
                        // potential line
                        startLineIndex++;
                        endLineIndex--;
                        allLinesInSubsetMatch = true;
                    }
                    else
                    {
                        allLinesInSubsetMatch = false;
                        break;
                    }
                }
                if (allLinesInSubsetMatch == true 
                    && startLineIndex != endLineIndex) // line cannot reflect itself
                {
                    // this is only a true match if it contains an edge
                    if (i == 0 || (lastIndex == j))
                    {
                        // because we broke from the loop
                        // when rows crossed, we are 1 past the 
                        // index of the line of reflection
                        // but we need to count the lines before
                        // reflection, so we add 1
                        if (exclusionLine.HasValue && exclusionLine.Value == startLineIndex)
                            continue;

                        for (int k = i; k <= j; k++)
                        {
                            reflectingLines.Add(k);
                        }
                        return startLineIndex;
                    }
                }
            }
        }

        return null;
    }
}