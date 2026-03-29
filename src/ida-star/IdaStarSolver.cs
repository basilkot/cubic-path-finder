using System.Diagnostics;

namespace IdaStarConsoleApp;

internal class IdaStarSolver
{
    private const long PartialMask = 0b0_111111111_111111111_111111111;

    private readonly long _initial;
    private readonly long _target;
    private readonly (string name, Func<long, long> func, (int index, int delta)[] layers)[] _moves;
    private readonly List<List<string>> _solutions = new();
    private readonly Random _rng = new();
    private readonly string _logPath;
    private long _nodesVisited;

    // U=0, F=1, R=2, E=3 (между U и D), M=4 (между L и R), S=5 (между F и B)
    private static readonly Dictionary<string, (int index, int delta)[]> MoveLayerMap = new()
    {
        // базовые — двигают только грань
        ["U"]   = [(0, +1)], ["U'"]  = [(0, -1)], ["U2"]  = [(0, +2)],
        ["F"]   = [(1, +1)], ["F'"]  = [(1, -1)], ["F2"]  = [(1, +2)],
        ["R"]   = [(2, +1)], ["R'"]  = [(2, -1)], ["R2"]  = [(2, +2)],

        // wide — двигают грань + средний слой
        ["Uu"]  = [(0, +1), (3, +1)], ["Uu'"] = [(0, -1), (3, -1)], ["Uu2"] = [(0, +2), (3, +2)],
        ["Rr"]  = [(2, +1), (4, +1)], ["Rr'"] = [(2, -1), (4, -1)], ["Rr2"] = [(2, +2), (4, +2)],
        ["Ff"]  = [(1, +1), (5, +1)], ["Ff'"] = [(1, -1), (5, -1)], ["Ff2"] = [(1, +2), (5, +2)],

        // 270° — грань на ±3, wide ещё и средний слой на ±3
        ["U3"]   = [(0, +3)],                    ["U3'"]  = [(0, -3)],
        ["F3"]   = [(1, +3)],                    ["F3'"]  = [(1, -3)],
        ["R3"]   = [(2, +3)],                    ["R3'"]  = [(2, -3)],
        ["Uu3"]  = [(0, +3), (3, +3)],           ["Uu3'"] = [(0, -3), (3, -3)],
        ["Rr3"]  = [(2, +3), (4, +3)],           ["Rr3'"] = [(2, -3), (4, -3)],
        ["Ff3"]  = [(1, +3), (5, +3)],           ["Ff3'"] = [(1, -3), (5, -3)],

        // 180° в обратную сторону — состояние кубика то же, но позиция -2
        ["U2'"]  = [(0, -2)],
        ["R2'"]  = [(2, -2)],
        ["F2'"]  = [(1, -2)],
        ["Uu2'"] = [(0, -2), (3, -2)],
        ["Rr2'"] = [(2, -2), (4, -2)],
        ["Ff2'"] = [(1, -2), (5, -2)],
    };

    public IdaStarSolver(long initial, long target, string[]? allowedMoves = null, string logPath = "results.log")
    {
        _initial = initial;
        _target = target & PartialMask;
        _logPath = logPath;
        _moves = Moves.Steps
            .Where(kv => allowedMoves == null || allowedMoves.Contains(kv.Key))
            .Select(kv => (kv.Key, kv.Value, MoveLayerMap[kv.Key]))
            .ToArray();
    }

