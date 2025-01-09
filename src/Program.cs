using CubeSolverConsoleApp;

// L_B_D_R_F_U
var input = 0b0000000000_000000000_000000000_000000000_111111111_111111111_111111111;
var target1 = 0b0_110110110_111111111_011111111;
var target0 = 0b0_111100101_111001101_101001111;


//var result = SolverDeep.ValidatePath(input, ["M2", "U", "R", "U", "R", "E", "X'", "E'", "L", "R'", "U'"]);
//Console.WriteLine(target0);
//Console.WriteLine(result);
//Console.WriteLine(Helpers.Compare(target0, result));

// var result = SolverWide.SolveCube(input, target0);
var result = SolverDeep.SolveCube(input, target0);

if (result.Count > 0)
{
    Console.WriteLine("Решение найдено:");
    foreach (var pair in result)
    {
        Helpers.Print(pair.state);
        Console.WriteLine(string.Join(" ", pair.path));
        Console.WriteLine("---");
    }
}
else
{
    Console.WriteLine("Решение не найдено");
}



//Console.WriteLine(Convert.ToString(swappedValue, 2).PadLeft(64, '0'));
//Console.WriteLine(Convert.ToString(input, 2).PadLeft(64, '0'));
//Console.WriteLine(Convert.ToString(result, 2).PadLeft(64, '0'));

//var result = (long)1 << 38;

//Console.WriteLine(result);
//Console.WriteLine(Convert.ToString(result, 2).PadLeft(64, '0'));

return;

