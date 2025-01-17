using System.Text.Json;
using CubeSolverConsoleApp;

class Program
{
    // L_B_D_R_F_U
    private static Dictionary<string, List<long>> fullStates = new()
    {
        { "initial", [0b0000000000_000000000_000000000_000000000_111111111_111111111_111111111] },
        { "0", [0b0000000000_000110010_000110010_000011010_111100101_111001101_101001111] },
        { "1", [0b0000000000_001001001_000000000_100000000_110110110_111111111_011111111] },
        { "2", [0b0000000000_010001000_000010110_111001000_111111001_111001101_001111001] },
        { "3", [0b0000000000_001000000_000111001_100101100_111110110_111001111_011011000] },

        { "4", [0b0000000000_010110010_000011000_010000000_101100101_111001101_111111111] },
        { "5", [0b0000000000_000101000_010010101_110100000_111010010_111001101_111011011] },
        /* initial to 5: Uu2 Rr2 E2 Uu R S' Rr U2
           0000000000000001000110110110100000010111010010111001101111011011
            */
        { "6", [0b0000000000_010011000_010011010_010011000_111100101_111001101_101001101] },
        { "7", [0b0000000000_000011000_000000001_100100010_101101101_111111011_011011111] },
        { "8", [0b0000000000_011011001_110111000_100110000_100101100_001101001_101001111] },
        { "9", [0b0000000000_010010010_010011000_010111000_101100101_111001101_101001111] },
    };
    // Helpers.PrintOutline(fullStates["2"].First());

    private static Dictionary<string, long> partialStates = new()
    {
        { "0", 0b0_111100101_111001101_101001111 },
        { "1", 0b0_110110110_111111111_011111111 },
        { "2", 0b0_111111001_111001101_001111001 },
        { "3", 0b0_111110110_111001111_011011000 },
        { "4", 0b0_101100101_111001101_111111111 },
        { "5", 0b0_111010010_111001101_111011011 },
        { "6", 0b0_111100101_111001101_101001101 },
        { "7", 0b0_101101101_111111011_011011111 },
        { "8", 0b0_100101100_001101001_101001111 },
        { "9", 0b0_101100101_111001101_101001111 },
    };

    public static void Main(string[] args)
    {
        ReadFiles();

        var skip = 0;
        string[] find = null;
        var deep = 7;
        var count = 1;
        if (args.Length > 0)
        {
            foreach (var arg in args)
            {
                if (arg[0] == 'f')
                {
                    find = arg.Substring(2).Split(',');
                }
                else if (arg[0] == 's')
                {
                    skip = int.Parse(arg.Substring(2));
                }
                else if (arg[0] == 'd')
                {
                    deep = int.Parse(arg.Substring(2));
                }
                else if (arg[0] == 'c')
                {
                    count = int.Parse(arg.Substring(2));
                }
                else if (arg[0] == 'e')
                {
                    var movesToExclude = arg.Substring(2).Split(',');
                    foreach (var move in movesToExclude)
                    {
                        Moves.Steps.Remove(move);
                    }
                }
                else if (arg[0] == 'p')
                {
                    Console.WriteLine(string.Join(',', Moves.Steps.Keys));
                    if (arg.Length > 1)
                    {
                        var s = arg.Substring(2).Replace("_", "");
                        var state = Convert.ToInt64(s, 2);
                        Helpers.PrintOutline(state);
                    }
                    return;
                }
                else if (arg[0] == 'w')
                {
                    var sourceAndPath = arg.Substring(2).Split(' ');
                    var source = sourceAndPath[0];
                    var pathOnly = sourceAndPath.Skip(1).ToArray();
                    var state = fullStates[source].Last();
                    SolverDeep.ValidatePath(state, pathOnly);
                    return;
                }
                else if (arg[0] == 'v')
                {
                    var stateFilePath = arg.Substring(2);
                    SolverDeep.PrepareSearchState(stateFilePath);
                }
                else if (arg[0] == 'h')
                {
                    Console.WriteLine("f - find, example: f=\"0 1\". ignores predefined paths to search");
                    Console.WriteLine("s - skip, example: s=10, skip 10 predefined searches");
                    Console.WriteLine("d - deep, max deep (actually it is d+1), default = 7");
                    Console.WriteLine("c - count, number solutions to search, default = 1");
                    Console.WriteLine("p - print all available moves, and outline of passed state");
                    Console.WriteLine("w - print step by step. example: w=initial F R' Rr");
                    Console.WriteLine(
                        "e - exclude, comma separated moves to exclude from paths, example: e=L,Ll,L',L2,Ll',Ll2");
                    return;
                }
            }
        }

        string[] pathToFind = find == null
            ? ReadPathToFind()
            : find;

        foreach (var pair in pathToFind.Skip(skip))
        {
            var sourceName = pair.Split(' ')[0];
            var targetName = pair.Split(' ')[1];

            if (!fullStates.TryGetValue(sourceName, out var sourceState))
            {
                continue;
            }

            var source = sourceState.Last();
            var checkFull = true;

            long t;
            if (fullStates.ContainsKey(targetName) && fullStates[targetName].Any())
            {
                t = fullStates[targetName].Last();
            }
            else
            {
                t = partialStates[targetName];
                checkFull = false;
            }

            var target = t;

            var context = new SolveContext
            {
                SolutionsCount = count,
                Source = sourceName,
                Target = targetName,
                SourceState = source,
                TargetState = target,
                CheckFull = checkFull,
                MaxDeep = deep,
                MinDeep = 0,
            };
            var solutions = SolverDeep.SolveCube(context);

            if (SolverDeep.Cancelled)
            {
                Console.WriteLine("Cancelled!");
                return;
            }

            if (solutions.Count > 0)
            {
                Console.WriteLine($"found {solutions.Count} solutions for {pair}");
            }
            else
            {
                Console.WriteLine($"no {context.MaxDeep + 1}th steps solution for {pair}");
            }

            SolverDeep.PrepareSearchState(null);
        }

        Console.WriteLine("finished!");

    }

