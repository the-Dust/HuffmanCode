using HuffmanCode.BinaryCoders.Base;
using System.Collections.Generic;

namespace HuffmanCode.BinaryCoders
{
    class BinaryCoderCreator
    {
        public static IBinaryCoder Create(Dictionary<char, List<byte>> charToCode, string text, string binaryCoderType)
        {
            switch (binaryCoderType)
            {
                case "usual": return new BinaryCoder(charToCode, text);
                case "async": return new AsyncBinaryCoder(charToCode, text);
                case "asyncAdvanced": return new AsyncAdvancedBinaryCoder(charToCode, text);
                default: return new BinaryCoder(charToCode, text);
            }
        }
    }
}
