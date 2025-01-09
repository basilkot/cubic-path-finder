namespace CubeSolverConsoleApp;

internal class Moves
{
    public static Dictionary<string, Func<long, long>> Steps = new()
    {
        {"U", MoveU },
        {"U'", MoveUi},
        {"U2", MoveU2},

        {"R", MoveR },
        {"R'", MoveRi},
        {"R2", MoveR2},

        {"F", MoveF },
        {"F'", MoveFi},
        {"F2", MoveF2},

        {"D", MoveD },
        {"D'", MoveDi},
        {"D2", MoveD2},

        {"L", MoveL },
        {"L'", MoveLi},
        {"L2", MoveL2},

        {"B", MoveB },
        {"B'", MoveBi},
        {"B2", MoveB2},

        {"M", MoveM },
        {"M'", MoveMi},
        {"M2", MoveM2},

        {"E", MoveE },
        {"E'", MoveEi},
        {"E2", MoveE2},

        {"S", MoveS },
        {"S'", MoveSi},
        {"S2", MoveS2},

        {"X", MoveX },
        {"X'", MoveXi},
        {"X2", MoveX2},

        {"Y", MoveY },
        {"Y'", MoveYi},
        {"Y2", MoveY2},

        {"Z", MoveZ },
        {"Z'", MoveZi},
        {"Z2", MoveZ2},
    };

    public static long MoveU(long state)
    {
        var result = Helpers.SwapBitGroups(state,
            [0, 1, 2, 5, 8, 7, 6, 3,
                9, 10, 11,
                18, 19, 20,
                38, 37, 36,
                47, 46, 45],
            [6, 3, 0, 1, 2, 5, 8, 7,
                18, 19, 20,
                38, 37, 36,
                47, 46, 45,
                9, 10, 11]);
        return result;
    }

    public static long MoveU2(long state)
    {
        var result = Helpers.SwapBitGroups(state,
            [0, 1, 2, 5, 8, 7, 6, 3,
                9, 10, 11,
                18, 19, 20,
                38, 37, 36,
                47, 46, 45],
            [8, 7, 6, 3, 0, 1, 2, 5,
                38, 37, 36,
                47, 46, 45,
                9, 10, 11,
                18, 19, 20]);
        return result;
    }

    public static long MoveUi(long state)
    {
        var result = Helpers.SwapBitGroups(state,
            [0, 1, 2, 5, 8, 7, 6, 3,
                9, 10, 11,
                18, 19, 20,
                38, 37, 36,
                47, 46, 45],
            [2, 5, 8, 7, 6, 3, 0, 1,
                47, 46, 45,
                9, 10, 11,
                18, 19, 20,
                38, 37, 36]);
        return result;
    }

    public static long MoveR(long state)
    {
        var result = Helpers.SwapBitGroups(state,
            [18, 19, 20, 23, 26, 25, 24, 21,
                2, 5, 8,
                44, 41, 38,
                35, 32, 29,
                11, 14, 17],
            [24, 21, 18, 19, 20, 23, 26, 25,
                11, 14, 17,
                2, 5, 8,
                44, 41, 38,
                35, 32, 29]);
        return result;
    }

    public static long MoveR2(long state)
    {
        var result = Helpers.SwapBitGroups(state,
            [18, 19, 20, 23, 26, 25, 24, 21,
                2, 5, 8,
                44, 41, 38,
                35, 32, 29,
                11, 14, 17],
            [26, 25, 24, 21, 18, 19, 20, 23,
                35, 32, 29,
                11, 14, 17,
                2, 5, 8,
                44, 41, 38]);
        return result;
    }

    public static long MoveRi(long state)
    {
        var result = Helpers.SwapBitGroups(state,
            [18, 19, 20, 23, 26, 25, 24, 21,
                2, 5, 8,
                44, 41, 38,
                35, 32, 29,
                11, 14, 17],
            [20, 23, 26, 25, 24, 21, 18, 19,
                44, 41, 38,
                35, 32, 29,
                11, 14, 17,
                2, 5, 8]);
        return result;
    }

    public static long MoveF(long state)
    {
        var result = Helpers.SwapBitGroups(state,
            [9, 10, 11, 14, 17, 16, 15, 12,
                6, 7, 8,
                18, 21, 24,
                35, 34, 33,
                51, 48, 45],
            [15, 12, 9, 10, 11, 14, 17, 16,
                51, 48, 45,
                6, 7, 8,
                18, 21, 24,
                35, 34, 33]);
        return result;
    }

