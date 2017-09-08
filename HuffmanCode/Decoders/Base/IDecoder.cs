using System.Text;

namespace HuffmanCode.Decoders.Base
{
    interface IDecoder
    {
        void Decode();
        void SaveToText(string fileName, Encoding encoding);
    }
}
