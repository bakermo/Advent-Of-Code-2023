using System.Security.AccessControl;
using System.Text.RegularExpressions;

var rawValues = File.ReadLines("values.txt").ToList();
////foreach (var rawValue in rawValues)
////    Console.WriteLine(rawValue);

//var rawValues = new List<string>()
//{
//    "two1nine",
//    "eightwothree",
//    "abcone2threexyz",
//    "xtwone3four",
//    "4nineeightseven2",
//    "zoneight234",
//    "7pqrstsixteen",
//};
//test
var textNumMap = new Dictionary<string, int>()
    {
        { "one", 1 },
        { "two", 2 },
        { "three", 3 },
        { "four", 4 },
        { "five", 5 },
        { "six", 6 },
        { "seven", 7 },
        { "eight", 8 },
        { "nine", 9 }
    };

var calibratedValues = new List<int>();

foreach (var rawValue in rawValues)
{
    Console.WriteLine(rawValue);
    //var convertedValue = GetConvertedValue(rawValue);
    var indexesOfNumbers = new Dictionary<int, string>();
    string sanitized = string.Empty;
    for (int i = 0; i < rawValue.Length; i++)
    {
        if (Char.IsDigit(rawValue[i]))
            indexesOfNumbers.Add(i, rawValue[i].ToString());
    }

    foreach (var num in textNumMap)
    {
        var index = rawValue.IndexOf(num.Key);
        if (index > -1)
            indexesOfNumbers.Add(index, num.Value.ToString());

        var lastIndex = rawValue.LastIndexOf(num.Key);
        if (index > -1 && !indexesOfNumbers.ContainsKey(lastIndex))
            indexesOfNumbers.Add(lastIndex, num.Value.ToString());
    }



    var first = indexesOfNumbers.OrderBy(x => x.Key).First().Value;
    var last = indexesOfNumbers.OrderBy(x => x.Key).Last().Value;
    var calibratedStr = $"{first}{last}";
    Console.WriteLine(rawValue + " " + calibratedStr);
    calibratedValues.Add(int.Parse(calibratedStr.Trim()));

    //foreach (char ch in convertedValue)
    //{
    //    if (Char.IsDigit(ch))
    //        sanitized += ch;
    //}
    //if (!string.IsNullOrEmpty(sanitized))
    //{
    //    var calibrated = string.Concat(sanitized.First(), sanitized.Last());

    //    if (!string.IsNullOrEmpty(calibrated))
    //        calibratedValues.Add(int.Parse(calibrated));
    //    Console.WriteLine("Raw Value: " + rawValue + " Converted: " + convertedValue + " Calibrated: " + calibrated);
    //}
}

string GetConvertedValue(string value)
{
    bool completedConversion = false;
   
    while (true)
    {
        var indexes = new Dictionary<int, string>();

        foreach (var pair in textNumMap)
        {
            var indexOfSpelledNumber = value.IndexOf(pair.Key);
            if (indexOfSpelledNumber > -1)
                indexes.Add(indexOfSpelledNumber, pair.Key);
        }

        if (indexes.Count == 0)
            break;
        //completedConversion = true;
        var index = indexes.OrderBy(x => x.Key).FirstOrDefault();
        value = value.Remove(index.Key, index.Value.Length);
        value = value.Insert(index.Key, textNumMap[index.Value].ToString());
        //var regex = new Regex(index.Value);
        //value = regex.Replace(value, textNumMap[index.Value].ToString(), 1);
        //foreach (var index in indexes.OrderBy(x => x.Key))
        //{
        //    var regex = new Regex(index.Value);
        //    value = regex.Replace(value, textNumMap[index.Value].ToString(), 1);
        //}
    }
    return value;
}

Console.WriteLine(calibratedValues.Sum());
Console.ReadLine();