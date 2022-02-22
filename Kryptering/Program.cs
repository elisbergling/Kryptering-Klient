using System;
using System.Text;
using System.Net.Sockets;
using System.Text.RegularExpressions;

namespace Kryptering
{
    class Program
    {
        public static void Main(string[] args)
        {
            const string address = "127.0.0.1";
            const int port = 8001;
            TcpClient tcpClient = new TcpClient();
            Anslut(tcpClient, address, port);

            NetworkStream tcpStream = tcpClient.GetStream();

            bool flag = true;

            Console.WriteLine("Välkommen till den här chat liknande saken");

            //Programloop
            while (flag)
            {
                Console.WriteLine("\nAnge/Ändra användanamn(1)");
                Console.WriteLine("Skapa ett nytt meddelande(2)");
                Console.WriteLine("Visa alla meddelande (3)");
                Console.WriteLine("Hämta sparade meddelade(4)");
                Console.WriteLine("Avsluta programmet(5)");
                int input = SäkerInmatning();

                switch (input)
                {
                    case 1:
                        AngeAnvändarnamn();
                        break;
                    case 2:
                        SkapaMeddelande(tcpStream);
                        break;
                    case 3:
                        VisaMeddelande();
                        break;
                    case 4:
                        HämtaMeddalande(tcpStream);
                        break;
                    case 5:
                        AvslutaProgrammet(tcpClient, tcpStream);
                        flag = false;
                        break;
                }
            }

            Console.WriteLine("Hejdå");
        }

        static void AngeAnvändarnamn()
        {
            User.Name = SäkerInmatning2("\nVad är ditt användarnamn: ");
        }

        static void SkapaMeddelande(NetworkStream tcpStream)
        {
            //Kollar något användarnamn är angivet
            if (User.Name == null)
            {
                Console.WriteLine("Verkar som du inte har angett användarnamn");
                Console.WriteLine("Detta måste ske före du skickar iväg ett meddelande");
                AngeAnvändarnamn();
            }

            // Skriv in meddelandet att skicka:
            String message = SäkerInmatning2("\nSkriv in meddelande: ");
            Message m = new Message(message);
            byte[] mByte = m.ToByte();
            // Skicka iväg meddelandet:
            tcpStream.Write(mByte, 0, mByte.Length);
            Console.WriteLine("\nSkickat!");
        }

        static void VisaMeddelande()
        {
            //Se ifall lista är tom
            if (Messages.MessagesProp.Count == 0)
            {
                Console.WriteLine("\nDet finns inga meddelanden här. Hämta dem på servern först.");
            }
            else
            {
                //Visa meddelanden
                foreach (var m in Messages.MessagesProp)
                {
                    m.VisaMeddelande();
                }
            }
        }

        static void HämtaMeddalande(NetworkStream tcpStream)
        {
            byte[] mByte = Encoding.ASCII.GetBytes("0"); //0 används som meddelande för att hämta från Servern
            // Skicka iväg meddelandet:
            tcpStream.Write(mByte, 0, mByte.Length);
            // Tag emot meddelande från servern:
            byte[] bRead = new byte[256];
            int bReadSize = tcpStream.Read(bRead, 0, bRead.Length);
            // Konvertera meddelandet till ett string-objekt och skriv ut:
            string read = "";
            for (int i = 0; i < bReadSize; i++)
                read += Convert.ToChar(bRead[i]);
            //Kolla ifall det finns meddelanden
            if (read == "2")
            {
                Console.WriteLine("\nDet fanns inga meddelanden att hämta. Skapa ett först.");
            }
            else
            {
                //Konvertera sträng från servern till olika Message objekt 
                string[] pattern = {"@@@"}; //@@@ används som skiljetecken mellan alla meddelanden
                string[] splitedString = read.Split(pattern, StringSplitOptions.RemoveEmptyEntries);

                //Tömmer befintliga meddelanden ifall användaren har hämtat tidigare
                Messages.MessagesProp.Clear();

                //Lägger till meddelanden i meddelande listan
                foreach (var split in splitedString)
                {
                    Messages.MessagesProp.Add(new Message(split, null));
                }

                Console.WriteLine("Meddelanden är hämtade");
            }
        }

        static void AvslutaProgrammet(TcpClient tcpClient, NetworkStream tcpStream)
        {
            byte[] mByte = Encoding.ASCII.GetBytes("1"); //1 används som meddelande för att avsluta Servern
            // Skicka iväg meddelandet:
            tcpStream.Write(mByte, 0, mByte.Length);
            // Stäng anslutningen:
            tcpClient.Close();
        }

        static void Anslut(TcpClient tcpClient, string address, int port)
        {
            try
            {
                // Anslut till servern:
                Console.WriteLine("Ansluter...");
                tcpClient.Connect(address, port);
                Console.WriteLine("Ansluten!\n");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine("Programmet stängs av. Hejdå");
                //Stänger av programmet ifall det inte finns någon tillgänglig server
                Environment.Exit(0);
            }
        }

        static int SäkerInmatning()
        {
            int number;

            while (true)
            {
                try
                {
                    //Mata in en sträng från konsollen
                    Console.Write("\nVälj vad du vill göra: ");
                    string? input = Console.ReadLine();

                    //Se ifall strängen är koverterbar till int. Om den är det ges variablen number inputs värde.
                    if (!int.TryParse(input, out number))
                    {
                        throw new NotANumberException();
                    }

                    //Se ifall nummret ligger inom rätt värden
                    if (number > 5 || number < 1)
                    {
                        throw new NumberOutOfBoundsException();
                    }

                    return number;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }

        static string SäkerInmatning2(string text)
        {
            //Definerar så att inmatningssträngen inte innhåller specialtecken
            Regex rg = new Regex(@"[^0-9a-zA-Z]+");
            while (true)
            {
                try
                {
                    Console.Write(text);
                    string input = Console.ReadLine()!;

                    //Ser ifall inmatningssträngen innhåller specialtecken
                    if (rg.IsMatch(input))
                    {
                        throw new NotAlowedInputStringException();
                    }

                    return input;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }
    }
}