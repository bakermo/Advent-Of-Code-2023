bool useTest = true;
string fileName = useTest ? "sample.txt" : "input.txt";

var input = File.ReadAllLines(fileName);
PartOne(input);

void PartOne(string[] input)
{
    var grid = new Grid(input);
    grid.PrintGrid();

    var foundPath = DFS(grid);
    var visted = new HashSet<GridNode>();
    Console.WriteLine($"Path length: {foundPath.Count}");  
    while (foundPath.Count > 0)
    {
        var node = foundPath.Pop();
        if (visted.Contains(node))
            continue;

        if (node.Value == '.')
            node.Value = '0';

        //Console.Clear();
        //Task.Delay(100);
        //grid.PrintGrid();
    }

    grid.PrintGrid();
}

Stack<GridNode> DFS(Grid grid)
{
    int targetRowIndex = grid.GridNodes.GetLength(0) - 1;
    int targetColIndex = grid.GridNodes.GetLength(1) - 2;
    var start = grid.GetNode(0, 1);
    var goal = grid.GetNode(targetRowIndex, targetColIndex);
    var parentMap = new Dictionary<GridNode, GridNode>();
    var testPath = new Stack<GridNode>();
    var visited = new HashSet<GridNode>();
    testPath.Push(start);
    while (testPath.Count > 0)
    {
        var node = testPath.Pop();
        if (visited.Contains(node))
            continue;

        if (node == goal)
            continue;

        visited.Add(node);
        var validNeighbors = new List<GridNode>();
        if (node.Value != '.')
        {
            var direction = (Direction)node.Value;
            var neighbor = grid.GetNext(node, direction);
            if (neighbor != null)
                validNeighbors.Add(neighbor);
        }
        else
            validNeighbors = grid.GetNeighborNodes(node);

        foreach (var neighbor in validNeighbors)
        {
            testPath.Push(neighbor);
            if (!visited.Contains(neighbor))
                parentMap.Add(neighbor, node);
            //else
            //    parentMap[neighbor] = node;
        }
    }

    var current = goal;
    var foundPath = new Stack<GridNode>();
    foundPath.Push(current);
    while (current != start)
    {
        current = parentMap[current];
        foundPath.Push(current);
    }

    return foundPath;
}

bool IsOppositeDirection(Direction current, Direction newDirection)
{
    if (current == Direction.Nowhere)
        return false;

    return newDirection == GetOppositeDirection(current);
}
Direction GetOppositeDirection(Direction direction)
{
    switch (direction)
    {
        case Direction.Left:
            return Direction.Right;
        case Direction.Right:
            return Direction.Left;
        case Direction.Up:
            return Direction.Down;
        default:
            return Direction.Up;
    }
}

class Grid
{
    public Grid(string[] input)
    {
        var cols = input[0].Length;
        var rows = input.Length;
        GridNodes = new GridNode[rows, cols];
        for (int row = 0; row < rows; row++)
        {
            var line = input[row];
            for (int col = 0; col < cols; col++)
            {
                var c = line[col];
                GridNodes[row, col] = new GridNode(c, row, col);
            }
        }
    }

    public GridNode? GetNode(int row, int col)
    {
        if (row < 0 || row >= GridNodes.GetLength(0))
        {
            return null;
        }
        if (col < 0 || col >= GridNodes.GetLength(1))
        {
            return null;
        }
        return GridNodes[row, col];
    }

    public GridNode? GetNext(GridNode current, Direction direction)
    {
        switch (direction)
        {
            case Direction.Up:
                return GetNode(current.Row - 1, current.Col);
            case Direction.Down:
                return GetNode(current.Row + 1, current.Col);
            case Direction.Left:
                return GetNode(current.Row, current.Col - 1);
            case Direction.Right:
                return GetNode(current.Row, current.Col + 1);
            default:
                return null;
        }
    }

    public Direction GetNeighborDirection(GridNode neighbor, GridNode current)
    {
        if (current.Row > neighbor.Row)
            return Direction.Up;
        else if (current.Row < neighbor.Row)
            return Direction.Down;
        else if (current.Col > neighbor.Col)
            return Direction.Left;
        else
            return Direction.Right;
    }

    public List<GridNode> GetNeighborNodes(GridNode gridNode)
    {
        var nodes = new List<GridNode>();
        var up = GetNode(gridNode.Row - 1, gridNode.Col);
        if (up != null)
            nodes.Add(up);

        var down = GetNode(gridNode.Row + 1, gridNode.Col);
        if (down != null)
            nodes.Add(down);

        var left = GetNode(gridNode.Row, gridNode.Col - 1);
        if (left != null)
            nodes.Add(left);

        var right = GetNode(gridNode.Row, gridNode.Col + 1);
        if (right != null)
            nodes.Add(right);

        return nodes.Where(c => c.Value != '#').ToList();
    }

    public GridNode[,] GridNodes { get; }

    public void PrintGrid()
    {
        for (int row = 0; row < GridNodes.GetLength(0); row++)
        {
            for (int col = 0; col < GridNodes.GetLength(1); col++)
            {
                var label = GridNodes[row, col].Value;
                if (char.IsNumber(label))
                    Console.ForegroundColor = ConsoleColor.Magenta;
                else
                    Console.ForegroundColor = ConsoleColor.White;

                Console.Write(GridNodes[row, col].Value.ToString());
            }
            Console.WriteLine();
        }
        Console.WriteLine();

        Console.ForegroundColor = ConsoleColor.White;
    }
}

class GridNode
{
    public GridNode(char value, int row, int col)
    {
        Row = row;
        Col = col;
        Value = value;
    }

    public int Row { get; }
    public int Col { get; }
    public char Value { get; set; }
}

class PlannerNode
{
    public PlannerNode(GridNode gridNode)
    {
        GridNode = gridNode;
    }

    public PlannerNode(GridNode gridNode, PlannerNode predecessor, Direction direction, int numberInThisDirection)
    {
        GridNode = gridNode;
        Predecessor = predecessor;
        Direction = direction;
        NumberInThisDirection = numberInThisDirection;
    }

    public GridNode GridNode { get; }
    public PlannerNode? Predecessor { get; }
    public Direction Direction { get; } = Direction.Nowhere;
    public int NumberInThisDirection { get; }
}

public enum Direction
{
    Nowhere,
    Up = '^',
    Down = 'v',
    Left = '<',
    Right = '>'
}