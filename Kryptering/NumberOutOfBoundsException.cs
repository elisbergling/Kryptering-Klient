using System;

namespace Kryptering
{
    public class NumberOutOfBoundsException : Exception
    {
        string message;
        public override string Message
        {
            get => message;
        }

        public NumberOutOfBoundsException()
        {
            message = "Nummret är antingen för stort eller för litet. Försök Igen.";
        }
    }
}