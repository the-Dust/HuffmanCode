using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ConsoleApplication1
{
    class Encoder
    {
        string textToEncode;
        public Dictionary<char, Node> CharToCode { get; private set; }
        public Dictionary<List<byte>, char> CodeToChar { get; private set; }
        public int TextLength { get; private set; }
        BinaryCoder bc;
        public List<byte> EncodedText { get; private set; }

        public Encoder(string textToEncode)
        {
            this.textToEncode = textToEncode;
            TextLength = textToEncode.Length;
            CodeToChar = new Dictionary<List<byte>, char>();
            CharToCode = new Dictionary<char, Node>();
        }

        public void Encode()
        {
            Dictionary<char, int> charFreq = textToEncode.GroupBy(x => x).ToDictionary(x => x.Key, x => x.Count());
            List<LeafNode> leafNodeList = new List<LeafNode>();
 
            PriotityQueue<int, Node> queue = new PriotityQueue<int, Node>();
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

            //if (rootNode.GetType() == typeof(LeafNode)) //for input strings of length 1; с typeof перемудрил) 
            if (rootNode is LeafNode)
                rootNode.BuildCode(new List<byte>(new byte[] { 0 }));
            else rootNode.BuildCode(new List<byte>());

            foreach (var leafNode in leafNodeList)
                CodeToChar.Add(leafNode.Code, leafNode.Sym);

            List<byte> code = new List<byte>();
            for (int i = 0; i < TextLength; i++)
                code.AddRange((CharToCode[textToEncode[i]].Code));
            bc = new BinaryCoder(code);
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
    }
}
