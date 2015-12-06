using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using WinChatNet.Messages;

namespace WinChatNet.Client
{
    public interface IWCClient
    {
        event EventHandler<WCMessageEventArg> MessageRecieved;
        event EventHandler<WCMessageEventArg> Connected;
        event EventHandler<WCMessageEventArg> Disconnected;

        IPAddress IP { get; set; }
        int Port { get; set; }
        String Username { get; set; }

        void Connect(IPAddress ip, int port, String username);
        void Disconnect(IWCMessage message);
    }
}
