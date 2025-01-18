namespace AStarConsoleApp;

internal class Program
{
    static void Main(string[] args)
    {
        // var path = "2 4";

        List<string> allPath = ["4 2",
            "5 4",
            "6 2"];

        while (allPath.Any())
        {
            foreach (var path in allPath.ToList())
            {
                Console.WriteLine($"search for {path}");

                var pair = path.Split(" ");

                var context = new SearchContext
                {
                    SourceName = pair[0],
                    TargetName = pair[1],
                    Source = fullStates[pair[0]],
                    Target = fullStates[pair[1]],
                    FullOverlap = true,
                    RandomizeMovesOrder = true,
                    // Ignore = []
                    // Ignore = ["X", "X'", "X2", "Y", "Y'", "Y2", "Z", "Z'", "Z2"]

                    Ignore =
                    [
                        "L", "L'", "L2",
                        "Ll", "Ll'", "Ll2",

                        "D", "D'", "D2",
                        "Dd", "Dd'", "Dd2",

                        "B", "B'", "B2",
                        "Bb", "Bb'", "Bb2",

                        "X", "X'", "X2",
                        "Y", "Y'", "Y2",
                        "Z", "Z'", "Z2"
                    ]
                    /**/
                };

                var result = RubikAStarSolver.AStarSearch(context);
                if (result == null)
                {
                    Console.WriteLine("Решение не найдено");
                }
                else
                {
                    allPath.Remove(path);
                    Console.WriteLine("Решение найдено:");
                    Console.WriteLine(string.Join(" ", result));
                    File.AppendAllLines("results.txt", [path, string.Join(" ", result), "------------"]);
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