using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using WinChatNet.Messages;

namespace WinChatNet.Client
{
    public abstract class WCClientBase : IWCClient
    {
        public event EventHandler<WCMessageEventArg> MessageRecieved;
        public event EventHandler<WCMessageEventArg> Connected;
        public event EventHandler<WCMessageEventArg> Disconnected;
        public IPAddress IP { get; set; }
        public int Port { get; set; }
        public String Username { get; set; }

        public abstract void Connect(IPAddress ip, int port, String username);

        public abstract void Disconnect(IWCMessage message);

        protected void SendMessageReceivedEvent(IWCMessage message)
        {
            EventHandler<WCMessageEventArg> handler = MessageRecieved;
            if (handler != null)
            {
                handler(this, new WCMessageEventArg(message));
            }
        }

        protected void SendConnectedEvent(IWCMessage message)
        {
            EventHandler<WCMessageEventArg> handler = Connected;
            if (handler != null)
            {
                handler(this, new WCMessageEventArg(message));
            }
        }

        protected void SendDisconnectedEvent(IWCMessage message)
        {
            EventHandler<WCMessageEventArg> handler = Disconnected;
            if (handler != null)
            {
                handler(this, new WCMessageEventArg(message));
            }
        }
    }
}
