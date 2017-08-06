using System.Collections.Generic;

namespace ConsoleApplication1
{
    class BinaryCoder
    {
        LinkedList<byte> code;
        public List<byte> EncodedText {get; private set;}

        public BinaryCoder(List<byte> entryList)
        {
            int rest = 8 - entryList.Count % 8;
            if (rest != 8)
                entryList.AddRange(new byte[rest]);
            code = new LinkedList<byte>(entryList);
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
    }
}
