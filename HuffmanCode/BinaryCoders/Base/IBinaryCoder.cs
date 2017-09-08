using System.Collections.Generic;

namespace HuffmanCode.BinaryCoders.Base
{
    interface IBinaryCoder
    {
        List<byte> EncodedText { get; }
        void EncodeToBinary();
    }
}
