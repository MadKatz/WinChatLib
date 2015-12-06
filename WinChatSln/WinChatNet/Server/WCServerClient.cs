using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinChatNet.Channels;
using WinChatNet.Messages;

namespace WinChatNet.Server
{
    public class WCServerClient
    {
        public event EventHandler<WCServerClientEventArgs> Connected;
        public event EventHandler<WCServerClientEventArgs> ConnectionDenied;
        public event EventHandler<WCServerClientEventArgs> Disconnected;
        public event EventHandler<WCMessageEventArg> MessageRecieved;

        public ICommunicationChannel CommunicationChannel { get; protected set; }
        public Guid ClientID { get; private set; }
        public String Username { get; set; }

        protected Boolean pendingAuth;

        public WCServerClient(ICommunicationChannel comChannel, String username)
        {
            CommunicationChannel = comChannel;
            Username = username;
            ClientID = Guid.NewGuid();
            CommunicationChannel.MessageRecieved += new EventHandler<WCMessageEventArg>(ComChannel_OnMessageReceivedEvent);
            CommunicationChannel.Disconnected += new EventHandler(ComChannel_OnDisconnectEvent);
            pendingAuth = true;
        }

        public void Start()
        {
            pendingAuth = true;
            CommunicationChannel.Start();
        }

        public override bool Equals(object obj)
        {
            if (obj is WCServerClient)
            {
                WCServerClient WCSCObj = (WCServerClient)obj;
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

        protected void ComChannel_OnMessageReceivedEvent(Object sender, WCMessageEventArg messageEventArgs)
        {
            HandleMessage(messageEventArgs.Message);
        }

        protected void ComChannel_OnDisconnectEvent(Object sender, EventArgs eventArgs)
        {
            SendDisconnectEvent(new WCMessage("Server Error", WCMessageType.DISCONNECT));
        }

        protected void HandleMessage(IWCMessage message)
        {
            switch (message.MessageType)
            {
                case WCMessageType.CONNECTREQUEST:
                    HandleConnectionRequest(message);
                    break;
                case WCMessageType.CONNECTACCEPT:
                    break;
                case WCMessageType.CONNECTDENY:
                    break;
                case WCMessageType.MESSAGE:
                    SendMessageEvent(message);
                    break;
                case WCMessageType.SERVER:
                    break;
                case WCMessageType.DISCONNECT:
                    CommunicationChannel.Disconnect(false);
                    SendDisconnectEvent(message);
                    break;
                default:
                    break;
            }
        }

        protected void HandleConnectionRequest(IWCMessage message)
        {
            if (pendingAuth)
            {
                if (message.Message.Length > 0)
                {
                    Username = message.Message;
                    pendingAuth = false;
                    SendConnectionAcceptEvent();
                }
                else
                {
                    IWCMessage denyMessage = new WCMessage("Connection Refused: Username was empty.", WCMessageType.CONNECTDENY);
                    SendConnectionDenyEvent(denyMessage);
                    Debug.WriteLine("HandleConnectioRequest() failed due to message.legnth not greater then 0");
                }
            }
            else
            {
                Debug.WriteLine("HandleConnectioRequest() failed due to pendingAuth = false");
            }
        }

        protected void SendMessageEvent(IWCMessage message) {
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
            String disconnectMsg = "***" + Username + " has disconnected ( " + message + " )";
            IWCMessage discoMessage = new WCMessage(disconnectMsg, WCMessageType.SERVER);
            if (handler != null)
            {
                handler(this, new WCServerClientEventArgs(ClientID, Username, message));
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
