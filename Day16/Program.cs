bool useTest = false;
string fileName = useTest ? "sample.txt" : "input.txt";

var input = File.ReadAllLines(fileName);
//PartOne(input);

PartTwo(input);
void PartTwo(string[] input)
{

    // convert the input lines to a 2d array of Node objects
    var grid = new Node[input.Length][];
    for (int i = 0; i < input.Length; i++)
    {
        grid[i] = new Node[input[i].Length];
        for (int j = 0; j < input[i].Length; j++)
        {
            grid[i][j] = new Node(input[i][j], i, j);
        }
    }

    var board = new Board(grid);
    var startingMoves = new List<Move>();

    // get all nodes in the first row
    for (int i = 0; i < board.Nodes[0].Length; i++)
    {
        startingMoves.Add(new Move(board.Nodes[0][i], Direction.Down));
    }
    // get all nodes in the last row
    for (int i = 0; i < board.Nodes[board.Nodes.Length - 1].Length; i++)
    {
        startingMoves.Add(new Move(board.Nodes[board.Nodes.Length - 1][i], Direction.Up));
    }
    // get all nodes in the first column
    for (int i = 0; i < board.Nodes.Length; i++)
    {
        startingMoves.Add(new Move(board.Nodes[i][0], Direction.Right));
    }
    // get all nodes in the last column
    for (int i = 0; i < board.Nodes.Length; i++)
    {
        startingMoves.Add(new Move(board.Nodes[i][board.Nodes[i].Length - 1], Direction.Left));
    }
    var runs = new List<int>();
    foreach (var startingMove in startingMoves)
    {

        // reset the values on the board
        for (int i = 0; i < input.Length; i++)
        {
            for (int j = 0; j < input[i].Length; j++)
            {
                board.Nodes[i][j].Value = input[i][j];
                board.Nodes[i][j].ClearVisits();
            }
        }

        var threads = new Queue<Move>();
        threads.Enqueue(new Move(startingMove.Node, startingMove.Direction));

       // PrintBoard(board);

        while (threads.Count > 0)
        {
            var currentMove = threads.Dequeue();
            var currentNode = currentMove.Node;
            if (!currentNode.HasBeenVisitedFrom(currentMove.Direction))
            {
                currentNode.Visit(currentMove.Direction);
                Direction nextDirection = currentMove.Direction;

                if (currentNode.Value == '/')
                {
                    if (IsVertical(nextDirection))
                        nextDirection = RotateClockwise(nextDirection);
                    else
                        nextDirection = RotateCounterClockwise(nextDirection);

                    board.QueueNextNode(currentNode, threads, nextDirection);
                }
                else if (currentNode.Value == '\\')
                {
                    if (IsVertical(nextDirection))
                        nextDirection = RotateCounterClockwise(nextDirection);
                    else
                        nextDirection = RotateClockwise(nextDirection);

                    board.QueueNextNode(currentNode, threads, nextDirection);
                }
                else if (currentNode.Value == '-')
                {
                    if (IsVertical(nextDirection))
                    {
                        board.QueueLeft(currentNode, threads);
                        board.QueueRight(currentNode, threads);
                    }
                    else
                        board.QueueNextNode(currentNode, threads, nextDirection);
                }
                else if (currentNode.Value == '|')
                {
                    if (IsHorizontal(nextDirection))
                    {
                        board.QueueUp(currentNode, threads);
                        board.QueueDown(currentNode, threads);
                    }
                    else
                        board.QueueNextNode(currentNode, threads, nextDirection);

                }
                else
                {
                    board.QueueNextNode(currentNode, threads, nextDirection);
                }
            }
        }

        //PrintBoard(board);


        int energizedNodes = CalculateEnergizedNodes(board);
        runs.Add(energizedNodes);
        //PrintBoard(board);
        Console.WriteLine("energizedNodes: " + energizedNodes);
    }
    
    Console.WriteLine("max: " + runs.Max());
}

void PartOne(string[] input)
{

    // convert the input lines to a 2d array of Node objects
    var grid = new Node[input.Length][];
    for (int i = 0; i < input.Length; i++)
    {
        grid[i] = new Node[input[i].Length];
        for (int j = 0; j < input[i].Length; j++)
        {
            grid[i][j] = new Node(input[i][j], i, j);
        }
    }

    var board = new Board(grid);

    var startingNode = grid[0][0];

    var threads = new Queue<Move>();
    threads.Enqueue(new Move(startingNode, Direction.Right));

    PrintBoard(board);

    while (threads.Count > 0)
    {
        var currentMove = threads.Dequeue();
        var currentNode = currentMove.Node;
        if (!currentNode.HasBeenVisitedFrom(currentMove.Direction))
        {
            currentNode.Visit(currentMove.Direction);
            Direction nextDirection = currentMove.Direction;

            if (currentNode.Value == '/')
            {
                if (IsVertical(nextDirection))
                    nextDirection = RotateClockwise(nextDirection);
                else
                    nextDirection = RotateCounterClockwise(nextDirection);

                board.QueueNextNode(currentNode, threads, nextDirection);
            }
            else if (currentNode.Value == '\\')
            {
                if (IsVertical(nextDirection))
                    nextDirection = RotateCounterClockwise(nextDirection);
                else
                    nextDirection = RotateClockwise(nextDirection);

                board.QueueNextNode(currentNode, threads, nextDirection);
            }
            else if (currentNode.Value == '-')
            {
                if (IsVertical(nextDirection))
                {
                    board.QueueLeft(currentNode, threads);
                    board.QueueRight(currentNode, threads);
                }
                else
                    board.QueueNextNode(currentNode, threads, nextDirection);
            }
            else if (currentNode.Value == '|')
            {
                if (IsHorizontal(nextDirection))
                {
                    board.QueueUp(currentNode, threads);
                    board.QueueDown(currentNode, threads);
                }
                else
                    board.QueueNextNode(currentNode, threads, nextDirection);

            }
            else
            {
                board.QueueNextNode(currentNode, threads, nextDirection);
            }
        }
    }

    PrintBoard(board);

   
    int energizedNodes = CalculateEnergizedNodes(board);

    PrintBoard(board);
    Console.WriteLine("energizedNodes: " + energizedNodes);
}

