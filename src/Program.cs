// L_B_D_R_F_U

using CubeSolverConsoleApp;

var input = 0b0000000000_000000000_000000000_000000000_111111111_111111111_111111111;
var full1 = 0b0000000000_000001101_000000000_100000000_110110110_111111111_011111111;

var target0 = 0b0_111100101_111001101_101001111;
var target1 = 0b0_110110110_111111111_011111111;
var target2 = 0b0_111111001_111001101_001111001;
var target3 = 0b0_111110110_111001111_011011000;
var target4 = 0b0_101100101_111001101_111111111;
var target5 = 0b0_111010010_111001101_111011011;
var target6 = 0b0_111100101_111001101_101001101;
var target7 = 0b0_101101101_111111011_011011111;
var target8 = 0b0_100101100_001101001_101001111;
var target9 = 0b0_101100101_111001101_101001111;


// check arguments
//  1) validate path
//  2) solve path between from and to
//  3) max path
//  4) max steps
//  5) solve everything
//      it means we start from initial, then goto each digit, save it, search path between shortest digits

Helpers.PrintOutline(full1);

//var result = SolverDeep.ValidatePath(input, ["D", "U", "R", "U", "R", "L", "Z2", "B", "R'", "M'", "X'"]);
//Console.WriteLine(target4);
//Console.WriteLine(result);
//Console.WriteLine(target4 & 0b0000000000_000000000_000000000_000000000_111111111_111111111_111111111);
//Console.WriteLine(result & 0b0000000000_000000000_000000000_000000000_111111111_111111111_111111111);
//Console.WriteLine(Helpers.Compare(target4, result));

// var result = SolverWide.SolveCube(input, target0);



//var result = SolverDeep.SolveCube(full1, target4);

//if (result.Count > 0)
//{
//    Console.WriteLine("Решение найдено:");
//    foreach (var pair in result)
//    {
//        Helpers.Print(pair.state);
//        Console.WriteLine(string.Join(" ", pair.path));
//        Console.WriteLine("---");
//    }
//}
//else
//{
//    Console.WriteLine("Решение не найдено");
//}
