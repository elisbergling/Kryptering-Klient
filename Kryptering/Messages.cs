using System.Collections.Generic;

namespace Kryptering
{
    public class Messages
    {
        static List<Message> messages = new List<Message>();

        public static List<Message> MessagesProp
        {
            get => messages;
            set => messages = value;
        }
    }
}