//var input = new string[]
//{
//    "32T3K 765", // 1 pair (5)
//    "T55J5 684", // 3 of kind lower (2)
//    "KK677 28", // 2 pair higher (3)
//    "KTJJT 220", // 2 pair lower (4)
//    "QQQJA 483" // 3 of kind (1)
//};

var input = File.ReadAllLines("values.txt");
PartOne(input);
//PartTwo(input);

void PartOne(string[] inputs)
{
    var hands = new List<Hand>();
    foreach (var input in inputs)
    {
        var handPart = input.Substring(0, 5);
        var bidPart = input.Substring(5, input.Length - 5).Trim();
        var cardsForHand = new List<Card>();
        hands.Add(new Hand(handPart.Select(c => new Card(c)), int.Parse(bidPart)));  
    }

    hands.Sort();
    var bids = new List<int>();
    for (int i = 0; i < hands.Count; i++)
        bids.Add((i + 1) * hands[i].Bid);

    Console.WriteLine("Total winnings: " + bids.Sum());
}

void PartTwo(string[] inputs)
{
    
}

public class Hand : IComparable<Hand>
{
    public Hand(IEnumerable<Card> cards, int bid)
    {
        Cards = cards.ToArray();
        Bid = bid;
        HandType = RankHand();
    }

    public int Bid { get; }
    public Card[] Cards { get; }
    public HandType HandType { get; }

    public int CompareTo(Hand? other)
    {
        if (other == null)
            return 1;
        int handCompare = this.HandType.CompareTo(other.HandType);
        if (handCompare == 0)
        {
            for (int i = 0; i < Cards.Length; i++)
            {
                int compare = this.Cards[i].CompareTo(other.Cards[i]);
                if (compare != 0)
                    return compare;
            }
        }
        return handCompare;
    }

    private HandType RankHand()
    {
        var uniqueCards = new Dictionary<CardRank, int>();
        foreach (var card in Cards)
            if (uniqueCards.ContainsKey(card.CardRank))
                uniqueCards[card.CardRank]++;
            else
                uniqueCards.Add(card.CardRank, 1);

        switch (uniqueCards.Count)
        {
            case 1: // only 1 group means they are all the same
                return HandType.FiveOfAKind;
            case 2:
                if (uniqueCards.Any(s => s.Value == 4))
                    return HandType.FourOfAKind;
                else
                    return HandType.FullHouse;
            case 3:
                if (uniqueCards.Any(s => s.Value == 3))
                    return HandType.ThreeOfAKind;
                else
                    return HandType.TwoPair;
            case 4:
                return HandType.OnePair;
            default:
                return HandType.HighCard;
        }
    }
}

public class Card : IComparable<Card>
{
    public Card(char rawValue)
    {
        RawValue = rawValue;
        switch (rawValue)
        {
            case 'T':
                CardRank = CardRank.Ten;
                break;
            case 'J':
                CardRank = CardRank.Jack;
                break;
            case 'Q':
                CardRank = CardRank.Queen;
                break;
            case 'K':
                CardRank = CardRank.King;
                break;
            case 'A':
                CardRank = CardRank.Ace;
                break;
            default:
                CardRank = (CardRank)int.Parse(rawValue.ToString());
                break;
        }            
    }
    public char RawValue { get; }
    public CardRank CardRank { get; }

    public int CompareTo(Card? other)
    {
        if (other == null)
            return 1;

        return this.CardRank.CompareTo(other.CardRank);
    }
}

public enum CardRank
{
    Two = 2,
    Three,
    Four,
    Five,
    Six,
    Seven,
    Eight,
    Nine,
    Ten,
    Jack,
    Queen,
    King,
    Ace
}

public enum HandType
{
    HighCard,
    OnePair,
    TwoPair,
    ThreeOfAKind,
    FullHouse,
    FourOfAKind,
    FiveOfAKind
}