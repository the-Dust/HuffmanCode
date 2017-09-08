using HuffmanCode.BinaryCoders.Base;
using System.Collections.Generic;

namespace HuffmanCode.BinaryCoders
{
    class BinaryCoder : IBinaryCoder
    {
        private LinkedList<byte> code;

        public List<byte> EncodedText {get; private set;}

        public BinaryCoder(Dictionary<char, List<byte>> charToCode, string text) 
        {
            code = GetCode(charToCode, text);
            EncodedText = new List<byte>();
        }

        public void EncodeToBinary()
        {
            while (code.Count != 0)
            {
                byte temp = 0;
                for (int i = 0; i < 8; i++)
                {
                    temp |= (byte)(code.First.Value << 7 - i);
                    code.RemoveFirst();
                }
                EncodedText.Add(temp);
            }
        }

        private LinkedList<byte> GetCode(Dictionary<char, List<byte>> charToCode, string text)
        {
            List<byte> entryList = new List<byte>();
            for (int i = 0; i < text.Length; i++)
                entryList.AddRange((charToCode[text[i]]));
            int rest = 8 - entryList.Count % 8;
            if (rest != 8)
                entryList.AddRange(new byte[rest]);
            return new LinkedList<byte>(entryList);
        }
    }
}
