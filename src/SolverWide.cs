using System.Collections.Concurrent;
using System.Diagnostics;

namespace CubeSolverConsoleApp;

internal class SolverWide
{
    private static (long state, string move)[] GetNextStates(long state)
    {
        var result = Moves.Steps.Keys.Select(x => (Moves.Steps[x](state), x));
        return result.ToArray();
    }

    private static ConcurrentBag<(long state, string[] path)> results = new();
    //private static ConcurrentDictionary<long, bool> visited = new();
    private static long checkNumbers = 0;
    private static Stopwatch ss;

    public static List<(long state, string[] path)> SolveCube(long initialState, long targetState)
    {
        ss = Stopwatch.StartNew();
        if (Helpers.Compare(initialState, targetState))
        {
            return results.ToList();
        }

        var tasks = Moves.Steps.Keys.Select(step => SolveFor(step, initialState, targetState)).ToArray();

        Task.WaitAll(tasks);

        return results.ToList();
    }

    private static Task SolveFor(string firstStep, long initialState, long targetState)
    {
        return Task.Run(() =>
        {
            Queue<(long state, string[] path)> queue = new();
            var state = Moves.Steps[firstStep](initialState);
            queue.Enqueue((state, [firstStep]));

            while (queue.Any())
            {
                var current = queue.Dequeue();

                var nextStates = GetNextStates(current.state);

                foreach (var next in nextStates)
                {
                    var nextState = next.state;
                    var moveName = next.move;

                    //if (!visited.ContainsKey(nextState))
                    {
                        var newPath = current.path.Concat([moveName]).ToArray();
                        var v = Interlocked.Increment(ref checkNumbers);
                        if (Helpers.Compare(nextState, targetState))
                        {
                            Helpers.AddToFile(newPath, nextState);
                            results.Add((nextState, newPath));
                        }
                        else if (newPath.Count() < 20)
                        {
                            if (v % 1_000_000 == 0)
                            {
                                Console.WriteLine(ss.ElapsedMilliseconds);
                                Console.WriteLine(v / 1_000_000);
                                Console.WriteLine(string.Join(" ", newPath));
                            }

                            //visited.TryAdd(nextState, true);
                            queue.Enqueue((nextState, newPath));
                        }
                    }
                }
            }

        });
    }

}