    public static long MoveF2(long state)
    {
        var result = Helpers.SwapBitGroups(state,
            [9, 10, 11, 14, 17, 16, 15, 12,
                6, 7, 8,
                18, 21, 24,
                35, 34, 33,
                51, 48, 45],
            [17, 16, 15, 12, 9, 10, 11, 14,
                35, 34, 33,
                51, 48, 45,
                6, 7, 8,
                18, 21, 24]);
        return result;
    }

    public static long MoveFi(long state)
    {
        var result = Helpers.SwapBitGroups(state,
            [9, 10, 11, 14, 17, 16, 15, 12,
                6, 7, 8,
                18, 21, 24,
                35, 34, 33,
                51, 48, 45],
            [11, 14, 17, 16, 15, 12, 9, 10,
                18, 21, 24,
                35, 34, 33,
                51, 48, 45,
                6, 7, 8]);
        return result;
    }

    public static long MoveD(long state)
    {
        var result = Helpers.SwapBitGroups(state,
            [27,28,29,32,35,34,33,30,
                15,16,17,
                53,52,51,
                44,43,42,
                24,25,26],
            [33,30,27,28,29,32,35,34,
                24,25,26,
                15,16,17,
                53,52,51,
                44,43,42]);
        return result;
    }

    public static long MoveD2(long state)
    {
        var result = Helpers.SwapBitGroups(state,
            [27,28,29,32,35,34,33,30,
                15,16,17,
                53,52,51,
                44,43,42,
                24,25,26],
            [35,34,33,30,27,28,29,32,
                44,43,42,
                24,25,26,
                15,16,17,
                53,52,51]);
        return result;
    }

    public static long MoveDi(long state)
    {
        var result = Helpers.SwapBitGroups(state,
            [27,28,29,32,35,34,33,30,
                15,16,17,
                53,52,51,
                44,43,42,
                24,25,26],
            [29,32,35,34,33,30,27,28,
                53,52,51,
                44,43,42,
                24,25,26,
                15,16,17]);
        return result;
    }

    public static long MoveL(long state)
    {
        var result = Helpers.SwapBitGroups(state,
            [45,46,47,50,53,52,51,48,
                0,3,6,
                42,39,36,
                33,30,27,
                9,12,15],
            [51,48,45,46,47,50,53,52,
                9,12,15,
                0,3,6,
                42,39,36,
                33,30,27]);
        return result;
    }

    public static long MoveL2(long state)
    {
        var result = Helpers.SwapBitGroups(state,
            [45,46,47,50,53,52,51,48,
                0,3,6,
                42,39,36,
                33,30,27,
                9,12,15],
            [53,52,51,48,45,46,47,50,
                33,30,27,
                9,12,15,
                0,3,6,
                42,39,36]);
        return result;
    }

    public static long MoveLi(long state)
    {
        var result = Helpers.SwapBitGroups(state,
            [45,46,47,50,53,52,51,48,
                0,3,6,
                42,39,36,
                33,30,27,
                9,12,15],
            [47,50,53,52,51,48,45,46,
                42,39,36,
                33,30,27,
                9,12,15,
                0,3,6]);
        return result;
    }

    public static long MoveB(long state)
    {
        var result = Helpers.SwapBitGroups(state,
            [36,37,38,41,44,43,42,39,
                0,1,2,
                20,23,26,
                29,28,27,
                53,50,47],
            [42,39,36,37,38,41,44,43,
                53,50,47,
                0,1,2,
                20,23,26,
                29,28,27]);
        return result;
    }
    public static long MoveB2(long state)
    {
        var result = Helpers.SwapBitGroups(state,
            [36,37,38,41,44,43,42,39,
                0,1,2,
                20,23,26,
                29,28,27,
                53,50,47],
            [44,43,42,39,36,37,38,41,
                29,28,27,
                53,50,47,
                0,1,2,
                20,23,26]);
        return result;
    }
    public static long MoveBi(long state)
    {
        var result = Helpers.SwapBitGroups(state,
            [36,37,38,41,44,43,42,39,
                0,1,2,
                20,23,26,
                29,28,27,
                53,50,47],
            [38,41,44,43,42,39,36,37,
                20,23,26,
                29,28,27,
                53,50,47,
                0,1,2]);
        return result;
    }

    public static long MoveM(long state)
    {
        var result = Helpers.SwapBitGroups(state,
            [1,4,7,
             43,40,37,
             34,31,28,
             10,13,16],
            [10,13,16,
             1,4,7,
             43,40,37,
             34,31,28]);
        return result;
    }

    public static long MoveM2(long state)
    {
        var result = Helpers.SwapBitGroups(state,
            [1,4,7,
                43,40,37,
                34,31,28,
                10,13,16],
            [34,31,28,
                10,13,16,
                1,4,7,
                43,40,37]);
        return result;
    }

