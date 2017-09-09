using HuffmanCode.PriorityQueues.Base;
using HuffmanCode.Decoders.Base;
using HuffmanCode.BinaryCoders.Base;
using Ninject;
using System.Configuration;
using HuffmanCode.PriorityQueues;
using HuffmanCode.BinaryCoders;
using HuffmanCode.Decoders;

namespace HuffmanCode.Infrastructure
{
    public class NinjectFactory
    {
        public IKernel Kernel { get; private set; }

        public NinjectFactory()
        {
            Kernel = new StandardKernel();
            AddBindings();
        }
        
        public void AddBindings()
        {
            string priorityQueueType = ConfigurationManager.AppSettings.Get("PriorityQueueType");
            string binaryCoderType = ConfigurationManager.AppSettings.Get("BinaryCoderType");
            string decoderType = ConfigurationManager.AppSettings.Get("DecoderType");

            switch (priorityQueueType)
            {
                case "firstVersion": Kernel.Bind(typeof(IPriorityQueue<,>)).To(typeof(PriorityQueue<,>)); break;
                case "secondVersion": Kernel.Bind(typeof(IPriorityQueue<,>)).To(typeof(PriorityQueueII<,>)); break;
                default: Kernel.Bind(typeof(IPriorityQueue<,>)).To(typeof(PriorityQueueII<,>)); break;
            }

            switch (binaryCoderType)
            {
                case "usual": Kernel.Bind<IBinaryCoder>().To<BinaryCoder>(); break;
                case "async": Kernel.Bind<IBinaryCoder>().To<AsyncBinaryCoder>(); break;
                case "asyncAdvanced": Kernel.Bind<IBinaryCoder>().To<AsyncAdvancedBinaryCoder>(); break;
                default: Kernel.Bind<IBinaryCoder>().To<AsyncAdvancedBinaryCoder>(); break;
            }
            
            switch (decoderType)
            {
                case "usual": Kernel.Bind<IDecoder>().To<Decoder>(); break;
                case "async": Kernel.Bind<IDecoder>().To<AsyncDecoder>(); break;
                case "asyncAdvanced": Kernel.Bind<IDecoder>().To<AsyncAdvancedDecoder>(); break;
                default: Kernel.Bind<IDecoder>().To<AsyncAdvancedDecoder>(); break;
            }
        }
    }
}