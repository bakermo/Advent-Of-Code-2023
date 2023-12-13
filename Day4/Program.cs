//var input = new string[]
//{
//    "Card 1: 41 48 83 86 17 | 83 86  6 31 17  9 48 53",
//    "Card 2: 13 32 20 16 61 | 61 30 68 82 17 32 24 19",
//    "Card 3:  1 21 53 59 44 | 69 82 63 72 16 21 14  1",
//    "Card 4: 41 92 73 84 69 | 59 84 76 51 58  5 54 83",
//    "Card 5: 87 83 26 28 32 | 88 30 70 12 93 22 82 36",
//    "Card 6: 31 18 13 56 72 | 74 77 10 23 35 67 36 11",
//};

var input = File.ReadAllLines("values.txt");
//PartOne(input);
PartTwo(input);

void PartOne(string[] inputs)
{
    var sums = new List<int>();
    foreach (var card in input)
    {
        int sum = 0;
        int count = -1;
        var tokens = card.Split(":");
        var cardId = tokens[0];
        var values = tokens[1];
        var numberTokens = values.Split("|");
        var winningNumbers = numberTokens[0]
                                .Trim()
                                .Split(" ")
                                .Where(x => !string.IsNullOrEmpty(x))
                                .Select(x => int.Parse(x.Trim()))
                                .ToHashSet();

        var scratchNumbers = numberTokens[1]
                                .Trim()
                                .Split(" ")
                                .Where(x => !string.IsNullOrEmpty(x))
                                .Select(x => int.Parse(x.Trim()))
                                .ToList();


        foreach (var number in scratchNumbers)
        {
            if (winningNumbers.Contains(number))
                count++;
        }
        sum = Convert.ToInt32(Math.Pow(2.0, count));

        sums.Add(sum);
        //Console.WriteLine(sum);
    }
    Console.WriteLine(sums.Sum());
}

void PartTwo(string[] inputs)
{
    int cardNumber = 1;
    var queue = new Queue<Card>();
    var knownCards = new Dictionary<int, Card>();

    foreach (var input in inputs)
    {
        var tokens = input.Split(":");
        var cardIdToken = tokens[0];
        var values = tokens[1];
        var numberTokens = values.Split("|");
        var winningNumbers = numberTokens[0]
                                .Trim()
                                .Split(" ")
                                .Where(x => !string.IsNullOrEmpty(x))
                                .Select(x => int.Parse(x.Trim()))
                                .ToHashSet();

        var scratchNumbers = numberTokens[1]
                                .Trim()
                                .Split(" ")
                                .Where(x => !string.IsNullOrEmpty(x))
                                .Select(x => int.Parse(x.Trim()))
                                .ToList();

        var cardIdSplit = cardIdToken.Split("Card");
        var cardId = int.Parse(cardIdSplit[1].Trim());

        var card = new Card
        {
            Id = cardId,
            RawValue = input,
            WinningNumbers = winningNumbers,
            ScratchCardNumbers = scratchNumbers,
        };

        queue.Enqueue(card);
        knownCards.Add(card.Id, card);
    }

    int cardCount = 0;
    while (queue.Count > 0)
    {
        var card = queue.Dequeue();
        int sum = 0;
        int count = 0;
        cardCount++;
        foreach (var number in card.ScratchCardNumbers)
        {
            if (card.WinningNumbers.Contains(number))
                count++;
        }
        if (count > 0)
        {
            for (int i = 1; i <= count; i++)
            {
                queue.Enqueue(knownCards[card.Id + i]);
            }
        }
    }
    Console.WriteLine(cardCount);
}

class Card
{
    public int Id { get; set; }
    public string RawValue { get; set; }
    public HashSet<int> WinningNumbers { get; set; }
    public List<int> ScratchCardNumbers { get; set; }
}