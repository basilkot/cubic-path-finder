namespace AStarConsoleApp;

internal class SearchContext
{
    public string SourceName { get; set; }
    public string TargetName { get; set; }

    public long Source { get; set; }
    public long Target { get; set; }

    public bool FullOverlap { get; set; }
    public bool RandomizeMovesOrder { get; set; }

    public string[] Ignore { get; set; }
}
