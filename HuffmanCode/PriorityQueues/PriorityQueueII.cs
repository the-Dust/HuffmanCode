using HuffmanCode.PriorityQueues.Base;
using System;
using System.Collections.Generic;

namespace HuffmanCode.PriorityQueues
{
    class PriotityQueueII<TPriority, TItem> : IPriorityQueue<TPriority, TItem> where TPriority : IComparable where TItem : class
    {
        private List<KeyValuePair<TPriority, TItem>> list;

        public int Count()
        {
            return list.Count;
        }

        public PriotityQueueII()
        {
            list = new List<KeyValuePair<TPriority, TItem>>();
        }

        public void Enqueue(TPriority priority, TItem item)
        {
            KeyValuePair<TPriority, TItem> kvp = new KeyValuePair<TPriority, TItem>(priority, item);
            list.Add(kvp);
            int index = list.Count - 1;
            if (list.Count > 1)
                SiftUp(index);
        }

        public TItem Dequeue()
        {
            if (list.Count == 0) return null;
            TItem max = list[0].Value;
            int indexOfLast = list.Count - 1;
            list[0] = list[indexOfLast];
            list.RemoveAt(indexOfLast);
            if (list.Count > 1)
                SiftDown(0);
            return max;
        }

        private void SiftUp(int index)
        {
            int parentIndex = (int)Math.Floor(((double)index - 1) / 2);
            while (parentIndex >= 0 && list[parentIndex].Key.CompareTo(list[index].Key) > 0)
            {
                ListSwap(list, parentIndex, index);
                index = parentIndex;
                parentIndex = (int)Math.Floor(((double)index - 1) / 2);
            }
        }

        private void SiftDown(int index)
        {
            while (true)
            {
                int indexOfLeftChild = 2 * index + 1;
                int indexOfRightChild = 2 * index + 2;
                int indexToSwap;
                if (indexOfLeftChild > list.Count - 1)
                    break;
                if (indexOfRightChild > list.Count - 1)
                    indexToSwap = indexOfLeftChild;
                else indexToSwap = list[indexOfLeftChild].Key.CompareTo(list[indexOfRightChild].Key) < 0 ? indexOfLeftChild : indexOfRightChild;
                if (list[indexToSwap].Key.CompareTo(list[index].Key)<0)
                {
                    ListSwap(list, indexToSwap, index);
                    index = indexToSwap;
                }
                else break;
            }

        }

        private void ListSwap(List<KeyValuePair<TPriority, TItem>> l, int a, int b)
        {
            KeyValuePair<TPriority, TItem> temp = l[a];
            l[a] = l[b];
            l[b] = temp;
        }

    }
}
