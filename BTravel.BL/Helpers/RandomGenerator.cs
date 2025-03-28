using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTravel.BL.Helpers
{
    public static class RandomGenerator
    {
        public static string GenerateSixNumbers()
        {
            var rndm = new Random();

            var result = rndm.Next(100000, 999999);

            return result.ToString();
        }

        public static string Generate8Numbers()
        {
            var rndm = new Random();

            var result = rndm.Next(10000000, 99999999);

            return result.ToString();
        }

        public static string GenerateRandomDigits(int NoOfDigits)
        {
            StringBuilder sb = new StringBuilder();
            Random random = new Random();
            for (int i = 0; i < NoOfDigits; i++) // No. of digits
            {
                char c;

                do
                {
                    c = (char)random.Next(33, 127);
                }
                while (!char.IsLetterOrDigit(c));

                sb.Append(c);
            }

            string result = sb.ToString();

            return result;
        }
    }
}