using System;
using System.Text;

namespace Kryptering
{
    public class Message
    {
        static int number = 0;
        string text;
        string sender;
        int messageNumber;

        public Message(string text)
        {
            this.text = text;
            sender = User.Name!;
            messageNumber = ++number;
        }

        public Message(string message, int? _) //Konstig lösning men fungerar. 
        {
            string[] pattern = {"???"}; //??? används som skiljetecken mellan text, sender och messageNumber
            string[] splitedString = message.Split(pattern, StringSplitOptions.None);

            text = AvKrypteraMeddelande(splitedString[0]);
            sender = splitedString[1];
            messageNumber = int.Parse(splitedString[2]);
        }

        //Anpassar ToString
        public override string ToString()
        {
            return KrypteraMeddelande(text) + "???" + sender + "???" +
                   messageNumber;
        }

        //Krytpterar sträng med ceaser chipher. Alla bokstäver förflyttas med ett steg. A blir B
        string KrypteraMeddelande(string message)
        {
            char[] cMessage = message.ToCharArray();
            for (int i = 0; i < cMessage.Length; i++)
            {
                cMessage[i]++;
            }

            return String.Join("", cMessage);
        }

        //Avkrytpterar sträng med ceaser chipher. Alla bokstäver förflyttas med ett steg i motsatt rikning. B blir A
        string AvKrypteraMeddelande(string message)
        {
            char[] cMessage = message.ToCharArray();
            for (int i = 0; i < cMessage.Length; i++)
            {
                cMessage[i]--;
            }

            return String.Join("", cMessage);
        }

        public void VisaMeddelande()
        {
            Console.WriteLine("---------" + messageNumber + "----------");
            Console.WriteLine("Avsändare: " + sender);
            Console.WriteLine("Meddelande: " + text);
            Console.WriteLine("---------" + messageNumber + "----------");
        }

        public byte[] ToByte()
        {
            return Encoding.ASCII.GetBytes(this.ToString());
        }
    }
}