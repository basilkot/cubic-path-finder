namespace AStarConsoleApp
{
    internal class RubikHeuristics
    {
        public static bool IsGoal(long start, long target, bool full)
        {
            if (full)
            {
                return start == target;
            }
            const long mask = 0b0000000000_000000000_000000000_000000000_111111111_111111111_111111111;
            var r1 = start & mask;
            var r2 = target & mask;
            return r1 == r2;
        }

        public static int MisplacedStickers(long start, long target, bool full)
        {
            // Пока просто возвращаем количество неправильно стоящих элементов
            // (по сути, это эвристика "количество элементов не на своем месте")
            if (!full)
            {
                const long mask = 0b0000000000_000000000_000000000_000000000_111111111_111111111_111111111;
                start &= mask;
                target &= mask;
            }
            var diff = start ^ target;
            var count = 0;
            while (diff != 0)
            {
                count++;
                diff &= diff - 1;
            }
            return count;
        }
    }
}
