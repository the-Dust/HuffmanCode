using HuffmanCode.PriorityQueues.Base;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HuffmanCode.PriorityQueues
{
    class PriorityQueue<TPriority, TItem> : IPriorityQueue<TPriority, TItem> where TPriority : IComparable where TItem : class
    {
        private SortedDictionary<TPriority, Queue<TItem>> subQueue;

        public PriorityQueue()
        {
            this.subQueue = new SortedDictionary<TPriority, Queue<TItem>>();
        }

        public void Enqueue(TPriority priority, TItem item)
        {
            if (subQueue.ContainsKey(priority))
            {
                subQueue[priority].Enqueue(item);
            }
            else
            {
                Queue<TItem> queue = new Queue<TItem>();
                queue.Enqueue(item);
                subQueue.Add(priority, queue);
            }
        }

        public TItem Dequeue()
        {
            if (subQueue.Count == 0)
                return default(TItem);
            var kvp = subQueue.First();
            if (kvp.Value.Count != 0)
                return subQueue.First().Value.Dequeue();
            else
            {
                TPriority priority = kvp.Key;
                subQueue.Remove(priority);
                return Dequeue();
            }
        }

        public int Count()
        {
            return subQueue.Sum(kvp => kvp.Value.Count);
        }
    }
}
