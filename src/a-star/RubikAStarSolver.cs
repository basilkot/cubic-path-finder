namespace AStarConsoleApp;

internal class RubikAStarSolver
{
    /// <summary>
    /// Пример A*-поиска для сборки кубика.
    /// Возвращает список ходов (пока только "U") от start до goal (или null, если не найден).
    /// </summary>
    public static List<string> AStarSearch(SearchContext context)
    {
        var start = context.Source;
        var target = context.Target;
        var full = context.FullOverlap;

        // Если уже решение
        if (RubikHeuristics.IsGoal(start, target, full))
            return new List<string>();

        // openSet = множество вершин, которые нужно рассмотреть
        var openSet = new AStarPriorityQueue();
        openSet.Enqueue(start, RubikHeuristics.MisplacedStickers(start, target, full));

        // cameFrom[state] = откуда мы пришли в state
        Dictionary<long, long> cameFrom = new();
        // moveFrom[state] = каким ходом мы пришли в state (чтобы восстановить алгоритм)
        Dictionary<long, string> moveFrom = new();

        // gScore[state] = стоимость пути от start до state
        Dictionary<long, int> gScore = new()
        {
            [start] = 0
        };

        // fScore[state] = gScore[state] + h(state)
        Dictionary<long, int> fScore = new()
        {
            [start] = RubikHeuristics.MisplacedStickers(start, target, full)
        };

        // Множество для проверки, есть ли уже в очереди (или было)
        HashSet<long> inOpenSet = [start];

        var startTime = DateTime.Now;
        while (openSet.Count > 0)
        {
            if ((DateTime.Now - startTime).TotalMinutes > 4)
            {
                return null; // Exit the loop and return null if time limit exceeded
            }

            // Извлекаем вершину с минимальным f
            var current = openSet.Dequeue();
            inOpenSet.Remove(current);

            // Проверяем, не цель ли она
            if (RubikHeuristics.IsGoal(current, target, full))
            {
                // Восстанавливаем путь
                return ReconstructPath(cameFrom, moveFrom, current);
            }

            int currentG = gScore[current];

            // Генерируем соседей (применение ходов)
            List<(long state, string move)> neighbors = new();
            var keys = Moves.Steps.Keys.Except(context.Ignore);
            if (context.RandomizeMovesOrder)
            {
                keys = keys.OrderBy(x => Guid.NewGuid());
            }
            foreach (var key in keys)
            {
                neighbors.Add((Moves.Steps[key](current), key));
            }

            foreach (var (neighbor, move) in neighbors)
            {
                int tentativeG = currentG + 1; // считаем цену хода = 1
                // если мы не встречали neighbor или нашли более короткий путь
                if (!gScore.ContainsKey(neighbor) || tentativeG < gScore[neighbor])
                {
                    gScore[neighbor] = tentativeG;
                    var h = RubikHeuristics.MisplacedStickers(neighbor, target, full);
                    fScore[neighbor] = tentativeG + h;

                    cameFrom[neighbor] = current;
                    moveFrom[neighbor] = move;

                    // Если neighbor не в очереди, добавляем
                    if (!inOpenSet.Contains(neighbor))
                    {
                        openSet.Enqueue(neighbor, fScore[neighbor]);
                        inOpenSet.Add(neighbor);
                    }
                }
            }
        }

        // Если очередь опустела, значит пути нет
        return null;
    }

    /// <summary>
    /// Восстановить путь ходов (от стартовой конфигурации до current)
    /// </summary>
    private static List<string> ReconstructPath(
        Dictionary<long, long> cameFrom,
        Dictionary<long, string> moveFrom,
        long current)
    {
        List<string> moves = new();
        while (cameFrom.ContainsKey(current))
        {
            var parent = cameFrom[current];
            var move = moveFrom[current];
            moves.Add(move);
            current = parent;
        }
        moves.Reverse();
        return moves;
    }
}