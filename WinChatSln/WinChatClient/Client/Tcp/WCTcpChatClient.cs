using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using WinChatNet.Client.Tcp;
using WinChatNet.Messages;

namespace WinChatClient.Client.Tcp
{
    public class WCTcpChatClient
    {
        protected WCTcpClient client;

        //Used to gage connection status. -1 = disconnected.
        //0 = pending connection(trying to connet). 1 = connected
        protected int connected;

        public WCTcpChatClient()
        {
            connected = -1;
            client = new WCTcpClient();
            client.Connected += new EventHandler<WCMessageEventArg>(Client_OnConnectedEvent);
            client.Disconnected += new EventHandler<WCMessageEventArg>(Client_OnDisconnectEvent);
            client.MessageRecieved += new EventHandler<WCMessageEventArg>(Client_OnMessageReceivedEvent);
        }

        public void Connect(IPAddress ip, int port, String username)
        {
            if (connected > 0)
            {
                Disconnect("Quitting");
            }
            connected = 0;
            client.Connect(ip, port, username);
            StartPendingConnection();
        }

        public void Disconnect(String reason)
        {
            IWCMessage message = new WCMessage(reason, WCMessageType.DISCONNECT);
            client.Disconnect(message);
            connected = -1;
        }

        public void SendMessage(String message)
        {
            if (message == null || connected < 1)
            {
                return;
            }
            if (message[0] == '/')
            {
                Disconnect("Quitting");
            }
            else
            {
                IWCMessage msgToSend = new WCMessage(message, WCMessageType.MESSAGE);
                client.SendMessage(msgToSend);
            }
        }

        protected void StartPendingConnection()
        {
            while (connected < 1)
            {
                if (connected < 0)
                {
                    //We did not connect. (Look into throwing exception here instead)
                    //look into timeout as well
                    return;
                }
            }
            if (connected > 0)
            {
                if (!client.Client.Connected)
                {
                    Disconnect("Lost connection to server.");
                }
                String input = null;
                while (input == null && connected > 0)
                {
                    if (Console.KeyAvailable)
                    {
                        input = Console.ReadLine();
                        if (input != null && connected > 0)
                        {
                            SendMessage(input);
                            input = null;
                        }
                    }
                }
            }
        }

        protected void Client_OnConnectedEvent(Object sender, WCMessageEventArg messageEventArgs)
        {
            Console.WriteLine(messageEventArgs.Message.Message);
            connected = 1;
        }

        protected void Client_OnDisconnectEvent(Object sender, WCMessageEventArg messageEventArgs)
        {
            Console.WriteLine(messageEventArgs.Message.Message);
            connected = -1;
        }

        protected void Client_OnMessageReceivedEvent(Object sender, WCMessageEventArg messageEventArgs)
        {
            Console.WriteLine(messageEventArgs.Message.Message.ToString());
        }
    }
}
