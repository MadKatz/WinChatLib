using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinChatNet.Channels;
using WinChatNet.Messages;

namespace WinChatNet.Server
{
    public abstract class WCServerClientBase : IWCServerClient
    {
        public event EventHandler<WCServerClientEventArgs> Connected;
        public event EventHandler<WCServerClientEventArgs> ConnectionDenied;
        public event EventHandler<WCServerClientEventArgs> Disconnected;
        public event EventHandler<Messages.WCMessageEventArg> MessageRecieved;

        public ICommunicationChannel CommunicationChannel { get; protected set; }

        public Guid ClientID { get; private set; }

        public string Username { get; set; }

        public WCServerClientBase()
        {
            ClientID = Guid.NewGuid();
        }

        public abstract void Start();

        public override bool Equals(object obj)
        {
            if (obj is IWCServerClient)
            {
                IWCServerClient WCSCObj = (IWCServerClient)obj;
                if (WCSCObj.ClientID.Equals(ClientID) && WCSCObj.Username.Equals(Username))
                {
                    return true;
                }
            }
            return false;
        }

        public override int GetHashCode()
        {
            return ClientID.GetHashCode() + Username.GetHashCode();
        }

        protected void SendMessageEvent(IWCMessage message)
        {
            EventHandler<WCMessageEventArg> handler = MessageRecieved;
            String msgWithUsername = "<" + Username + "> " + message.Message;
            WCMessageEventArg eventMessage = new WCMessageEventArg(new WCMessage(msgWithUsername, WCMessageType.MESSAGE));
            if (handler != null)
            {
                handler(this, eventMessage);
            }
        }

        protected void SendConnectionAcceptEvent()
        {
            EventHandler<WCServerClientEventArgs> handler = Connected;
            String connectMsg = "***" + Username + " has connected.";
            IWCMessage message = new WCMessage(connectMsg, WCMessageType.SERVER);
            if (handler != null)
            {
                handler(this, new WCServerClientEventArgs(ClientID, Username, message));
            }
        }

        protected void SendDisconnectEvent(IWCMessage message)
        {
            EventHandler<WCServerClientEventArgs> handler = Disconnected;
            String disconnectMsg = "***" + Username + " has disconnected ( " + message.Message + " )";
            IWCMessage discoMessage = new WCMessage(disconnectMsg, WCMessageType.SERVER);
            if (handler != null)
            {
                handler(this, new WCServerClientEventArgs(ClientID, Username, discoMessage));
            }
        }

        protected void SendConnectionDenyEvent(IWCMessage message)
        {
            EventHandler<WCServerClientEventArgs> handler = ConnectionDenied;
            if (handler != null)
            {
                handler(this, new WCServerClientEventArgs(ClientID, Username, message));
            }
        }
    }
}
