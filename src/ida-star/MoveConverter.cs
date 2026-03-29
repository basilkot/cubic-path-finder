namespace IdaStarConsoleApp;

internal static class MoveConverter
{
    // F=1, Ff=2, R=3, Rr=4, U=5, Uu=6
    // + по часовой, - против часовой
    private static readonly Dictionary<string, string> Map = new()
    {
        ["F"]    = "1",   ["F'"]   = "-1",  ["F2"]   = "11",  ["F2'"]  = "-1-1", ["F3"]   = "111",  ["F3'"]  = "-1-1-1",
        ["Ff"]   = "2",   ["Ff'"]  = "-2",  ["Ff2"]  = "22",  ["Ff2'"] = "-2-2", ["Ff3"]  = "222",  ["Ff3'"] = "-2-2-2",
        ["R"]    = "3",   ["R'"]   = "-3",  ["R2"]   = "33",  ["R2'"]  = "-3-3", ["R3"]   = "333",  ["R3'"]  = "-3-3-3",
        ["Rr"]   = "4",   ["Rr'"]  = "-4",  ["Rr2"]  = "44",  ["Rr2'"] = "-4-4", ["Rr3"]  = "444",  ["Rr3'"] = "-4-4-4",
        ["U"]    = "5",   ["U'"]   = "-5",  ["U2"]   = "55",  ["U2'"]  = "-5-5", ["U3"]   = "555",  ["U3'"]  = "-5-5-5",
        ["Uu"]   = "6",   ["Uu'"]  = "-6",  ["Uu2"]  = "66",  ["Uu2'"] = "-6-6", ["Uu3"]  = "666",  ["Uu3'"] = "-6-6-6",
    };

    public static void ConvertFile(string filePath)
    {
        var lines = File.ReadAllLines(filePath);
        var output = new List<string>();

        foreach (var line in lines)
        {
            output.Add(line);

            // ищем строки с решениями: "  #N: move1 move2 ..."
            var trimmed = line.TrimStart();
            if (!trimmed.StartsWith("#"))
                continue;

            var colonIndex = trimmed.IndexOf(':');
            if (colonIndex < 0)
                continue;

            var movePart = trimmed[(colonIndex + 1)..].Trim();

            // убираем "(nodes: ...)" если есть
            var parenIndex = movePart.IndexOf('(');
            if (parenIndex >= 0)
                movePart = movePart[..parenIndex].Trim();

            var moves = movePart.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            var converted = new List<string>();

            foreach (var move in moves)
            {
                if (Map.TryGetValue(move, out var code))
                    converted.Add(code);
                else
                    converted.Add($"?{move}");
            }

            output.Add(string.Join("-", converted));
        }

        File.WriteAllLines(filePath, output);
        Console.WriteLine($"Converted {filePath}");
    }
}
