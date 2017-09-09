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
                case "firstVersion": return new PriorityQueue<TPriority, TItem>();
                case "secondVersion": return new PriorityQueueII<TPriority, TItem>();
                default: return new PriorityQueue<TPriority, TItem>();
            }
        }
    }
}
