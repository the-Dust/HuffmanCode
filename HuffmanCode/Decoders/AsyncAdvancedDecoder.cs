using HuffmanCode.Decoders.Base;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HuffmanCode.Decoders
{
    class AsyncAdvancedDecoder : IDecoder
    {
        private Dictionary<string, char> codeToChar;

        private string fileToDecode;

        private ConcurrentQueue<byte> buffer;

        private StringBuilder outStr;

        private int textLength;

        public string DecodedText { get; private set; }

        private CancellationTokenSource cts = new CancellationTokenSource();

        public AsyncAdvancedDecoder(Dictionary<List<byte>, char> map, string fileToDecode, int textLength)
        {
            this.codeToChar = map.ToDictionary(kvp => string.Concat(kvp.Key), kvp => kvp.Value);
            this.fileToDecode = fileToDecode;
            this.textLength = textLength;
            buffer = new ConcurrentQueue<byte>();
            outStr = new StringBuilder();
        }

        public void Decode()
        {
            Task t1 = Task.Run(() => Dequeue());
            Task t2 = Task.Run(() => Enqueue());
            Task.WaitAll(t1, t2);
        }

        public void SaveToText(string fileName, Encoding encoding)
        {
            FileStream fsOut = new FileStream(fileName, FileMode.Create, FileAccess.Write);
            StreamWriter sw = new StreamWriter(fsOut, encoding);
            sw.Write(DecodedText);
            sw.Close();
            fsOut.Close();
        }

        private void Enqueue()
        {
            FileStream fsIn = new FileStream(fileToDecode, FileMode.Open, FileAccess.Read);
            BinaryReader br = new BinaryReader(fsIn);
            for (int i = 0; i < fsIn.Length; i++)
            {
                ByteToBits(br.ReadByte());
            }
            br.Close();
            fsIn.Close();
            cts.Cancel();
        }

        private void Dequeue()
        {
            for (int i = 0; i < textLength; i++)
            {
                string temp = "";
                while (!codeToChar.ContainsKey(temp))
                {
                    if (buffer.Count == 0)
                    {
                        if (!cts.IsCancellationRequested)
                            continue;
                        else
                            goto EndOfMethod;
                    }
                    byte b;
                    buffer.TryDequeue(out b);
                    temp += b.ToString();
                }
                outStr.Append(codeToChar[temp]);
            }
            EndOfMethod:
            DecodedText = outStr.ToString();
        }

        private void ByteToBits(byte b)
        {
            for (int i = 0; i < 8; i++)
            {
                byte decoded = (byte)((b >> 7 - i) & 1);
                buffer.Enqueue(decoded);
            }
        }
    }
}
