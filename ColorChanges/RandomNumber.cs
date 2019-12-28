using System;
using System.Security.Cryptography;

namespace ColorChanges
{
    public static class RandomNumber
    {
        static RNGCryptoServiceProvider rnd;
        static RandomNumber()
        {
            rnd = new RNGCryptoServiceProvider();
        }

        // not more then 255
        public static int GetValue(int from, int to)
        {
            byte[] randomNumber = new byte[1];
            rnd.GetBytes(randomNumber);
            return (Convert.ToInt32(randomNumber.GetValue(0)) % to + from);
        }
    }
}
