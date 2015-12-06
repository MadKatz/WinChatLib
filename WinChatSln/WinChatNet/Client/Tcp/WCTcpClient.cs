using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using WinChatNet.Channels.Tcp;
using WinChatNet.Messages;

namespace WinChatNet.Client.Tcp
{
    public class WCTcpClient : WCClientBase
    {
        protected TcpCommunicationChannel CommunicationChannel;
        public TcpClient Client { get; set; }

        public override void Connect(IPAddress ip, int port, String username)
        {
            IP = ip;
            Port = port;
            Username = username;
            Client = new TcpClient();
            CommunicationChannel = new TcpCommunicationChannel(Client, ip, port);
            Client.BeginConnect(ip, port, ConnectCallBack, null);
        }

        public override void Disconnect(IWCMessage message)
        {
            CommunicationChannel.SendMessage(message);
            CommunicationChannel.Disconnect(false);
        }

        public void SendMessage(IWCMessage message)
        {
            CommunicationChannel.SendMessage(message);
        }

        protected void ComChannel_OnMessageReceivedEvent(Object sender, WCMessageEventArg messageEventArgs)
        {
            Handle_Message(messageEventArgs.Message);
        }

        protected void ComChannel_OnDisconnectEvent(Object sender, EventArgs eventArgs)
        {
            SendDisconnectedEvent(new WCMessage("Client Communication Error", WCMessageType.DISCONNECT));
        }

        protected void Handle_Message(IWCMessage message)
        {
            switch (message.MessageType)
            {
                case WCMessageType.CONNECTREQUEST:
                    break;
                case WCMessageType.CONNECTACCEPT:
                    SendConnectedEvent(message);
                    break;
                case WCMessageType.CONNECTDENY:
                    CommunicationChannel.Disconnect(false);
                    SendDisconnectedEvent(message);
                    break;
                case WCMessageType.MESSAGE:
                    SendMessageReceivedEvent(message);
                    break;
                case WCMessageType.SERVER:
                    SendMessageReceivedEvent(message);
                    break;
                case WCMessageType.DISCONNECT:
                    CommunicationChannel.Disconnect(false);
                    SendDisconnectedEvent(message);
                    break;
                default:
                    break;
            }
        }

        protected void ConnectCallBack(IAsyncResult ar)
        {
            try
            {
                Client.EndConnect(ar);
                SendConnectedEvent(new WCMessage("***Connection to Server established", WCMessageType.SERVER));
                CommunicationChannel.MessageRecieved += new EventHandler<WCMessageEventArg>(ComChannel_OnMessageReceivedEvent);
                CommunicationChannel.Disconnected += new EventHandler(ComChannel_OnDisconnectEvent);
                CommunicationChannel.Start();
                CommunicationChannel.SendMessage(new WCMessage(Username, WCMessageType.CONNECTREQUEST));
            }
            catch (SocketException se)
            {
                SendDisconnectedEvent(new WCMessage("Connection Resufed. Error Code: " + se.ErrorCode, WCMessageType.DISCONNECT));
            }
            catch (Exception ex)
            {
                SendDisconnectedEvent(new WCMessage("Failed to Connect to Server. " + ex.Message, WCMessageType.DISCONNECT));
            }
        }
    }
}
