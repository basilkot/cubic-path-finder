namespace AStarConsoleApp;

internal class Program
{
    static void Main(string[] args)
    {
        // var path = "2 4";

        //var minutes = 5;
        //foreach (var arg in args)
        //{
        //    if (arg[0] == 't')
        //    {
        //        minutes = int.Parse(arg[2..]);
        //    }
        //}

        /*
        "i 0", "i 1", "i 2", "i 3", "i 4", "i 5", "i 6", "i 7", "i 8", "i 9",
        "0 1", "0 2", "0 3", "0 4", "0 5", "0 6", "0 7", "0 8", "0 9",
        "1 0", "1 2", "1 3", "1 4", "1 5", "1 6", "1 7", "1 8", "1 9",
        "2 0", "2 1", "2 3", "2 4", "2 5", "2 6", "2 7", "2 8", "2 9",
        "3 0", "3 1", "3 2", "3 4", "3 5", "3 6", "3 7", "3 8", "3 9",
        "4 0", "4 1", "4 2", "4 3", "4 5", "4 6", "4 7", "4 8", "4 9",
        "5 0", "5 1", "5 2", "5 3", "5 4", "5 6", "5 7", "5 8", "5 9",
        "6 0", "6 1", "6 2", "6 3", "6 4", "6 5", "6 7", "6 8", "6 9",
        "7 0", "7 1", "7 2", "7 3", "7 4", "7 5", "7 6", "7 8", "7 9",
        "8 0", "8 1", "8 2", "8 3", "8 4", "8 5", "8 6", "8 7", "8 9",
        "9 0", "9 1", "9 2", "9 3", "9 4", "9 5", "9 6", "9 7", "9 8"
        */

        var allPath = File.ReadAllLines("paths.txt").ToList();
        var rnd = new Random();

        while (allPath.Any())
        {
            foreach (var path in allPath.ToList())
            {
                Console.WriteLine(DateTime.Now);
                Console.WriteLine($"search for {path}");

                var pair = path.Split(" ", StringSplitOptions.RemoveEmptyEntries);
                //var inverse = rnd.Next(0, 2) == 0;
                var inverse = false;

                var sourceName = string.Empty;
                var targetName = string.Empty;
                var sourceState = 0L;
                var targetState = 0L;

                if (pair.Length == 2)
                {
                    var fromIndex = inverse ? 1 : 0;
                    var toIndex = inverse ? 0 : 1;

                    sourceName = pair[fromIndex];
                    targetName = pair[toIndex];

                    sourceState = fullStates[pair[fromIndex]];
                    targetState = fullStates[pair[toIndex]];
                }
                else
                {
                    sourceName = path;
                    targetName = "inverse-" + path;
                    sourceState = fullStates[path];
                    targetState = ~sourceState & 0b0000000000_111111111_111111111_111111111_111111111_111111111_111111111L;
                }

                Console.WriteLine(Convert.ToString((long)sourceState, 2).PadLeft(64, '0'));
                Console.WriteLine(Convert.ToString((long)targetState, 2).PadLeft(64, '0'));


                var context = new SearchContext
                {
                    SourceName = inverse ? targetName : sourceName,
                    TargetName = inverse ? sourceName : targetName,
                    Source = inverse ? targetState : sourceState,
                    Target = inverse ? sourceState : targetState,
                    FullOverlap = true,
                    RandomizeMovesOrder = true,
                    Time = rnd.Next(5, 16),
                    //Time = -1,
                    // Ignore = []
                    // Ignore = ["X", "X'", "X2", "Y", "Y'", "Y2", "Z", "Z'", "Z2"]

                    Ignore =
                    [
                        //"L", "L'", "L2",
                        //"Ll", "Ll'", "Ll2",

                        //"D", "D'", "D2",
                        //"Dd", "Dd'", "Dd2",

                        //"B", "B'", "B2",
                        //"Bb", "Bb'", "Bb2",

                        "X", "X'", "X2",
                        "Y", "Y'", "Y2",
                        "Z", "Z'", "Z2"
                    ]
                    /**/
                };
                if (inverse) Console.WriteLine("inverse");
                Console.WriteLine(context.Time);

                Console.WriteLine("Building pattern database...");
                RubikHeuristics.BuildPatternDatabase(context);

                Console.WriteLine("Searching...");
                var result = RubikAStarSolver.AStarSearch(context);
                if (result == null)
                {
                    Console.WriteLine("Решение не найдено");
                }
                else
                {
                    if (inverse)
                    {
                        result.Reverse();
                        result = result.Select(x => x.EndsWith("'") ? x[..^1] : x.EndsWith("2") ? x : x + "'").ToList();
                    }
                    allPath.Remove(path);
                    File.WriteAllLines("paths.txt", allPath);
                    Console.WriteLine("Решение найдено:");
                    Console.WriteLine(string.Join(" ", result));
                    File.AppendAllLines("results.txt", [path, string.Join(" ", result.Select(x => $"`{x}`")), "------------"]);
                }
            }

        }
    }

    // L_B_D_R_F_U
    private static Dictionary<string, long> fullStates = new()
    {
        { "i", 0b0000000000_000000000_000000000_000000000_111111111_111111111_111111111L },
        { "0", 0b0000000000_000110010_000110010_000011010_111100101_111001101_101001111L },
        { "1", 0b0000000000_001001001_000000000_100000000_110110110_111111111_011111111L },
        { "2", 0b0000000000_010001000_000010110_111001000_111111001_111001101_001111001L },
        { "3", 0b0000000000_001000000_000111001_100101100_111110110_111001111_011011000L },
        { "4", 0b0000000000_010110010_000011000_010000000_101100101_111001101_111111111L },
        { "5", 0b0000000000_000101000_010010101_110100000_111010010_111001101_111011011L },
        { "6", 0b0000000000_010011000_010011010_010011000_111100101_111001101_101001101L },
        { "7", 0b0000000000_000011000_000000001_100100010_101101101_111111011_011011111L },
        { "8", 0b0000000000_011011001_110111000_100110000_100101100_001101001_101001111L },
        { "9", 0b0000000000_010010010_010011000_010111000_101100101_111001101_101001111L },
    };

}