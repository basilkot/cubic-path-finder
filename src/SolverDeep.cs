using System.Collections.Concurrent;
using System.Diagnostics;

namespace CubeSolverConsoleApp;

internal class SolverDeep
{
    public static bool Cancelled = false;
    public static long ValidatePath(long state, string[] path)
    {
        var currentState = state;
        foreach (var step in path)
        {
            Console.WriteLine(step);
            Console.WriteLine(currentState.AsString());
            currentState = Moves.Steps[step](currentState);
            Helpers.PrintOutline(currentState);
            Console.WriteLine(currentState.AsString());
            Console.WriteLine("---------");
            Console.ReadKey();
        }

        return currentState;
    }

    private static Stopwatch ss;

    public static ConcurrentBag<(string[] path, long state)> solutions = new();

    private static Dictionary<string, List<DeepSearchState>> SearchStates { get; } = new();

    private static CancellationTokenSource Cts = null;

    public static void PrepareSearchState(string? filename)
    {
        SearchStates.Clear();
        if (filename != null && File.Exists(filename))
        {
            var result = Helpers.ReadStates(filename);
            foreach (var (key, value) in result)
            {
                SearchStates.Add(key, value);
            }
        }
    }

    public static List<(string[] path, long state)> SolveCube(SolveContext context)
    {
        Cts = new CancellationTokenSource();
        ss = Stopwatch.StartNew();
        solutions.Clear();

        var tasks = new[] { KeyListenerTask($"ss-{context.Source}-{context.Target}.json") }.Union(
            Moves.Steps.Keys.Select(step =>
                StartFrom(context, step, new Stack<string>())
            )
        ).ToArray();
        Console.WriteLine($"Run {tasks.Length} tasks");
        Console.WriteLine($"Solve {context.Source} => {context.Target}");

        Task.WaitAll(tasks);
        Console.WriteLine($"Checked all solutions for {context.Source} => {context.Target}");

        return solutions.ToList();
    }

    private static Task KeyListenerTask(string filename)
    {
        return Task.Run(() =>
        {
            while (!Cts.IsCancellationRequested)
            {
                var key = Console.ReadKey(true);
                switch (key.Key)
                {
                    case ConsoleKey.S:
                        Helpers.SaveState(SearchStates, filename);
                        break;
                    case ConsoleKey.Q:
                        if ((key.Modifiers & ConsoleModifiers.Control) != 0)
                        {
                            Cancelled = true;
                            Helpers.SaveState(SearchStates, filename);
                            Cts.Cancel();
                            return;
                        }
                        break;

                }
            }
        });
    }

    static Task StartFrom(SolveContext context, string step, Stack<string> path)
    {
        return Task.Run(() =>
        {
            if (!SearchStates.ContainsKey(step))
            {
                SearchStates.Add(step, new List<DeepSearchState>());
            }

            path.Push(step);
            var startState = Moves.Steps[step](context.SourceState);
            var counter = 0L;
            SolveDeep(startState, path, context, ref counter, step);
            Console.WriteLine($"task {step} finished ({counter} paths checked)");
        });
    }

    static bool SolveDeep(long state, Stack<string> path, SolveContext context, ref long counter, string name)
    {
        if (Cts.IsCancellationRequested)
        {
            return false;
        }

        if (Helpers.Compare(state, context.TargetState, context.CheckFull))
        {
            if (path.Count < context.MinDeep)
            {
                WriteResult("short", path, state, context);
            }
            else
            {
                var pathArray = WriteResult(null, path, state, context);
                solutions.Add((pathArray, state));
                Console.WriteLine("!!!!!!!!!!!!!!!!!!!!!!!!");
                Console.WriteLine(string.Join(' ', pathArray));
                Helpers.PrintOutline(state);
                return true;
            }
        }
        counter++;

        if (path.Count > context.MaxDeep)
        {
            return false;
        }

        if (counter % 1_000_000 == 0)
        {
            var value = string.Join(' ', [
                $"╒{name}:".PadRight(5),
                (ss.ElapsedMilliseconds / 1000).ToString().PadRight(10),
                (counter / 1_000_000).ToString().PadRight(5),
                context.Source, "=>", context.Target
            ]);
            var pathValue = "╘► " + string.Join(" ", path.Reverse().ToArray());
            var line = string.Join(Environment.NewLine, [value, pathValue]);
            Console.WriteLine(line);
        }

        var searchStates = SearchStates[name];
        var searchState = searchStates.FirstOrDefault(x => x.Level == path.Count);
        if (searchState == null)
        {
            searchState = new DeepSearchState
            {
                Level = path.Count,
                LastMove = path.Peek()
            };
            searchStates.Add(searchState);
        }
        else if (searchState.Steps != null)
        {
            var nextStepState = searchStates.FirstOrDefault(x => x.Level == path.Count + 1);
            if (nextStepState != null)
            {
                searchState.Steps = searchState.Steps.SkipWhile(x => x != nextStepState.LastMove).ToArray();
            }
        }
        if (searchState.Steps == null)
        {
            searchState.Steps = Moves.Steps.Keys.OrderBy(x => Guid.NewGuid()).ToArray();
        }

        foreach (var step in searchState.Steps)
        {
            if (path.Count == 0 || step[0] != path.Peek()[0])
            {
                var newState = Moves.Steps[step](state);
                path.Push(step);
                var result = SolveDeep(newState, path, context, ref counter, name);
                if (Cts.IsCancellationRequested)
                {
                    return false;
                }
                if (result)
                {
                    if (context.SolutionsCount == solutions.Count)
                    {
                        Cts.Cancel();
                        return true;
                    }
                }
                path.Pop();
            }
        }

        searchStates.Remove(searchState);

        return false;
    }

    static string[] WriteResult(string? msg, Stack<string> path, long state, SolveContext context)
    {
        var pathArray = path.Reverse().ToArray();
        var nl = Environment.NewLine;
        msg = msg == null ? null : $"{msg}{nl}";
        var message = $"{msg}{msg}{context.Source} => {context.Target}{nl}{context.SourceState.AsString()}";
        Helpers.AddToFile(message, pathArray, state);
        // solutions.Add((pathArray, state));
        return pathArray;
    }
}


internal class DeepSearchState
{
    public int Level { get; set; } // current level
    public string[] Steps { get; set; } // available moves in order
    public string LastMove { get; set; }
}
