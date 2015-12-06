using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using WinChatNet.Channels;
using WinChatNet.Channels.Tcp;
using WinChatNet.Messages;

namespace WinChatNet.Server.Tcp
{
    public class WCTcpServer : WCServerBase
    {
        protected const String tempUsername = "anonymous123";
        protected Dictionary<Guid, WCServerClient> clients;
        protected Dictionary<Guid, WCServerClient> pendingClients;
        protected IPAddress ip;
        protected int port;

        public WCTcpServer(IPAddress ip, int port)
            : base()
        {
            this.ip = ip;
            this.port = port;
        }

        public override void Start()
        {
            clients = new Dictionary<Guid, WCServerClient>();
            pendingClients = new Dictionary<Guid, WCServerClient>();
            listener = new TcpConnectionListener(ip, port);
            Running = true;
            listener.Start();
            listener.CommunicationChannelConnected += new EventHandler<CommunicationChannelEventArg>(Listener_OnConnectedEvent);
        }

        public override void Stop(String shutDownMessage)
        {
            listener.Stop();
            Running = false;
            String str = "***Server is shutting down. Reason ( ";
            IWCMessage message = new WCMessage(str + shutDownMessage + " )", WCMessageType.DISCONNECT);
            foreach (var client in clients.Values)
            {
                client.CommunicationChannel.SendMessage(message);
                client.CommunicationChannel.Disconnect(false);
            }
        }

        public override void SendMessage(IWCMessage message)
        {
            foreach (var client in clients.Values)
            {
                client.CommunicationChannel.SendMessage(message);
            }
        }

        protected void Listener_OnConnectedEvent(Object sender, CommunicationChannelEventArg comChannelEventArgs)
        {
            TcpCommunicationChannel comChannel = (TcpCommunicationChannel)comChannelEventArgs.CommunicationChannel;
            WCServerClient client = new WCServerClient(comChannel, tempUsername);
            client.MessageRecieved += new EventHandler<WCMessageEventArg>(Client_OnMessageReceivedEvent);
            client.Disconnected += new EventHandler<WCServerClientEventArgs>(Client_OnDisconnectEvent);
            client.Connected += new EventHandler<WCServerClientEventArgs>(Client_OnConnectedEvent);
            client.ConnectionDenied += new EventHandler<WCServerClientEventArgs>(Client_OnConnectionDenyEvent);
            pendingClients.Add(client.ClientID, client);
            client.Start();
        }

        protected void Client_OnMessageReceivedEvent(Object sender, WCMessageEventArg messageEventArgs)
        {
            SendMessage(messageEventArgs.Message);
        }

        protected void Client_OnConnectedEvent(Object sender, WCServerClientEventArgs clientEventArgs)
        {
            WCServerClient client = null;
            pendingClients.TryGetValue(clientEventArgs.ClientID, out client);
            if (client == null)
            {
                //TODO: throw expection here
                Debug.WriteLine("Failed to find clientID in pendingClients during Client_onConnectedEvent");
            }
            else
            {
                client.CommunicationChannel.SendMessage(new WCMessage("Connection Accepted.", WCMessageType.CONNECTACCEPT));
                clients.Add(client.ClientID, client);
                SendMessage(clientEventArgs.Message);
            }
        }

        protected void Client_OnConnectionDenyEvent(Object sender, WCServerClientEventArgs clientEventArgs)
        {
            WCServerClient client = null;
            pendingClients.TryGetValue(clientEventArgs.ClientID, out client);
            if (client == null)
            {
                //TODO: throw expection here
                Debug.WriteLine("Failed to find clientID in pendingClients during Client_OnConnectionDenyEvent");
            }
            else
            {
                pendingClients.Remove(client.ClientID);
                client.CommunicationChannel.SendMessage(clientEventArgs.Message);
                client.CommunicationChannel.Disconnect(false);
            }
        }

        protected void Client_OnDisconnectEvent(Object sender, WCServerClientEventArgs clientEventArgs)
        {
            if (clients.ContainsKey(clientEventArgs.ClientID))
            {
                clients.Remove(clientEventArgs.ClientID);
                SendMessage(clientEventArgs.Message);
            }
            else
            {
                pendingClients.Remove(clientEventArgs.ClientID);
            }
        }
    }
}
