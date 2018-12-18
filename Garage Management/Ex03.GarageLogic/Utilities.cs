using System;
using System.Linq;
using System.Threading;

namespace GarageLogic
{
    public static class Utilities
    {
        public const int k_MaxPercentage = 100;
        public const int k_MillisecondsTimeout = 2200;

        public static void IsDigitsOnly(string i_Str)
        {
            if (!i_Str.All(char.IsDigit))
            {
                throw new FormatException("Parameter should contain digits only.");
            }
        }

        public static void IsLettersOnly(string i_Str)
        {
            if (!i_Str.All(i_C => char.IsSeparator(i_C) || char.IsLetter(i_C)))
            {
                throw new FormatException("Parameter should contain letters only.");
            }
        }

        public static void IsNonNegativeFloat(string i_Str)
        {
            bool isNonNegative = float.TryParse(i_Str, out float floatResult) && floatResult >= 0;
            if (!isNonNegative)
            {
                throw new FormatException("Parameter should be a non negative number.");
            }
        }

        public static void SleepAndClearScreen(bool i_ClearScreen)
        {
            Thread.Sleep(k_MillisecondsTimeout);
            if (i_ClearScreen)
            {
                Console.Clear();
            }

            Console.ForegroundColor = ConsoleColor.Gray;
        }
    }
}