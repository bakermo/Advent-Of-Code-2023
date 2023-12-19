
bool useTest = false;
string fileName = useTest ? "sample.txt" : "input.txt";

var input = File.ReadAllLines(fileName);
PartOne(input);

//PartTwo(input);

void PartOne(string[] input)
{
    var parts = new List<Part>();
    var workFlows = new Dictionary<string, WorkFlow>();
    Load(input, parts, workFlows);
    Queue<WorkFlow> queue = new Queue<WorkFlow>();
    var acceptedParts = new List<Part>();
    foreach (var part in parts)
    {
        var startingWorkflow = workFlows["in"];
        queue.Enqueue(startingWorkflow);

        while (queue.Count > 0)
        {
            var currentWorkflow = queue.Dequeue();
            var rules = currentWorkflow.Rules;
            foreach (var rule in rules)
            {
                int? field;
                switch (rule.Field)
                {
                    case 'x':
                        field = part.X;
                        break;
                    case 'm':
                        field = part.M;
                        break;
                    case 'a':
                        field = part.A;
                        break;
                    case 's':
                        field = part.S;
                        break;
                    default:
                        field = null;
                        break;
                }

                if (field == null)
                {
                    if (rule.Destination == "A")
                        acceptedParts.Add(part);
                    else if (workFlows.ContainsKey(rule.Destination))
                        queue.Enqueue(workFlows[rule.Destination]);

                    break;
                }
                else if ((rule.Operator == Operator.LessThan && field < rule.Threshold) ||
                        (rule.Operator == Operator.GreaterThan) && field > rule.Threshold)
                {
                    if (workFlows.ContainsKey(rule.Destination))
                    {
                        queue.Enqueue(workFlows[rule.Destination]);
                    }
                    else if (rule.Destination == "A")
                    {
                        acceptedParts.Add(part);
                    }

                    // otherwise, its an R

                    break;
                }
                // otherwise, go to next rule
            }
        }
        
    }

    int sum = acceptedParts.Sum(p => p.X + p.M + p.A + p.S);
    Console.WriteLine(sum);
}


void PartTwo(string[] input)
{

}



void Load(string[] input, List<Part> parts, Dictionary<string, WorkFlow> workFlows)
{
    foreach (var line in input)
    {
        if (!string.IsNullOrEmpty(line))
        {
            if (line.StartsWith('{'))
            {
                // this is a part
                var raw = line.TrimStart('{').TrimEnd('}').Trim();
                var fields = raw.Split(',');
                var part = new Part();
                foreach (var field in fields)
                {
                    var tokens = field.Split('=');
                    var key = tokens[0].Trim();
                    var value = tokens[1].Trim();
                    switch (key)
                    {
                        case "x":
                            part.X = int.Parse(value);
                            break;
                        case "m":
                            part.M = int.Parse(value);
                            break;
                        case "a":
                            part.A = int.Parse(value);
                            break;
                        case "s":
                            part.S = int.Parse(value);
                            break;
                    }
                }
                parts.Add(part);
            }
            else
            {
                var raw = line.Split('{');
                var id = raw[0].Trim();
                var flow = raw[1].TrimEnd('}').Trim();
                var steps = flow.Split(',');
                var workFlow = new WorkFlow()
                {
                    ID = id,
                    Rules = new List<Rule>()
                };

                for (int i = 0; i < steps.Length; i++)
                {

                    if (i == steps.Length - 1)
                        workFlow.Rules.Add(new Rule() { Destination = steps[i].Trim() });
                    else
                    {
                        var step = steps[i];
                        char field = step[0];
                        Operator op = (Operator)step[1];
                        var rest = step.Substring(2);
                        var tokens = rest.Split(':');
                        int value = int.Parse(tokens[0]);
                        workFlow.Rules.Add(new Rule()
                        {
                            Operator = op,
                            Destination = tokens[1].Trim(),
                            Field = field,
                            Threshold = value
                        });
                    }
                }
                workFlows.Add(id, workFlow);
            }
        }
    }
}

class Part
{
    public int X { get; set; }
    public int M { get; set; }
    public int A { get; set; }
    public int S { get; set; }
}

class WorkFlow
{
    public string ID { get; set; }
    public List<Rule> Rules { get; set; }
}

class Rule
{
    public Operator? Operator { get; set; }
    public int? Threshold { get; set; }
    public char? Field { get; set; }
    public string Destination { get; set; }
}

enum Operator
{
    LessThan = '<',
    GreaterThan = '>',
}