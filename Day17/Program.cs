bool useTest = true;
string fileName = useTest ? "sample.txt" : "input.txt";

var input = File.ReadAllLines(fileName);
PartOne(input);

void PartOne(string[] input)
{
    var grid = new Grid(input);

    grid.PrintGrid();

    var visited = new HashSet<GridNode>();
    var start = new PlannerNode(grid.GetNode(0, 0));
    start!.Cost = 0;

    var priorityQueue = new PriorityQueue<PlannerNode, PlannerNode>();
    priorityQueue.Enqueue(start, start);
    int targetRowIndex = grid.GridNodes.GetLength(0) - 1;
    int targetColIndex = grid.GridNodes.GetLength(1) - 1;
    while (priorityQueue.Count > 0) 
    { 
        var current = priorityQueue.Dequeue();
        if (current.GridNode.Row == targetRowIndex && current.GridNode.Col == targetColIndex)
        {
            Console.WriteLine("Cost: " + current.Cost);
            MarkPath(current);
            break;
        }

        if (visited.Contains(current.GridNode))
            continue;

        visited.Add(current.GridNode);

        foreach (var neighbor in grid.GetNeighborNodes(current.GridNode))
        {
            var relativeCost = current.Cost + neighbor.Cost;
            neighbor.Cost = relativeCost;
            Direction neighborDirection = grid.GetNeighborDirection(neighbor, current.GridNode);
            if (!IsOppositeDirection(current.Direction, neighborDirection))
            {
                if (current.Direction == neighborDirection)
                {
                    if (current.NumberInThisDirection < 3)
                    {
                        var plannerNode = new PlannerNode(neighbor, current, neighborDirection, current.NumberInThisDirection + 1);
                        priorityQueue.Enqueue(plannerNode, plannerNode);
                    }
                }
                else
                {
                    var plannerNode = new PlannerNode(neighbor, current, neighborDirection, 1);
                    priorityQueue.Enqueue(plannerNode, plannerNode);
                }
            }
        }
    }
    grid.PrintGrid();

}

bool IsOppositeDirection(Direction? current, Direction newDirection)
{
    if (current == null)
        return false;

    return newDirection == GetOppositeDirection(current.Value);
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

void MarkPath(PlannerNode finalNode)
{
    var currentNode = finalNode;
    while (currentNode != null)
    {
        if (currentNode.Direction.HasValue)
            currentNode.GridNode.Label = (char)currentNode.Direction;
        currentNode = currentNode.Predecessor;
    }
}

class Grid 
{
    public Grid(string[] input)
    {
        var width = input[0].Length;
        var height = input.Length;
        GridNodes = new GridNode[width, height];
        for (int row = 0; row < height; row++)
        {
            var line = input[row];
            for (int col = 0; col < width; col++)
            {
                var c = line[col];
                GridNodes[row, col] = new GridNode(int.Parse(c.ToString()), row, col);
            }
        }
    }

    public int Size => GridNodes.Length;

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

        return nodes;
    }

    public GridNode[,] GridNodes { get; }
    
    public void PrintGrid()
    {
        for (int row = 0; row < GridNodes.GetLength(0); row++)
        {
            for (int col = 0; col < GridNodes.GetLength(1); col++)
            {
                var label = GridNodes[row, col].Label;
                if (!char.IsNumber(label))
                    Console.ForegroundColor = ConsoleColor.Magenta;
                else
                    Console.ForegroundColor = ConsoleColor.White;

                Console.Write(GridNodes[row, col].Label.ToString());
            }
            Console.WriteLine();
        }
        Console.WriteLine();

        Console.ForegroundColor = ConsoleColor.White;
    }
}

class GridNode
{
    public GridNode(int cost, int row, int col)
    {
        Cost = cost;
        Row = row;
        Col = col;
        Label = cost.ToString()[0];
    }

    public int Row { get; }
    public int Col { get; }
    public int Cost { get; set; } = int.MaxValue / 2;
    public char Label { get; set; }
    public GridNode? Predecessor { get; set; } = null;
}

class PlannerNode : IComparable<PlannerNode>
{
    public PlannerNode(GridNode gridNode)
    {
        GridNode = gridNode;
        Cost = gridNode.Cost;
    }

    public PlannerNode(GridNode gridNode, PlannerNode predecessor, Direction direction, int numberInThisDirection)
    {
        GridNode = gridNode;
        Predecessor = predecessor;
        Direction = direction;
        NumberInThisDirection = numberInThisDirection;
        Cost = gridNode.Cost;
    }

    public GridNode GridNode { get; }
    public PlannerNode? Predecessor { get; }
    public Direction? Direction { get; }
    public int NumberInThisDirection { get; };
    public int CompareTo(PlannerNode? other)
    {
        return Cost.CompareTo(other?.Cost);
    }
    public int Cost { get; set; }
}

enum Direction
{
    Up = '^',
    Down = 'v',
    Left = '<',
    Right = '>'
}
