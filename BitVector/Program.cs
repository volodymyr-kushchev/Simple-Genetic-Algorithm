using System;
using System.Collections;
using System.Collections.Specialized;
using System.Drawing;

namespace VectorOfBits
{
    class Program
    {
        static void Main(string[] args)
        {
            var colorBit = new BitVector32(-65536);// red color 
            var colorBitArr = new BitArray(32);
            Color colorRed = Color.Red;
            bool[] red = new bool[32];
            red = FromColorToBool(colorRed);
            string resColor = "";
            for (int i = 0; i < 32; i++)
            {
                //if (red[i] == true)
                //    resColor += "1";
                //else resColor += "0";
                resColor += red[i].ToString();
            }
            Console.WriteLine(resColor);


            Console.WriteLine(colorBit.ToString());
        }



        private Color FromBoolToColor(bool[] arr)
        {
            bool[] res = new bool[32];
            bool[] b0 = new bool[8], b1 = new bool[8], b2 = new bool[8], b3 = new bool[8];
            for (int i = 0; i < 8; i++)
                b0[i] = arr[i];
            for (int i = 8; i < 16; i++)
                b1[(i - 8)] = arr[i];
            for (int i = 16; i < 24; i++)
                b2[(i - 16)] = arr[i];
            for (int i = 24; i < 32; i++)
                b3[(i - 24)] = arr[i];
            Int32 n0 = Convert.ToInt32(ConvertBoolArrayToByte(b0));
            Int32 n1 = Convert.ToInt32(ConvertBoolArrayToByte(b1));
            Int32 n2 = Convert.ToInt32(ConvertBoolArrayToByte(b2));
            Int32 n3 = Convert.ToInt32(ConvertBoolArrayToByte(b3));
            Color color = Color.FromArgb(n0, n1, n2, n3);
            return color;

        }

        static public bool[] FromColorToBool(Color color)
        {
            bool[] res = new bool[32];
            bool[] b0 = ConvertByteToBoolArray(color.A);
            bool[] b1 = ConvertByteToBoolArray(color.R);
            bool[] b2 = ConvertByteToBoolArray(color.G);
            bool[] b3 = ConvertByteToBoolArray(color.B);
            for(int i =0;i<8;i++)
            {
                Console.WriteLine(b3[i]);
            }
            for (int i = 0; i < 8; i++)
                res[i] = b0[i];
            for (int i = 8; i < 16; i++)
                res[i] = b1[(i - 8)];
            for (int i = 16; i < 24; i++)
                res[i] = b2[(i - 16)];
            for (int i = 24; i < 32; i++)
                res[i] = b3[(i - 24)];
            return res;

        }
        static private byte ConvertBoolArrayToByte(bool[] source)
        {
            byte result = 0;
            // This assumes the array never contains more than 8 elements!
            int index = 8 - source.Length;

            // Loop through the array
            foreach (bool b in source)
            {
                // if the element is 'true' set the bit at that position
                if (b)
                    result |= (byte)(1 << (7 - index));

                index++;
            }

            return result;
        }
        // Auxiliary method
        static private bool[] ConvertByteToBoolArray(byte b)
        {
            // prepare the return result
            bool[] result = new bool[8];

            // check each bit in the byte. if 1 set to true, if 0 set to false
            for (int i = 0; i < 8; i++)
                result[i] = (b & (1 << i)) == 0 ? false : true;

            // reverse the array
            //Array.Reverse(result);

            return result;
        }

    }
}
