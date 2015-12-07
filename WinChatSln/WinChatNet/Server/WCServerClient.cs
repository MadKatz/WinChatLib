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
    public class WCServerClient : WCServerClientBase
    {

        protected Boolean pendingAuth;

        public WCServerClient(ICommunicationChannel comChannel, String username) : base()
        {
            CommunicationChannel = comChannel;
            Username = username;
            CommunicationChannel.MessageRecieved += new EventHandler<WCMessageEventArg>(ComChannel_OnMessageReceivedEvent);
            CommunicationChannel.Disconnected += new EventHandler(ComChannel_OnDisconnectEvent);
            pendingAuth = true;
        }

        public override void Start()
        {
            pendingAuth = true;
            CommunicationChannel.Start();
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
    }
}
