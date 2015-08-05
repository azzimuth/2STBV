using _2STBV.Common.Classes;
using System.Collections.Generic;

namespace _2STBV.Util
{
    public class PersistentQueue
    {
        public static Queue<Message> Queue = new Queue<Message>();

        public static void AddToQueue(Message message)
        {
            Queue.Enqueue(message);
        }
    }
}