using System;
using System.Collections.Generic;

namespace ConsoleApplication1
{
    class Node : IComparable<Node>
    {
        public int Sum{get; protected set;}
        public List<byte> Code{get; protected set;}
        public Node(int sum)
        {
            Sum = sum;
            Code = new List<byte>();
        }
        public int CompareTo(Node node)
        {
            return this.Sum.CompareTo(node.Sum);
        }
        public virtual void BuildCode(List<byte> code)
        {
            Code = code;
        }
        public string CodeToString()
        {
            return string.Concat(Code);
        }
    }

    class InternalNode : Node
    {
        Node leftNode;
        Node rightNode;
        public InternalNode(Node leftNode, Node rightNode)
            : base(leftNode.Sum + rightNode.Sum)
        {
            this.leftNode = leftNode;
            this.rightNode = rightNode;
        }
        public override void BuildCode(List<byte> code)
        {

            List<byte> l = new List<byte>(code);
            l.Add(0);
            leftNode.BuildCode(l);
            List<byte> r = new List<byte>(code);
            r.Add(1);
            rightNode.BuildCode(r);
        }
    }

    class LeafNode : Node
    {
        public char Sym { get; protected set; }
        public LeafNode(char sym, int sum) : base(sum)
        {
            Sym = sym;
        }
    }
}
