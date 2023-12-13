var input = new List<string> {
    "467..114..",
    "...*......",
    "..35..633.",
    "......#...",
    "617*......",
    ".....+.58.",
    "..592.....",
    "......755.",
    "...$.*....",
    ".664.598.."
};
var parts = new List<PartNumber>();
var symbols = new List<Symbol>();
for (int i = 0; i < input.Count; i++)
{
    var tokens = input[i].Split('.')
        .Where(x => !string.IsNullOrEmpty(x));

    if (tokens.Any())
    {
        foreach (var token in tokens)
        {
            if (int.TryParse(token, out var value))
            {
                var partNum = new PartNumber()
                {
                    Row = i,
                    StartColumn = input[i].IndexOf(token),
                    EndColumn = input[i].IndexOf(token) + token.Length,
                    Value = token
                };
                parts.Add(partNum);

            } 
            else
            {
                var symbol = new Symbol()
                {
                    Row = i,
                    Column = input[i].IndexOf(token),
                    Value = token
                };
                symbols.Add(symbol);
            }
        }
    }
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
    public string Value { get; set; }
}