int CalculateEnergizedNodes(Board board)
{
    var energizedNodes = 0;
    for (int i = 0; i < board.Nodes.Length; i++)
    {
        for (int j = 0; j < board.Nodes[i].Length; j++)
        {
            var node = board.Nodes[i][j];
            node.Value = node.HasBeenVisited() ? '#' : '.';
            energizedNodes += node.HasBeenVisited() ? 1 : 0;
        }
    }
    return energizedNodes;
}

void PrintBoard(Board board)
{
    for (int i = 0; i < board.Nodes.Length; i++)
    {
        Console.Write($"{i + 1}:\t");
        for (int j = 0; j < board.Nodes[i].Length; j++)
        {
            Console.Write(board.Nodes[i][j].Value);
        }
        Console.WriteLine();

    }
    Console.WriteLine();
}

bool IsVertical(Direction direction) => direction == Direction.Up || direction == Direction.Down;
bool IsHorizontal(Direction direction) => direction == Direction.Left || direction == Direction.Right;

Direction RotateClockwise(Direction direction)
{
    switch (direction)
    {
        case Direction.Up:
            return Direction.Right;
        case Direction.Down:
            return Direction.Left;
        case Direction.Left:
            return Direction.Up;
        default:
            return Direction.Down;
    }
}

Direction RotateCounterClockwise(Direction direction)
{
    switch (direction)
    {
        case Direction.Up:
            return Direction.Left;
        case Direction.Down:
            return Direction.Right;
        case Direction.Left:
            return Direction.Down;
        default:
            return Direction.Up;
    }
}

class Board
{
    public Board(Node[][] nodes)
    {
        Nodes = nodes;
    }
    public Node[][] Nodes { get; }

    private Node? GetLeft(Node currentNode)
    {
        if (currentNode.Col == 0)
            return null;

        return Nodes[currentNode.Row][currentNode.Col - 1];
    }

    private Node? GetRight(Node currentNode)
    {
        if (currentNode.Col == Nodes[0].Length - 1)
            return null;

        return Nodes[currentNode.Row][currentNode.Col + 1];
    }

    private Node? GetUp(Node currentNode)
    {
        if (currentNode.Row == 0)
            return null;

        return Nodes[currentNode.Row - 1][currentNode.Col];
    }

    private Node? GetDown(Node currentNode)
    {
        if (currentNode.Row == Nodes.Length - 1)
            return null;

        return Nodes[currentNode.Row + 1][currentNode.Col];
    }

    public Node? GetNextNode(Node currentNode, Direction direction)
    {
        if (direction == Direction.Up)
            return GetUp(currentNode);
        else if (direction == Direction.Down)
            return GetDown(currentNode);
        else if (direction == Direction.Left)
            return GetLeft(currentNode);
        else
            return GetRight(currentNode);   
    }

    public void QueueNextNode(Node currentNode, Queue<Move> threads, Direction direction)
    {
        var nextNode = GetNextNode(currentNode, direction);
        QueueNode(currentNode, threads, nextNode, direction);
    }

    private void QueueNode(Node currentNode, Queue<Move> threads, Node? nextNode, Direction direction)
    {
        if (nextNode != null)
        {
            if (!nextNode.HasBeenVisitedFrom(direction))
                threads.Enqueue(new Move(nextNode, direction));
        }
    }

    public void QueueUp(Node currentNode, Queue<Move> threads)
    {
        var nextNode = GetUp(currentNode);
        QueueNode(currentNode, threads, nextNode, Direction.Up);
    }

    public void QueueDown(Node currentNode, Queue<Move> threads)
    {
        var nextNode = GetDown(currentNode);
        QueueNode(currentNode, threads, nextNode, Direction.Down);
    }

    public void QueueLeft(Node currentNode, Queue<Move> threads)
    {
        var nextNode = GetLeft(currentNode);
        QueueNode(currentNode, threads, nextNode, Direction.Left);
    }

    public void QueueRight(Node currentNode, Queue<Move> threads)
    {
        var nextNode = GetRight(currentNode);
        QueueNode(currentNode, threads, nextNode, Direction.Right);
    }
}

class Node
{
    public Node(char value, int row, int col)
    {
        Value = value;
        Row = row;
        Col = col;
        _visits = new Dictionary<Direction, int>();
    }
    public char Value { get; set; }
    public int Row { get; }
    public int Col { get; }
    private Dictionary<Direction, int> _visits;
    public void Visit(Direction direction)
    {
        if (_visits.ContainsKey(direction))
            _visits[direction]++;
        else
            _visits.Add(direction, 1);
    }

    public void ClearVisits() => _visits.Clear();

    public int Visits() 
    {
        int sum = 0;
        foreach (var key in _visits.Keys)
            sum += _visits[key];
        return sum;
    }

    public bool HasBeenVisited() => _visits.Any();
    
    public bool HasBeenVisitedFrom(Direction direction)
    {
        return _visits.ContainsKey(direction);
    }
}

class Move
{
    public Move(Node node, Direction direction)
    {
        Node = node;
        Direction = direction;
    }
    public Node Node { get; set; }
    public Direction Direction { get; set; }
}

enum Direction
{
    Up = '^',
    Down = 'V',
    Left = '<',
    Right = '>'
}