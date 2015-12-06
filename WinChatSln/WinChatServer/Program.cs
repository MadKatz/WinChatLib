using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using WinChatNet.Server.Tcp;

namespace WinChatServer
{
    class Program
    {
        private static WCTcpServer server;
        static void Main(string[] args)
        {
            String input = null;
            while (input == null)
            {
                input = GetInput();
                if (input.Equals("1"))
                {
                    StartServer();
                    input = null;
                }
                else if (input.Equals("2"))
                {
                    KillServer();
                    input = null;
                }
                else if (input.Equals("3"))
                {
                    KillServer();
                    System.Environment.Exit(0);
                }
            }
        }

        static String GetInput()
        {
            Console.WriteLine("1) Start server.");
            Console.WriteLine("2) Kill Server.");
            Console.WriteLine("3) Quit.");
            String input = null;
            input = Console.ReadLine();
            return input;
        }

        static void StartServer()
        {
            server = new WCTcpServer(IPAddress.Parse("127.0.0.1"), 5005);
            server.Start();
            Console.WriteLine("Server started @ 127.0.0.1:5005");
        }

        static void KillServer()
        {
            if (server != null)
            {
                if (server.Running)
                {
                    server.Stop("Server is shutting down.");
                    Console.WriteLine("Server shutting down.");
                }
            }
        }
    }
}
