using System;

namespace Kryptering
{
    public class NotAlowedInputStringException : Exception
    {
        string message;

        public override string Message
        {
            get => message;
        }

        public NotAlowedInputStringException()
        {
            message = "Strängen innehåller otillåtna tecken. Försök igen.";
        }
    }
}