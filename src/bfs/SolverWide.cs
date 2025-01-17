//namespace CubeSolverConsoleApp;

//internal class SolverWide
//{
//    public static string[] BfsFindGoal(SolveContext context)
//    {
//        var state = context.SourceState;
//        if (Helpers.Compare(state, context.TargetState, context.CheckFull))
//        {
//            return [];
//        }

//        // Посещённые состояния
//        var visited = new HashSet<long> { state };
//        var path = new Queue<string>();

//        // Очередь для слоёв BFS
//        var queue = new Queue<long>();
//        queue.Enqueue(state);

//        // Пока очередь не опустела
//        while (queue.Count > 0)
//        {
//            long current = queue.Dequeue();
//            foreach (var neighbor in Moves.GetNextStates(current))
//            {
//                // Если ещё не посещали
//                if (!visited.Contains(neighbor.state))
//                {
//                    path.Enqueue(neighbor.step);
//                    // Проверяем, не цель ли это
//                    if (Helpers.Compare(context.TargetState, neighbor.state, context.CheckFull))
//                    {
//                        return path.Reverse().ToArray();
//                    }

//                    visited.Add(neighbor.state);
//                    queue.Enqueue(neighbor.state);
//                }
//            }
//        }

//        // Если выходим из цикла — цель не достигнута
//        return false;
//    }
//}
