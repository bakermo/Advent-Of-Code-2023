int redLimit = 12;
int blueLimit = 14;
int greenLimit = 13;
//var gameInput = new List<string>()
//{
//    "Game 1: 3 blue, 4 red; 1 red, 2 green, 6 blue; 2 green",
//    "Game 2: 1 blue, 2 green; 3 green, 4 blue, 1 red; 1 green, 1 blue",
//    "Game 3: 8 green, 6 blue, 20 red; 5 blue, 4 red, 13 green; 5 green, 1 red",
//    "Game 4: 1 green, 3 red, 6 blue; 3 green, 6 red; 3 green, 15 blue, 14 red",
//    "Game 5: 6 red, 1 blue, 3 green; 2 blue, 1 red, 2 green"
//};

var gameInput = File.ReadLines("values.txt");
var gamePowers = new List<int>();
//var validGameIds = new List<int>();
foreach (var input in gameInput)
{
    bool gameIsValid = true;
    var tokens = input.Split(":");
    var gameToken = tokens[0];
    int gameId = int.Parse(gameToken.Split(" ")[1]);

    var gameResult = tokens[1];
    var runs = gameResult.Split(";");
    int minRed = 0;
    int minBlue = 0;
    int minGreen = 0;
    foreach (var run in runs)
    {
        var selections = run.Trim().Split(",");
        var green = selections.FirstOrDefault(x => x.EndsWith("green"));
        var red = selections.FirstOrDefault(x => x.EndsWith("red"));
        var blue = selections.FirstOrDefault(x => x.EndsWith("blue"));

        int redCount = GetCubeCount(red);
        int blueCount = GetCubeCount(blue);
        int greenCount = GetCubeCount(green);

        if (redCount > minRed)
            minRed = redCount;
        if (blueCount > minBlue)
            minBlue = blueCount;
        if (greenCount > minGreen)
            minGreen = greenCount;
    }
    var power = minRed * minBlue * minGreen;
    gamePowers.Add(power);
   
}

foreach (var game in gamePowers)
    Console.WriteLine(game);
//foreach (var game in validGameIds)
//    Console.WriteLine(game);

//Console.Write("Sum of valid IDs: " + validGameIds.Sum());
Console.Write("Sum of valid IDs: " + gamePowers.Sum());

int GetCubeCount(string selection)
{
    if (string.IsNullOrEmpty(selection))
        return 0;
    selection = selection.Trim();
    var tokens = selection.Split(" ");
    return int.Parse(tokens[0]);
}

bool IsValid(int limit, string selection)
{
    var cubeCount = GetCubeCount(selection);
    if (cubeCount > limit) return false;

    return true;
}