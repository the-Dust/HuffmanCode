using HuffmanCode.PriorityQueues.Base;
using System;

namespace HuffmanCode.PriorityQueues
{
    class PriorityQueueCreator
    {
        public static IPriorityQueue<TPriority, TItem> Create<TPriority, TItem>(string priorityQueueType) 
                                                        where TPriority : IComparable where TItem : class
        {
            switch (priorityQueueType)
            {
                case "firstVersion": return new PriotityQueue<TPriority, TItem>();
                case "secondVersion": return new PriotityQueueII<TPriority, TItem>();
                default: return new PriotityQueue<TPriority, TItem>();
            }
        }
    }
}
