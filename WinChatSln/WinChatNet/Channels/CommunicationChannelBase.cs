using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using WinChatNet.Messages;
using WinChatNet.NetworkAdapter;
using WinChatNet.Socket;

namespace WinChatNet.Channels
{
    public abstract class CommunicationChannelBase : ICommunicationChannel
    {
        public event EventHandler Disconnected;
        public event EventHandler<WCMessageEventArg> MessageRecieved;
        public event EventHandler<WCMessageEventArg> MessageSent;

        public CommunicationState CommunicationState { get; set; }
        public DateTime LastMessageSentDateStamp { get; set; }
        public DateTime LastMessageRecievedDateStamp { get; set; }
        public INetworkAdapter WireProtocol { get; set; }
        public IWCSocket WCSocket { get; set; }

        protected CommunicationChannelBase()
        {
            CommunicationState = Channels.CommunicationState.Disconnected;
            LastMessageSentDateStamp = DateTime.MinValue;
            LastMessageRecievedDateStamp = DateTime.MinValue;
        }
        public abstract void SendMessage(WCMessage message);

        public abstract void Disconnect();

        public void Start()
        {
            StartCommunicationLoop();
            CommunicationState = Channels.CommunicationState.Connected;
        }

        protected void OnDisconnect()
        {
            EventHandler handler = Disconnected;
            if (handler != null)
            {
                handler(this, new EventArgs());
            }
        }

        protected void OnMessageRecieved(IWCMessage message)
        {
            EventHandler<WCMessageEventArg> handler = MessageRecieved;
            if (handler != null)
            {
                handler(this, new WCMessageEventArg(message));
            }
        }

        protected void OnMessageSent(IWCMessage message)
        {
            EventHandler<WCMessageEventArg> handler = MessageSent;
            if (handler != null)
            {
                handler(this, new WCMessageEventArg(message));
            }
        }

        protected abstract void StartCommunicationLoop();
    }
}
