bool useTest = false;
string fileName = useTest ? "sample.txt" : "input.txt";

var input = File.ReadAllLines(fileName);
PartOne(input);

void PartOne(string[] input)
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

    // it will always be an odd number of unmappable ones
    // because it has to match between lines, not on a line
    // which means the number of total rows must be even


    // its somewhere between 31735 (i think?) and 39319
    // I don't think im accurately finding the line
    int colCount = 0;
    int rowCount = 0;
    foreach (var board in boards)
    {
        Console.WriteLine("Board Rows: " + board.Rows.Count());
        Console.WriteLine("Board Cols: " + board.Columns.Count());
        Console.WriteLine();

        int matchLineHorizontal = FindMatchLine(board.Rows);
        int matchLineVertical = FindMatchLine(board.Columns);

        rowCount += matchLineHorizontal;
        colCount += matchLineVertical;
    }
    int sum = (rowCount * 100) + colCount;
    Console.WriteLine(sum);
}

int FindMatchLine(Dictionary<int, string> lines)
{
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
            if (allLinesInSubsetMatch == true)
            {
                // this is only a true match if it contains an edge
                if (i == 0 || (lastIndex == j))
                {
                    // because we broke from the loop
                    // when rows crossed, we are 1 past the 
                    // index of the line of reflection
                    // but we need to count the lines before
                    // reflection, so we add 1
                    //colCount += startCol;
                    Console.WriteLine("Found a match on line " + (startLineIndex + 1));
                    return startLineIndex;
                }
            }
        }  
    }

    return 0;
}

bool IsSequential (HashSet<int> set)
{
    var start = set.Min();
    var finish = set.Max();

    for (int i = start; i <= finish; i++)
    {
        if (!set.Contains(i))
            return false;
    }
    return true;
}

class Board
{
    public Board(string[] input)
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

        for (int col = 0; col < Grid[0].Length; col++)
        {
            string colValue = string.Empty;
            for (int row = 0; row < Grid.Length; row++)
            {
                colValue += Grid[row][col];
            }
            Columns.Add(col, colValue);
        }
    }

    public Dictionary<int, string> Rows { get; } = new Dictionary<int, string>();
    public Dictionary<int, string> Columns { get; } = new Dictionary<int, string>();

    public char[][] Grid { get; set; } 
}


//// find all horizontal pairs
//var seenRows = new Dictionary<string, int>();
//var rowPairs = new HashSet<int>();
//foreach (var row in board.Rows)
//{
//    if (!seenRows.ContainsKey(row.Value))
//        seenRows.Add(row.Value, row.Key);

//    foreach (var otherRow in board.Rows)
//    {
//        if (row.Key != otherRow.Key)
//        {
//            if (seenRows.ContainsKey(otherRow.Value)
//                && seenRows[otherRow.Value] != otherRow.Key
//                && row.Key != otherRow.Key)
//                rowPairs.Add(otherRow.Key);
//        }
//    }
//}

//var seenCols = new Dictionary<string, int>();
//var colPairs = new HashSet<int>();
//foreach (var col in board.Columns)
//{
//    if (!seenCols.ContainsKey(col.Value))
//        seenCols.Add(col.Value, col.Key);

//    foreach (var otherCol in board.Columns)
//    {
//        if (col.Key != otherCol.Key)
//        {
//            if (seenCols.ContainsKey(otherCol.Value)
//                && seenCols[otherCol.Value] != otherCol.Key
//                && col.Key != otherCol.Key)
//                colPairs.Add(otherCol.Key);
//        }
//    }
//}


//if (rowPairs.Count > colPairs.Count)
//{
//    int start = rowPairs.Min();
//    int finish = rowPairs.Max();
//    int middle = (start + finish) / 2;
//    rowCount += start; // board.Rows.Count - rowPairs.Count;//middle - 1;
//}
//else if (colPairs.Count > rowPairs.Count)
//{
//    int start = colPairs.Min();
//    int finish = colPairs.Max();
//    int middle = (start + finish) / 2;
//    colCount += start;// board.Columns.Count - colPairs.Count;// seenCols.Count; // middle - 1;
//}
//else
//{
//    bool useRow = IsSequential(rowPairs);
//    if (useRow)
//    {
//        int start = rowPairs.Min();
//        int finish = rowPairs.Max();
//        int middle = (start + finish) / 2;
//        rowCount += start; // board.Rows.Count - rowPairs.Count;// middle - 1;
//    }
//    else
//    {
//        int start = colPairs.Min();
//        int finish = colPairs.Max();
//        int middle = (start + finish) / 2;
//        colCount += start;// board.Columns.Count - colPairs.Count;// middle - 1;
//    }
//}