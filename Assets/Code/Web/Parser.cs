using System;

namespace Assembly.IBX.Web
{
    public static class Parser
    {
        /// <summary>
        /// Parses a flags integer into a boolean array
        /// </summary>
        /// <param name="flags">The flags integer to parse</param>
        /// <param name="totalFlags">The total number of bits, one for each flag</param>
        /// <returns>A boolean array representitive of the flag bits read right to left</returns>
        public static bool[] ParseFlags(int flags, int totalFlags)
        {
            //Parse the flags integer into its binary representation
            string binary = Convert.ToString(flags, 2);

            //Create the output array with an index for each flag bit in the flags binary
            bool[] parsedFlags = new bool[totalFlags];

            //For each bit in the binary...
            for(int i = binary.Length - 1; i >= 0; i--)
            {
                //Extract the current bit
                char bit = binary[i];

                //If this bit is 0 set this flag to false, otherwise true
                if(bit == '0')
                {
                    parsedFlags[i] = false;
                }
                else
                {
                    parsedFlags[i] = true;
                }
            }

            return parsedFlags;
        }
    }
}
