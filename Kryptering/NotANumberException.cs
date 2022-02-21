using System;

namespace Kryptering
{
    public class NotANumberException : Exception
    {
        string message;

        public override string Message
        {
            get => message;
        }

        public NotANumberException()
        {
            message = "Det där är ingen siffra. Försök igen.";
        }
    }
}