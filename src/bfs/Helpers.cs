using System.Text;
using System.Text.Json;

namespace CubeSolverConsoleApp;

internal static class Helpers
{
    public static string AsString(this long value)
    {
        return Convert.ToString(value, 2).PadLeft(64, '0');
    }

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

    public static int[] SwapBitGroups(int[] value, int[] posGroup1, int[] posGroup2)
    {
        var groupSize = posGroup2.Length;

        var result = new int[value.Length];
        Array.Copy(value, result, value.Length);

        for (var i = 0; i < groupSize; i++)
        {
            result[posGroup1[i]] = value[posGroup2[i]];
        }

        return result;
    }

    public static bool Compare(int[] v1, int[] v2)
    {
        return v1.SequenceEqual(v2);
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

    public static bool Compare(long v1, long v2, bool checkFull)
    {
        if (checkFull)
        {
            return v1 == v2;
        }
        const long mask = 0b0000000000_000000000_000000000_000000000_111111111_111111111_111111111;
        var r1 = v1 & mask;
        var r2 = v2 & mask;
        return r1 == r2;
    }

    public static void AddToFile(string msg, string[] pathArray, long newState)
    {
        var path = string.Join(" ", pathArray);
        var value = string.Join(Environment.NewLine, new[] {
            msg,
            path,
            newState.AsString(),
            DateTime.Now.ToLongTimeString()
        }.Where(x => !string.IsNullOrEmpty(x))) + Environment.NewLine;
        File.AppendAllText("result.txt", value);
    }


    public static void PrintOutline(long state)
    {
        const string w = "\u2588\u2588";
        const string b = "\u2591\u2591";

        // convert long to bits array
        var bits = new bool[64];
        for (var i = 0; i < 64; i++)
        {
            bits[i] = (state & (1L << i)) != 0;
        }

        string t(int index)
        {
            //return bits[index] ? b : "  ";
            return bits[index] ? w : b;
        }

        var result = $@"         ╔══╤══╤══╗
         ║{t(00)}│{t(01)}│{t(02)}║
         ╟──┼──┼──╢
         ║{t(03)}│{t(04)}│{t(05)}║
         ╟──┼──┼──╢
         ║{t(06)}│{t(07)}│{t(08)}║
╔══╤══╤══╬══╪══╪══╬══╤══╤══╗
║{t(47)}│{t(46)}│{t(45)}║{t(09)}│{t(10)}│{t(11)}║{t(18)}│{t(19)}│{t(20)}║
╟──┼──┼──╫──┼──┼──╫──┼──┼──╢
║{t(50)}│{t(49)}│{t(48)}║{t(12)}│{t(13)}│{t(14)}║{t(21)}│{t(22)}│{t(23)}║
╟──┼──┼──╫──┼──┼──╫──┼──┼──╢
║{t(53)}│{t(52)}│{t(51)}║{t(15)}│{t(16)}│{t(17)}║{t(24)}│{t(25)}│{t(26)}║
╚══╧══╧══╬══╪══╪══╬══╧══╧══╝
         ║{t(33)}│{t(34)}│{t(35)}║
         ╟──┼──┼──╢
         ║{t(30)}│{t(31)}│{t(32)}║
         ╟──┼──┼──╢
         ║{t(27)}│{t(28)}│{t(29)}║
         ╠══╪══╪══╣
         ║{t(42)}│{t(43)}│{t(44)}║
         ╟──┼──┼──╢
         ║{t(39)}│{t(40)}│{t(41)}║
         ╟──┼──┼──╢
         ║{t(36)}│{t(37)}│{t(38)}║
         ╚══╧══╧══╝
";
        Console.WriteLine(result);
    }

    public static void SaveState(Dictionary<string, List<DeepSearchState>> searchStates, string filename)
    {
        var result = JsonSerializer.Serialize(searchStates,
            new JsonSerializerOptions
            {
                WriteIndented = true,
            }).Replace("\\u0027", "'");

        File.WriteAllText(filename, result, Encoding.UTF8);
    }

    public static Dictionary<string, List<DeepSearchState>> ReadStates(string filename)
    {
        if (File.Exists(filename))
        {
            var result = File.ReadAllText(filename);
            return JsonSerializer.Deserialize<Dictionary<string, List<DeepSearchState>>>(result);
        }
        return new Dictionary<string, List<DeepSearchState>>();
    }
}

