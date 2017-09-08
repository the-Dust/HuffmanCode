using HuffmanCode.Decoders.Base;
using System.Collections.Generic;

namespace HuffmanCode.Decoders
{
    class DecoderCreator
    {
        public static IDecoder Create(Dictionary<List<byte>, char> map, string fileToDecode, int textLength, string binaryCoderType)
        {
            switch (binaryCoderType)
            {
                case "usual": return new Decoder(map, fileToDecode, textLength);
                case "async": return new AsyncDecoder(map, fileToDecode, textLength);
                case "asyncAdvanced": return new AsyncAdvancedDecoder(map, fileToDecode, textLength);
                default: return new Decoder(map, fileToDecode, textLength);
            }
        }
    }
}
