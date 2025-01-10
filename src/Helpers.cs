namespace CubeSolverConsoleApp;

internal class Helpers
{
    private static string PrintBits(long value, int[] indexes)
    {
        return indexes
            .Select(index => (value >> index) & 1)
            .Aggregate("", (current, bit) => current + bit);
    }

    public static void Print(long value)
    {
        Console.WriteLine();
        Console.WriteLine(PrintBits(value, [0, 1, 2]).PadLeft(7, ' '));
        Console.WriteLine(PrintBits(value, [3, 4, 5]).PadLeft(7, ' '));
        Console.WriteLine(PrintBits(value, [6, 7, 8]).PadLeft(7, ' '));
        Console.WriteLine();

        Console.Write(PrintBits(value, [47, 46, 45]));
        Console.Write(" ");
        Console.Write(PrintBits(value, [9, 10, 11]));
        Console.Write(" ");
        Console.WriteLine(PrintBits(value, [18, 19, 20]));

        Console.Write(PrintBits(value, [50, 49, 48]));
        Console.Write(" ");
        Console.Write(PrintBits(value, [12, 13, 14]));
        Console.Write(" ");
        Console.WriteLine(PrintBits(value, [21, 22, 23]));

        Console.Write(PrintBits(value, [53, 52, 51]));
        Console.Write(" ");
        Console.Write(PrintBits(value, [15, 16, 17]));
        Console.Write(" ");
        Console.WriteLine(PrintBits(value, [24, 25, 26]));

        Console.WriteLine();

        Console.WriteLine(PrintBits(value, [33, 34, 35]).PadLeft(7, ' '));
        Console.WriteLine(PrintBits(value, [30, 31, 32]).PadLeft(7, ' '));
        Console.WriteLine(PrintBits(value, [27, 28, 29]).PadLeft(7, ' '));

        Console.WriteLine();

        Console.WriteLine(PrintBits(value, [42, 43, 44]).PadLeft(7, ' '));
        Console.WriteLine(PrintBits(value, [39, 40, 41]).PadLeft(7, ' '));
        Console.WriteLine(PrintBits(value, [36, 37, 38]).PadLeft(7, ' '));

        Console.WriteLine();
    }

    public static long SwapBitGroups(long value, int[] posGroup1, int[] posGroup2)
    {
        var groupSize = posGroup2.Length;

        var result = value;

        for (var i = 0; i < groupSize; i++)
        {
            var bit = (value >> posGroup2[i]) & 1;
            result &= ~(1L << posGroup1[i]);
            result |= (bit << posGroup1[i]);
        }

        return result;
    }

    public static bool Compare(long v1, long v2)
    {
        const long mask = 0b0000000000_000000000_000000000_000000000_111111111_111111111_111111111;
        var r1 = v1 & mask;
        var r2 = v2 & mask;
        return r1 == r2;
    }

    public static void AddToFile(string[] pathArray, long newState)
    {
        var path = string.Join(" ", pathArray);
        File.AppendAllText("result.txt", path + Environment.NewLine);
        File.AppendAllText("result.txt", Convert.ToString(newState, 2).PadLeft(64, '0') + Environment.NewLine);
        File.AppendAllText("result.txt", DateTime.Now.ToLongTimeString() + Environment.NewLine);
    }
}

