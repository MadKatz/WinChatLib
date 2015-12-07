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
                    SendServerMessage();
                    input = null;
                }
                else if (input.Equals("3"))
                {
                    KillServer();
                    input = null;
                }
                else if (input.Equals("4"))
                {
                    KillServer();
                    System.Environment.Exit(0);
                }
                else
                {
                    Console.WriteLine(">Invalid input.\n");
                    input = null;
                }
            }
        }

        static String GetInput()
        {
            Console.WriteLine("1) Start server.");
            Console.WriteLine("2) Send Server Message.");
            Console.WriteLine("3) Kill Server.");
            Console.WriteLine("4) Quit.");
            Console.Write(">");
            String input = null;
            input = Console.ReadLine();
            return input;
        }

        static void StartServer()
        {
            Console.Write(">Enter ip to listen on: ");
            String ip = Console.ReadLine();
            server = new WCTcpServer(IPAddress.Parse(ip), 5005);
            server.Start();
            Console.WriteLine(">Server started on " + ip + ":5005\n");
        }

        static void KillServer()
        {
            if (server != null)
            {
                if (server.Running)
                {
                    server.Stop("Console shutdown signal received.");
                    Console.WriteLine(">Server shutting down.\n");
                }
                else
                {
                    Console.WriteLine(">Error: Server not started.\n");
                }
            }
            else
            {
                Console.WriteLine(">Error: Server not started.\n");
            }
        }

        static void SendServerMessage()
        {
            if (server != null)
            {
                if (server.Running)
                {
                    Console.Write("\nMessage To Send> ");
                    String input = Console.ReadLine();
                    server.SendServerMessage(input);
                    Console.WriteLine(">Sent.\n");
                }
                else
                {
                    Console.WriteLine(">Error: Server not started.\n");
                }
            }
            else
            {
                Console.WriteLine(">Error: Server not started.\n");
            }
        }
    }
}
