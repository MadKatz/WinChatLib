using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinChatNet.Channels;
using WinChatNet.Messages;

namespace WinChatNet.Server
{
    public interface IWCServerClient
    {
        event EventHandler<WCServerClientEventArgs> Connected;
        event EventHandler<WCServerClientEventArgs> ConnectionDenied;
        event EventHandler<WCServerClientEventArgs> Disconnected;
        event EventHandler<WCMessageEventArg> MessageRecieved;

        ICommunicationChannel CommunicationChannel { get; }
        Guid ClientID { get; }
        String Username { get; set; }

        void Start();
    }
}