    public List<List<string>> Solve(int minDepth = 7, int maxDepth = 20, int maxSolutions = int.MaxValue)
    {
        var sw = Stopwatch.StartNew();
        // перемешиваем порядок ходов
        _rng.Shuffle(_moves);
        var moveOrder = string.Join(", ", _moves.Select(m => m.name));

        var threshold = Math.Max(Heuristic(_initial), minDepth);

        Console.WriteLine($"Initial heuristic: {Heuristic(_initial)}, starting from depth: {threshold}");
        Console.WriteLine($"Moves available: {_moves.Length}");
        Console.WriteLine();

        File.AppendAllText(_logPath,
            $"=== {DateTime.Now:yyyy-MM-dd HH:mm:ss} ===\n" +
            $"Initial: {Convert.ToString(_initial, 2).PadLeft(54, '0')}\n" +
            $"Target:  {Convert.ToString(_target, 2).PadLeft(27, '0')}\n" +
            $"Move order: {moveOrder}\n");

        var layerPositions = new int[6]; // U, F, R, E, M, S

        while (threshold <= maxDepth && _solutions.Count < maxSolutions)
        {
            _nodesVisited = 0;
            Console.WriteLine($"--- Threshold: {threshold} ---");

            var path = new List<string>();
            var nextThreshold = Search(_initial, 0, threshold, path, layerPositions, maxSolutions);

            Console.WriteLine($"  Nodes visited: {_nodesVisited:N0}, solutions so far: {_solutions.Count}, elapsed: {sw.Elapsed}");

            if (nextThreshold == int.MaxValue)
                break;

            threshold = nextThreshold;
        }

        Console.WriteLine();
        Console.WriteLine($"Total solutions found: {_solutions.Count}, elapsed: {sw.Elapsed}");

        File.AppendAllText(_logPath,
            $"Total: {_solutions.Count}, elapsed: {sw.Elapsed}\n\n");

        return _solutions;
    }

    private int Search(long state, int g, int threshold, List<string> path, int[] layerPositions, int maxSolutions)
    {
        _nodesVisited++;

        var h = Heuristic(state);
        var f = g + h;

        if (f > threshold)
            return f;

        if (h == 0)
        {
            _solutions.Add(new List<string>(path));
            var solutionStr = string.Join(" ", path);
            Console.WriteLine($"  #{_solutions.Count}: {solutionStr} (nodes: {_nodesVisited:N0})");
            File.AppendAllText(_logPath, $"  #{_solutions.Count}: {solutionStr} (nodes: {_nodesVisited:N0})\n");
            return f;
        }

        var min = int.MaxValue;

        foreach (var (name, func, layers) in _moves)
        {
            if (_solutions.Count >= maxSolutions)
                return min;

            // не делаем два хода одной группы подряд (U+U можно объединить, но U+Uu — нельзя, это средний слой)
            if (path.Count > 0 && SameGroup(path[^1], name))
                continue;

            // проверяем лимит вращения: все затронутые слои должны остаться в [-2, +2]
            var valid = true;
            foreach (var (index, delta) in layers)
            {
                var newPos = layerPositions[index] + delta;
                if (newPos < -2 || newPos > 2)
                {
                    valid = false;
                    break;
                }
            }
            if (!valid)
                continue;

            // применяем
            foreach (var (index, delta) in layers)
                layerPositions[index] += delta;

            var nextState = func(state);
            path.Add(name);

            var t = Search(nextState, g + 1, threshold, path, layerPositions, maxSolutions);

            path.RemoveAt(path.Count - 1);

            // откатываем
            foreach (var (index, delta) in layers)
                layerPositions[index] -= delta;

            if (t < min)
                min = t;
        }

        return min;
    }

    private int Heuristic(long state)
    {
        var diff = (state & PartialMask) ^ _target;
        var count = 0;
        while (diff != 0)
        {
            count++;
            diff &= diff - 1;
        }
        return (count + 19) / 20;
    }

    private static string MoveGroup(string move)
    {
        // "U" -> "U", "U'" -> "U", "U2" -> "U", "U3" -> "U", "U3'" -> "U"
        // "Uu" -> "Uu", "Uu'" -> "Uu", "Uu2" -> "Uu", "Uu3" -> "Uu", "Uu3'" -> "Uu"
        int len = 1;
        if (move.Length > 1 && char.IsLower(move[1]))
            len = 2; // wide move: Uu, Rr, Ff
        return move[..len];
    }

    private static bool SameGroup(string move1, string move2)
    {
        return MoveGroup(move1) == MoveGroup(move2);
    }
}