    public static long MoveMi(long state)
    {
        var result = Helpers.SwapBitGroups(state,
            [1,4,7,
                43,40,37,
                34,31,28,
                10,13,16],
            [43,40,37,
                34,31,28,
                10,13,16,
                1,4,7]);
        return result;
    }

    public static long MoveE(long state)
    {
        var result = Helpers.SwapBitGroups(state,
            [12,13,14,
                50,49,48,
                41,40,39,
                21,22,23],
            [21,22,23,
                12,13,14,
                50,49,48,
                41,40,39]);
        return result;
    }

    public static long MoveE2(long state)
    {
        var result = Helpers.SwapBitGroups(state,
            [12,13,14,
                50,49,48,
                41,40,39,
                21,22,23],
            [41,40,39,
                21,22,23,
                12,13,14,
                50,49,48]);
        return result;
    }

    public static long MoveEi(long state)
    {
        var result = Helpers.SwapBitGroups(state,
            [12,13,14,
                50,49,48,
                41,40,39,
                21,22,23],
            [50,49,48
                ,41,40,39,
                21,22,23,
                12,13,14]);
        return result;
    }

    public static long MoveS(long state)
    {
        var result = Helpers.SwapBitGroups(state,
            [3,4,5,
                19,22,25,
                32,31,30,
                52,49,46],
            [52,49,46,
                3,4,5,
                19,22,25,
                32,31,30]);
        return result;
    }

    public static long MoveS2(long state)
    {
        var result = Helpers.SwapBitGroups(state,
            [3,4,5,
                19,22,25,
                32,31,30,
                52,49,46],
            [32,31,30,
                52,49,46,
                3,4,5,
                19,22,25]);
        return result;
    }

    public static long MoveSi(long state)
    {
        var result = Helpers.SwapBitGroups(state,
            [3,4,5,
                19,22,25,
                32,31,30,
                52,49,46],
            [19,22,25,
                32,31,30,
                52,49,46,
                3,4,5]);
        return result;
    }

    public static long MoveX(long state)
    {
        var result = Helpers.SwapBitGroups(state,
            [0, 1, 2, 3, 4, 5, 6, 7, 8,
                9, 10, 11, 12, 13, 14, 15, 16, 17,
                18, 19, 20, 21, 23, 24, 25, 26,
                27, 28, 29, 30, 31, 32, 33, 34, 35,
                36, 37, 38, 39, 40, 41, 42, 43, 44,
                45, 46, 47, 48, 50, 51, 52, 53],
            [9, 10, 11, 12, 13, 14, 15, 16, 17,
                33,34,35,30,31,32,27,28,29,
                24,21,18,25,19,26,23,20,
                36, 37, 38, 39, 40, 41, 42, 43, 44,
                6,7,8,3,4,5,0,1,2,
                51,48,47,52,46,53,50,47]
        );
        return result;
    }

    public static long MoveX2(long state)
    {
        var result = Helpers.SwapBitGroups(state,
            [0, 1, 2, 3, 4, 5, 6, 7, 8,
                9, 10, 11, 12, 13, 14, 15, 16, 17,
                18, 19, 20, 21, 23, 24, 25, 26,
                27, 28, 29, 30, 31, 32, 33, 34, 35,
                36, 37, 38, 39, 40, 41, 42, 43, 44,
                45, 46, 47, 48, 50, 51, 52, 53],
            [33,34,35,30,31,32,27,28,29,
                42,43,44,39,40,41,36,37,38,
                26,25,24,23,21,20,19,18,
                6,7,8,3,4,5,0,1,2,
                15,16,17,12,13,14,9,10,11,
                53,52,51,50,48,47,46,45]
        );
        return result;
    }

    public static long MoveXi(long state)
    {
        var result = Helpers.SwapBitGroups(state,
            [0, 1, 2, 3, 4, 5, 6, 7, 8,
                9, 10, 11, 12, 13, 14, 15, 16, 17,
                18, 19, 20, 21, 23, 24, 25, 26,
                27, 28, 29, 30, 31, 32, 33, 34, 35,
                36, 37, 38, 39, 40, 41, 42, 43, 44,
                45, 46, 47, 48, 50, 51, 52, 53],
            [42,43,44,39,40,41,36,37,38,
                0,1,2,3,4,5,6,7,8,
                20,23,26,19,25,18,21,24,
                15,16,17,12,13,14,9,10,11,
                27,28,29,30,31,32,33,34,35,
                47,50,53,46,52,45,48,51]
        );
        return result;
    }

