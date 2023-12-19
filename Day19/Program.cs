bool useTest = false;
string fileName = useTest ? "sample.txt" : "input.txt";

var input = File.ReadAllLines(fileName);
//PartOne(input);

PartTwo(input);