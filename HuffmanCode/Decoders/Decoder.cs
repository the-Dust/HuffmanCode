using HuffmanCode.Decoders.Base;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace HuffmanCode.Decoders
{
    class Decoder : IDecoder
    {
        private Dictionary<string, char> codeToChar;

        private string fileToDecode;

        private LinkedList<byte> code;

        private StringBuilder outStr;

        private int textLength;

        public string DecodedText { get; private set; }

        public Decoder(Dictionary<List<byte>, char> map, string fileToDecode, int textLength)
        {
            this.codeToChar = map.ToDictionary(kvp=>string.Concat(kvp.Key), kvp=> kvp.Value);
            this.fileToDecode = fileToDecode;
            this.textLength = textLength;
            code = new LinkedList<byte>();
            outStr = new StringBuilder();
        }

        public void Decode()
        {
            FileStream fsIn = new FileStream(fileToDecode, FileMode.Open, FileAccess.Read);
            BinaryReader br = new BinaryReader(fsIn);
            for (int i = 0; i < fsIn.Length; i++)
            {
                ByteToBits(br.ReadByte());
            }
            br.Close();
            fsIn.Close();

            for (int i = 0; i < textLength; i++)
            {
                string temp = "";
                while (!codeToChar.ContainsKey(temp) && code.Count != 0)
                {
                    byte b = code.First.Value;
                    code.RemoveFirst();
                    temp += b.ToString();
                }
                outStr.Append(codeToChar[temp]);
            }
            DecodedText = outStr.ToString();
        }

        public void SaveToText(string fileName, Encoding encoding)
        {
            FileStream fsOut = new FileStream(fileName, FileMode.Create, FileAccess.Write);
            StreamWriter sw = new StreamWriter(fsOut, encoding);
            sw.Write(DecodedText);
            sw.Close();
            fsOut.Close();
        }

        private void ByteToBits(byte b)
        {
            for (int i = 0; i < 8; i++)
            {
                byte decoded = (byte)((b >> 7 - i)& 1);
                code.AddLast(decoded);
            }
        }
    }
}
