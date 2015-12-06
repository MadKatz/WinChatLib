using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using WinChatClient.Client.Tcp;
using WinChatNet.Client.Tcp;

namespace WinChatClient
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Press 1 to connect to localhost server.");
            Console.WriteLine("Press 2 to exit.");
            String input = null;
            while (input == null)
            {
                input = Console.ReadLine();
                if (input == "1")
                {
                    Connect();
                    Console.WriteLine("Press 1 to connect to localhost server.");
                    Console.WriteLine("Press 2 to exit.");
                    input = null;
                }
                else if (input == "2")
                {
                    System.Environment.Exit(0);
                }
                else
                {
                    input = null;
                }
            }
            
        }

        private static void Connect()
        {
            Console.WriteLine("Enter username: ");
            String username = Console.ReadLine();
            WCTcpChatClient client = new WCTcpChatClient();
            client.Connect(IPAddress.Parse("127.0.0.1"), 5005, username);
        }
    }
}
