using HuffmanCode.BinaryCoders.Base;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace HuffmanCode.BinaryCoders
{
    class AsyncAdvancedBinaryCoder : IBinaryCoder
    {
        public List<byte> EncodedText { get; private set; }

        private ConcurrentQueue<byte> buffer = new ConcurrentQueue<byte>();

        private Dictionary<char, List<byte>> charToCode;

        private string text;

        private CancellationTokenSource cts = new CancellationTokenSource();

        public AsyncAdvancedBinaryCoder(Dictionary<char, List<byte>> charToCode, string text)
        {
            this.charToCode = charToCode;
            this.text = text;
            EncodedText = new List<byte>();
        }

        public void EncodeToBinary()
        {
            Task t1 = Task.Run(() => Dequeue());
            Task t2 = Task.Run(() => Enqueue());
            Task.WaitAll(t1, t2);
        }

        private void Enqueue()
        {
            int counter = 0;
            for (int i = 0; i < text.Length; i++)
            {
                var list = charToCode[text[i]];
                for (int j = 0; j < list.Count; j++)
                {
                    buffer.Enqueue(list[j]);
                    counter++;
                }
            }
            int rest = 8 - counter % 8;
            if (rest != 8)
            {
                for (int j = 0; j < rest; j++)
                {
                    buffer.Enqueue(0);
                }
            }
            cts.Cancel();
        }

        private void Dequeue()
        {
            while (!cts.IsCancellationRequested || buffer.Count != 0)
            {
                byte temp = 0;
                while (buffer.Count < 8) { }
                for (int i = 0; i < 8; i++)
                {
                    byte q;
                    buffer.TryDequeue(out q);
                    temp |= (byte)(q << 7 - i);
                }
                EncodedText.Add(temp);
            }
        }
    }
}