    public static long MoveY(long state)
    {
        var result = Helpers.SwapBitGroups(state,
            [0, 1, 2, 3, 5, 6, 7, 8,
                9, 10, 11, 12, 13, 14, 15, 16, 17,
                18, 19, 20, 21, 22, 23, 24, 25, 26,
                27, 28, 29, 30, 32, 33, 34, 35,
                36, 37, 38, 39, 40, 41, 42, 43, 44,
                45, 46, 47, 48, 49, 50, 51, 52, 53],
            [6,3,0,7,1,8,5,2,
                18, 19, 20, 21, 22, 23, 24, 25, 26,
                38,37,36,41,40,39,44,43,42,
                33,30,27,34,28,35,32,29,
                45, 46, 47, 48, 49, 50, 51, 52, 53,
                11,10,9,14,13,12,17,16,15]
        );
        return result;
    }

    public static long MoveY2(long state)
    {
        var result = Helpers.SwapBitGroups(state,
            [0, 1, 2, 3, 5, 6, 7, 8,
                9, 10, 11, 12, 13, 14, 15, 16, 17,
                18, 19, 20, 21, 22, 23, 24, 25, 26,
                27, 28, 29, 30, 32, 33, 34, 35,
                36, 37, 38, 39, 40, 41, 42, 43, 44,
                45, 46, 47, 48, 49, 50, 51, 52, 53],
            [8,7,6,5,3,2,1,0,
                38,37,36,41,40,39,44,43,42,
                47,46,45,50,49,48,53,52,51,
                35,34,33,32,30,29,28,27,
                11,10,9,14,13,12,17,16,15,
                20,19,18,23,22,21,26,25,24]
        );
        return result;
    }

    public static long MoveYi(long state)
    {
        var result = Helpers.SwapBitGroups(state,
            [0, 1, 2, 3, 5, 6, 7, 8,
                9, 10, 11, 12, 13, 14, 15, 16, 17,
                18, 19, 20, 21, 22, 23, 24, 25, 26,
                27, 28, 29, 30, 32, 33, 34, 35,
                36, 37, 38, 39, 40, 41, 42, 43, 44,
                45, 46, 47, 48, 49, 50, 51, 52, 53],
            [2,5,8,1,7,0,3,6,
                47,46,45,50,49,48,53,52,51,
                9,10,11,12,13,14,15,16,17,
                29,32,35,28,34,27,30,33,
                20,19,18,23,22,21,26,25,24,
                36,37,38,39,40,41,42,43,44]
        );
        return result;
    }

    public static long MoveZ(long state)
    {
        var result = Helpers.SwapBitGroups(state,
            [0, 1, 2, 3, 4, 5, 6, 7, 8,
                9, 10, 11, 12, 14, 15, 16, 17,
                18, 19, 20, 21, 22, 23, 24, 25, 26,
                27, 28, 29, 30, 31, 32, 33, 34, 35,
                36, 37, 38, 39, 41, 42, 43, 44,
                45, 46, 47, 48, 49, 50, 51, 52, 53],
            [53,50,47,52,49,46,51,48,45,
                15,12,9,16,10,17,14,11,
                6,3,0,7,4,1,8,5,2,
                26,23,20,25,22,19,24,21,18,
                42,39,36,43,37,44,41,38,
                33,30,27,34,31,28,35,32,29]
        );
        return result;
    }

    public static long MoveZ2(long state)
    {
        var result = Helpers.SwapBitGroups(state,
            [0, 1, 2, 3, 4, 5, 6, 7, 8,
                9, 10, 11, 12, 14, 15, 16, 17,
                18, 19, 20, 21, 22, 23, 24, 25, 26,
                27, 28, 29, 30, 31, 32, 33, 34, 35,
                36, 37, 38, 39, 41, 42, 43, 44,
                45, 46, 47, 48, 49, 50, 51, 52, 53],
            [29,28,27,32,31,30,35,34,33,
                17,16,15,14,12,11,10,9,
                51,52,53,48,49,50,45,46,47,
                2,1,0,5,4,3,8,7,6,
                44,43,42,41,30,38,37,36,
                24,25,26,21,22,23,18,19,20]
        );
        return result;
    }

    public static long MoveZi(long state)
    {
        var result = Helpers.SwapBitGroups(state,
            [0, 1, 2, 3, 4, 5, 6, 7, 8,
                9, 10, 11, 12, 14, 15, 16, 17,
                18, 19, 20, 21, 22, 23, 24, 25, 26,
                27, 28, 29, 30, 31, 32, 33, 34, 35,
                36, 37, 38, 39, 41, 42, 43, 44,
                45, 46, 47, 48, 49, 50, 51, 52, 53],
            [20,23,26,19,22,25,18,21,24,
                11,14,17,10,16,9,12,15,
                35,32,29,34,31,28,33,30,27,
                47,50,53,46,49,52,45,48,51,
                38,41,44,37,43,36,39,42,
                8,5,2,7,4,1,6,3,0]
        );
        return result;
    }
}
