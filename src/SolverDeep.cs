using System.Collections.Concurrent;
using System.Diagnostics;

namespace CubeSolverConsoleApp;

internal class SolverDeep
{
    private static long _steps = 0;

    public static long ValidatePath(long state, string[] path)
    {
        var currentState = state;
        foreach (var step in path)
        {
            Console.WriteLine(step);
            Console.WriteLine(currentState);
            currentState = Moves.Steps[step](currentState);
            Helpers.Print(currentState);
            Console.WriteLine(currentState);
            Console.WriteLine("---------");
            Console.ReadKey();
        }

        return currentState;
    }

    private static Stopwatch ss;

    public static ConcurrentBag<(string[] path, long state)> solutions = new();

    public static List<(string[] path, long state)> SolveCube(long initialState, long targetState)
    {
        ss = Stopwatch.StartNew();

        var tasks = Moves.Steps.Keys.Select(step => StartFrom(initialState, targetState, step, new Stack<string>())).ToArray();

        Task.WaitAll(tasks);

        return solutions.ToList();
    }

    static Task StartFrom(long state, long targetState, string step, Stack<string> path)
    {
        return Task.Run(() =>
        {
            path.Push(step);
            var newState = Moves.Steps[step](state);
            SolveDeep(newState, targetState, path);
        });
    }

    static bool SolveDeep(long state, long targetState, Stack<string> path)
    {
        if (Helpers.Compare(state, targetState))
        {
            return true;
        }

        if (path.Count > 6)
        {
            return false;
        }

        var v = Interlocked.Increment(ref _steps);
        if (v % 1_000_000 == 0)
        {
            Console.WriteLine(ss.ElapsedMilliseconds);
            Console.WriteLine(v / 1_000_000);
            Console.WriteLine(string.Join(" ", path.Reverse().ToArray()));
        }


        foreach (var step in Moves.Steps.Keys)
        {
            if (path.Count == 0 || step[0] != path.Peek()[0])
            {
                var newState = Moves.Steps[step](state);
                path.Push(step);
                var result = SolveDeep(newState, targetState, path);
                if (result)
                {
                    var pathArray = path.Reverse().ToArray();
                    Helpers.AddToFile(pathArray, newState);
                    solutions.Add((pathArray, newState));
                    Console.WriteLine("!!!!!!!!!!!!!!!!!!!!!!!!");
                    Console.WriteLine(string.Join(' ', pathArray));
                }
                path.Pop();
            }
        }

        return false;


    }
}
