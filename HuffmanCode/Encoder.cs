using HuffmanCode.BinaryCoders;
using HuffmanCode.BinaryCoders.Base;
using HuffmanCode.PriorityQueues;
using HuffmanCode.PriorityQueues.Base;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;

namespace HuffmanCode
{
    class Encoder
    {
        private string textToEncode;

        public Dictionary<char, Node> CharToCode { get; private set; }

        public Dictionary<List<byte>, char> CodeToChar { get; private set; }

        public int TextLength { get; private set; }

        public List<byte> EncodedText { get; private set; }

        private string fileName;

        public Encoder(string fileName)
        {
            this.fileName = fileName;
            CodeToChar = new Dictionary<List<byte>, char>();
            CharToCode = new Dictionary<char, Node>();
        }

        public void Encode()
        {
            textToEncode = ReadText(fileName);
            TextLength = textToEncode.Length;

            //Dictionary<char, int> charFreq = textToEncode.GroupBy(x => x).ToDictionary(x => x.Key, x => x.Count());
            Dictionary<char, int> charFreq = textToEncode.AsParallel().GroupBy(x => x).ToDictionary(x => x.Key, x => x.Count());

            List<LeafNode> leafNodeList = new List<LeafNode>();

            string priotityQueueType = ConfigurationManager.AppSettings.Get("PriorityQueueType");
            IPriorityQueue<int, Node> queue = PriorityQueueCreator.Create<int, Node>(priotityQueueType);

            foreach (var kvp in charFreq)
            {
                LeafNode leafNode = new LeafNode(kvp.Key, kvp.Value);
                queue.Enqueue(kvp.Value, leafNode);
                CharToCode.Add(kvp.Key, leafNode);
                leafNodeList.Add(leafNode);
            }

            while (queue.Count() > 1)
            {
                Node first = queue.Dequeue();
                Node second = queue.Dequeue();
                InternalNode node = new InternalNode(first, second);
                queue.Enqueue(node.Sum, node);
            }

            var rootNode = queue.Dequeue();

            if (rootNode is LeafNode)
                rootNode.BuildCode(new List<byte>(new byte[] { 0 }));
            else rootNode.BuildCode(new List<byte>());

            foreach (var leafNode in leafNodeList)
                CodeToChar.Add(leafNode.Code, leafNode.Sym);

            var charToList = CharToCode.ToDictionary(x => x.Key, y => y.Value.Code);

            string binaryCoderType = ConfigurationManager.AppSettings.Get("BinaryCoderType");
            IBinaryCoder bc = BinaryCoderCreator.Create(charToList, textToEncode, binaryCoderType);

            bc.EncodeToBinary();
            EncodedText = bc.EncodedText;
        }

        public void WriteToFile(string fileName)
        {
            FileStream fs = new FileStream(fileName, FileMode.Create, FileAccess.Write);
            BinaryWriter bw = new BinaryWriter(fs);
            foreach (var num in EncodedText)
                bw.Write(num);
            bw.Close();
            fs.Close();
        }

        private string ReadText(string fileName)
        {
            StreamReader sr = new StreamReader(fileName, Encoding.GetEncoding(1251));
            string text = sr.ReadToEnd();
            sr.Close();
            return text;
        }
    }
}
