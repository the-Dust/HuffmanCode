using System;

namespace HuffmanCode.PriorityQueues.Base
{
    interface IPriorityQueue<TPriority, TItem> where TPriority : IComparable where TItem : class
    {
        void Enqueue(TPriority priority, TItem item);
        TItem Dequeue();
        int Count();
    }
}
