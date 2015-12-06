using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinChatNet.NetworkAdapter;
using WinChatNet.Messages;

namespace WinChatNet.Messenger
{
    public interface IMessenger
    {
        event EventHandler<WCMessageEventArg> MessageRecieved;
        event EventHandler<WCMessageEventArg> MessageSent;

        DateTime LastMessageSentDateStamp { get; set; }
        DateTime LastMessageRecievedDateStamp { get; set; }
        INetworkAdapter WireProtocol { get; set; }

        void SendMessage(IWCMessage message);
    }
}
