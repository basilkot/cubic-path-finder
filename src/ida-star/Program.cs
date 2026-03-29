using IdaStarConsoleApp;

// --convert mode
if (args.Length >= 2 && args[0] == "--convert")
{
    MoveConverter.ConvertFile(args[1]);
    return;
}

// L_B_D_R_F_U
long initial = 0b0000000000_000000000_000000000_000000000_111111111_111111111_111111111;

long[] targets =
[
    0b0_111100101_111001101_101001111, // 0
];

// только передние 3 грани + wide (средние слои только через wide)
string[] allowedMoves =
[
    "U", "U'", "U2",
    "R", "R'", "R2",
    "F", "F'", "F2",
    "Uu", "Uu'", "Uu2",
    "Rr", "Rr'", "Rr2",
    "Ff", "Ff'", "Ff2",
    "U3", "U3'",
    "R3", "R3'",
    "F3", "F3'",
    "Uu3", "Uu3'",
    "Rr3", "Rr3'",
    "Ff3", "Ff3'",
];

foreach (var (target, i) in targets.Select((t, i) => (t, i)))
{
    Console.WriteLine($"=== Target {i} ===");
    Helpers.PrintOutline(initial);

    var solver = new IdaStarSolver(initial, target, allowedMoves: allowedMoves);
    var solutions = solver.Solve(minDepth: 7, maxDepth: 11);

    Console.WriteLine();
}
