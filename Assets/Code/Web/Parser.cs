using System;
using System.Diagnostics;
using System.Linq;

namespace Assembly.IBX.WebIO
{
    public static class Parser
    {
        /// <summary>
        /// Parses a flags integer into a boolean array
        /// </summary>
        /// <param name="flags">The flags integer to parse</param>
        /// <returns>A boolean array representitive of the flag bits read right to left</returns>
        public static bool[] ParseFlags(int flags)
        {
            if(flags < 0) return null;

            //Parse the flags integer into its binary representation
            string binary = Convert.ToString(flags, 2);

            //Convert the binary number into a set of bits read from left to right
            bool[] bits = binary.Select(c => c == '1').ToArray();

            //Reverse the extracted bits so that the bits are effectively read right to left
            bits.Reverse();

            //Return these bits as a boolean array
            return bits;
        }
    }
}
