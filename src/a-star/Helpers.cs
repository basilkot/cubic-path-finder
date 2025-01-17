namespace AStarConsoleApp;

internal class Helpers
{
    public static long SwapBitGroups(long value, int[] posGroup1, int[] posGroup2)
    {
        var groupSize = posGroup2.Length;

        var result = value;

        for (var i = 0; i < groupSize; i++)
        {
            var bit = (value >> posGroup2[i]) & 1;
            result &= ~(1L << posGroup1[i]);
            result |= (bit << posGroup1[i]);
        }

        return result;
    }
}
