using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinChatNet.NetworkAdapter;
using WinChatNet.Messages;
using WinChatNet.Channels;

namespace WinChatNet.Server
{
    public abstract class ServerBase : IWCServer
    {

        public event EventHandler<WCMessageEventArg> MessageRecieved;

        public event EventHandler<WCMessageEventArg> MessageSent;

        public DateTime LastMessageSentDateStamp { get; set; }
        public DateTime LastMessageRecievedDateStamp { get; set; }
        public INetworkAdapter WireProtocol { get; set; }
        public bool Running { get; private set; }

        public ServerBase()
        {
            LastMessageRecievedDateStamp = DateTime.MinValue;
            LastMessageSentDateStamp = DateTime.MinValue;
            Running = false;
        }

        public abstract void Start();

        public abstract void Stop();

        public abstract void SendMessage(WCMessage message);
    }
}
