using System;
using System.Collections.Generic;
using System.Text;

namespace Altinn.Common
{
    public static class StringExtensions
    {
        public static string ScrambleMiddlePartOfString(this String input, int charsToScramble)
        {
            int firstHalfOfWord = (int)Math.Ceiling((double)input.Length / (double)2);
            int toBeScrambledInFirstHalf = (int)Math.Ceiling((double)charsToScramble / (double)2);

            return input.Substring(0, firstHalfOfWord - toBeScrambledInFirstHalf).PadRight(firstHalfOfWord - toBeScrambledInFirstHalf + charsToScramble, 'x') + input.Substring(firstHalfOfWord - toBeScrambledInFirstHalf + charsToScramble);
        }
    }
}