    private static string[] ReadPathToFind()
    {
        if (File.Exists("path-to-find.txt"))
        {
            return File.ReadAllLines("path-to-find.txt")
                .Where(x => !x.StartsWith("//") && !string.IsNullOrWhiteSpace(x))
                .Select(x => x.IndexOf("//", StringComparison.Ordinal) == -1
                    ? x
                    : x.Substring(0, x.IndexOf("//", StringComparison.Ordinal)))
                .ToArray();
        }

        return
        [
            "initial 0", // U2 R S2 E2 Rr' S2 U2
            "initial 1", // F' Uu' E R U2 R'
            "initial 2", // Ff2 Uu' F2 E Rr F' Uu2 F2
            "initial 3", // Uu Ff R' F U' F' E'
            "initial 4", // R2 Ff' E Ff R2 S'
            "initial 5", // Rr' U2 R' S' M' U2 Ff Rr Uu2
            "initial 6", // R2 U S' Uu' R2 M2 E'
            "initial 7", // F' M' S' U' S' F
            "initial 8", // U' M Ff' Uu' Rr U2 Ff'
            "initial 9", // M F2 Rr' E' R F2 S2

            "0 1", // 
            "0 2", // 
            "0 3", // 
            "0 4", // 
            "0 5", // 
            "0 6", // 
            "0 7", // 
            "0 8", // 
            "0 9", // 

            "1 0",
            "1 2",
            "1 3",
            "1 4",
            "1 5",
            "1 6",
            "1 7",
            "1 8",
            "1 9",

            "2 0",
            "2 1",
            "2 3",
            "2 4",
            "2 5",
            "2 6",
            "2 7",
            "2 8",
            "2 9",

            "3 0",
            "3 1",
            "3 2",
            "3 4",
            "3 5",
            "3 6",
            "3 7",
            "3 8",
            "3 9",

            "4 0",
            "4 1",
            "4 2",
            "4 3",
            "4 5",
            "4 6",
            "4 7",
            "4 8",
            "4 9",

            "5 0",
            "5 1",
            "5 2",
            "5 3",
            "5 4",
            "5 6",
            "5 7",
            "5 8",
            "5 9",

            "6 0",
            "6 1",
            "6 2",
            "6 3",
            "6 4",
            "6 5",
            "6 7",
            "6 8",
            "6 9",

            "7 0",
            "7 1",
            "7 2",
            "7 3",
            "7 4",
            "7 5",
            "7 6",
            "7 8",
            "7 9",

            "8 0",
            "8 1",
            "8 2",
            "8 3",
            "8 4",
            "8 5",
            "8 6",
            "8 7",
            "8 9",

            "9 0",
            "9 1",
            "9 2",
            "9 3",
            "9 4",
            "9 5",
            "9 6",
            "9 7",
            "9 8",
        ];
    }

    private static void WriteFiles()
    {
        var fullStatesToSerialize = new Dictionary<string, List<string>>();
        foreach (var (key, value) in fullStates)
        {
            fullStatesToSerialize[key] = value.Select(Helpers.AsString).ToList();
        }

        var fsj = JsonSerializer.Serialize(fullStatesToSerialize, new JsonSerializerOptions { WriteIndented = true });

        File.WriteAllText("full-states.json", fsj);

        var partialStatesToSerialize = new Dictionary<string, string>();
        foreach (var (key, value) in partialStates)
        {
            partialStatesToSerialize[key] = Helpers.AsString(value);
        }
        var psj = JsonSerializer.Serialize(partialStatesToSerialize, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText("partial-states.json", psj);
        return;
    }

    private static void ReadFiles()
    {
        if (File.Exists("full-states.json"))
        {
            try
            {
                var fsj = File.ReadAllText("full-states.json");
                var fullStatesToDeserialize = JsonSerializer.Deserialize<Dictionary<string, List<string>>>(fsj, new JsonSerializerOptions
                {
                    ReadCommentHandling = JsonCommentHandling.Skip,
                    AllowTrailingCommas = true
                });
                var result = new Dictionary<string, List<long>>();
                foreach (var (key, value) in fullStatesToDeserialize)
                {
                    result[key] = value.Select(s => Convert.ToInt64(s, 2)).ToList();
                }

                fullStates = result;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        if (File.Exists("partial-states.json"))
        {
            try
            {
                var psj = File.ReadAllText("partial-states.json");
                var partialStatesToDeserialize = JsonSerializer.Deserialize<Dictionary<string, string>>(psj);
                var result = new Dictionary<string, long>();
                foreach (var (key, value) in partialStatesToDeserialize)
                {
                    result[key] = Convert.ToInt64(value, 2);
                }
                partialStates = result;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}
