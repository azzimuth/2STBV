using _2WTBA.Common.Classes;
using System.Collections.Generic;

namespace _2WTBA.Util
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