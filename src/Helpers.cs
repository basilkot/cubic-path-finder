namespace CubeSolverConsoleApp;

internal class Helpers
{
    private static string PrintBits(long value, int[] indexes)
    {
        var str = "";
        for (int i = 0; i < indexes.Length; i++)
        {
            long bit = (value >> indexes[i]) & 1;  // Извлекаем бит
            str += bit;  // Добавляем бит в начало строки
        }

        return str;
    }

    public static void Print(long value)
    {
        Console.WriteLine(PrintBits(value, [0, 1, 2]).PadLeft(6, ' '));
        Console.WriteLine(PrintBits(value, [3, 4, 5]).PadLeft(6, ' '));
        Console.WriteLine(PrintBits(value, [6, 7, 8]).PadLeft(6, ' '));
        Console.WriteLine(PrintBits(value, [47, 46, 45, 9, 10, 11, 18, 19, 20]));
        Console.WriteLine(PrintBits(value, [50, 49, 48, 12, 13, 14, 21, 22, 23]));
        Console.WriteLine(PrintBits(value, [53, 52, 51, 15, 16, 17, 24, 25, 26]));
        Console.WriteLine(PrintBits(value, [33, 34, 35]).PadLeft(6, ' '));
        Console.WriteLine(PrintBits(value, [30, 31, 32]).PadLeft(6, ' '));
        Console.WriteLine(PrintBits(value, [27, 28, 29]).PadLeft(6, ' '));
        Console.WriteLine(PrintBits(value, [42, 43, 44]).PadLeft(6, ' '));
        Console.WriteLine(PrintBits(value, [39, 40, 41]).PadLeft(6, ' '));
        Console.WriteLine(PrintBits(value, [36, 37, 38]).PadLeft(6, ' '));
    }

    public static long SwapBitGroups(long value, int[] posGroup1, int[] posGroup2)
    {
        if (posGroup1.Length != posGroup2.Length)
            throw new ArgumentException("Группы битов должны быть одинакового размера");

        int groupSize = posGroup2.Length;

        var result = value;

        for (int i = 0; i < groupSize; i++)
        {
            long bit = (value >> posGroup2[i]) & 1;
            result &= ~(1L << posGroup1[i]);
            result |= (bit << posGroup1[i]);
        }

        return result;
    }

    public static bool Compare(long v1, long v2)
    {
        const long mask = 0b0000000000_000000000_000000000_000000000_111111111_111111111_111111111;
        var r1 = (v1 & mask);
        var r2 = (v2 & mask);
        if (r1 == r2)
        {
            return true;
        }

        return false;
    }

    public static long SwapBitGroups1(long value, int[] posGroup1, int[] posGroup2)
    {
        if (posGroup1.Length != posGroup2.Length)
            throw new ArgumentException("Группы битов должны быть одинакового размера");

        int groupSize = posGroup1.Length;

        // Создаем маски битов
        long group1 = 0;
        long group2 = 0;

        // Извлекаем группы битов из соответствующих позиций
        for (int i = 0; i < groupSize; i++)
        {
            group1 |= ((value >> posGroup1[i]) & 1) << i;  // Формируем группу 1
            group2 |= ((value >> posGroup2[i]) & 1) << i;  // Формируем группу 2
        }

        // Если группы разные, меняем их
        if (group1 != group2)
        {
            for (int i = 0; i < groupSize; i++)
            {
                // Меняем биты местами с помощью XOR
                var t1 = 1L << posGroup1[i];
                var t2 = 1L << posGroup2[i];


                //value ^= ((long)1 << posGroup1[i]);  // Переключаем бит на первой группе
                //value ^= ((long)1 << posGroup2[i]);  // Переключаем бит на второй группе                //value ^= ((long)1 << posGroup1[i]);  // Переключаем бит на первой группе
                value ^= t1;  // Переключаем бит на второй группе                //value ^= ((long)1 << posGroup1[i]);  // Переключаем бит на первой группе
                value ^= t2;  // Переключаем бит на второй группе
            }
        }

        return value;  // Возвращаем измененное число
    }

    public static void AddToFile(string[] pathArray, long newState)
    {
        var path = string.Join(" ", pathArray);
        File.AppendAllText("result.txt", path + Environment.NewLine);
        File.AppendAllText("result.txt", Convert.ToString(newState, 2).PadLeft(64, '0') + Environment.NewLine);
        File.AppendAllText("result.txt", DateTime.Now.ToLongTimeString() + Environment.NewLine);
    }
}

