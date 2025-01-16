namespace CubeSolverConsoleApp;

internal class SolveContext
{
    public string Source { get; set; }
    public string Target { get; set; }
    public int SolutionsCount { get; set; }
    public bool CheckFull { get; set; }
    public long SourceState { get; set; }
    public long TargetState { get; set; }
    public int MaxDeep { get; set; }
    public int MinDeep { get; set; }
    public long _steps;

}
