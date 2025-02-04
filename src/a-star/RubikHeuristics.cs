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

        private static Dictionary<long, int> cornerPdb = null;
        private static Dictionary<long, int> edgePdb = null;

        public static void BuildPatternDatabase(SearchContext context)
        {
            cornerPdb = new();
            edgePdb = new();
            if (File.Exists(context.TargetName + ".corners"))
            {
                using var reader = new StreamReader(context.TargetName + ".corners");
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    var parts = line.Split(',');
                    if (parts.Length == 2 && long.TryParse(parts[0], out long key) && int.TryParse(parts[1], out int value))
                    {
                        cornerPdb[key] = value;
                    }
                }
                using var reader2 = new StreamReader(context.TargetName + ".edges");
                while ((line = reader2.ReadLine()) != null)
                {
                    var parts = line.Split(',');
                    if (parts.Length == 2 && long.TryParse(parts[0], out long key) && int.TryParse(parts[1], out int value))
                    {
                        edgePdb[key] = value;
                    }
                }

                return;
            }

            Queue<(long state, int depth)> queue = new Queue<(long, int)>();
            HashSet<long> visited = new HashSet<long>();

            long solvedCornerPattern = GetCornerPattern(context.Target);
            long solvedEdgePattern = GetEdgePattern(context.Target);

            cornerPdb[solvedCornerPattern] = 0;
            edgePdb[solvedEdgePattern] = 0;

            queue.Enqueue((context.Target, 0));
            visited.Add(context.Target);

            while (queue.Count > 0)
            {
                var (currentState, depth) = queue.Dequeue();

                var keys = Moves.Steps.Keys.Except(context.Ignore).ToList();
                if (context.RandomizeMovesOrder)
                {
                    keys = keys.OrderBy(x => Guid.NewGuid()).ToList();
                }

                foreach (var key in keys)
                {
                    var nextState = Moves.Steps[key](currentState);
                    long nextCornerPattern = GetCornerPattern(nextState);
                    long nextEdgePattern = GetEdgePattern(nextState);

                    bool updated = false;
                    if (!cornerPdb.ContainsKey(nextCornerPattern))
                    {
                        cornerPdb[nextCornerPattern] = depth + 1;
                        updated = true;
                    }
                    if (!edgePdb.ContainsKey(nextEdgePattern))
                    {
                        edgePdb[nextEdgePattern] = depth + 1;
                        updated = true;
                    }

                    if (updated)
                    {
                        queue.Enqueue((nextState, depth + 1));
                    }
                }
            }

            using var writer = new StreamWriter(context.TargetName + ".corners");
            foreach (var kvp in cornerPdb)
            {
                writer.WriteLine($"{kvp.Key},{kvp.Value}");
            }

            using var writer2 = new StreamWriter(context.TargetName + ".edges");
            foreach (var kvp in edgePdb)
            {
                writer2.WriteLine($"{kvp.Key},{kvp.Value}");
            }
        }

        private static long GetCornerPattern(long state)
        {
            return state & 0b0000000000_101000101_101000101_101000101_101000100_101000001_001000101L;
        }

        private static long GetEdgePattern(long state)
        {
            return state & 0b0000000000_010101010_010101010_010101010_010101010_010101010_010101010L;
        }

        public static int PDB(long state)
        {
            var targetCornerState = GetCornerPattern(state);
            var targetEdgeState = GetEdgePattern(state);
            return cornerPdb[targetCornerState] + edgePdb[targetEdgeState];
            //return Math.Max(cornerPdb[targetCornerState], edgePdb[targetEdgeState]);
        }
    }
}
