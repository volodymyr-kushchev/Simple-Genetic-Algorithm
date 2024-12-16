using System.Drawing;

namespace Domain.Services;

public static class Converter
{
    public static Color FromBoolToColor(bool[] arr)
    {
        bool[] b0 = new bool[8], b1 = new bool[8], b2 = new bool[8], b3 = new bool[8];
        for (var i = 0; i < 8; i++)
            b0[i] = arr[i];
        for (var i = 8; i < 16; i++)
            b1[(i - 8)] = arr[i];
        for (var i = 16; i < 24; i++)
            b2[(i - 16)] = arr[i];
        for (var i = 24; i < 32; i++)
            b3[(i - 24)] = arr[i];
        var n0 = Convert.ToInt32(ConvertBoolArrayToByte(b0));
        var n1 = Convert.ToInt32(ConvertBoolArrayToByte(b1));
        var n2 = Convert.ToInt32(ConvertBoolArrayToByte(b2));
        var n3 = Convert.ToInt32(ConvertBoolArrayToByte(b3));
        var color = Color.FromArgb(n0, n1, n2, n3);
        return color;
    }
    
    public static byte ConvertBoolArrayToByte(bool[] source)
    {
        byte result = 0;
        // This assumes the array never contains more than 8 elements!
        var index = 8 - source.Length;

        // Loop through the array
        foreach (var b in source)
        {
            // if the element is 'true' set the bit at that position
            if (b)
                result |= (byte)(1 << (7 - index));

            index++;
        }

        return result;
    }
    
    public static bool[] ConvertByteToBoolArray(byte b)
    {
        // prepare the return result
        var result = new bool[8];

        // check each bit in the byte. if 1 set to true, if 0 set to false
        for (var i = 0; i < 8; i++)
            result[i] = (b & (1 << i)) != 0;
        
        return result;
    }
    
    public static string ChromosomeToString(bool[] chromosome)
    {
        return chromosome.Aggregate(string.Empty, (current, chroma) => current + (chroma ? "1" : "0"));
    }
}