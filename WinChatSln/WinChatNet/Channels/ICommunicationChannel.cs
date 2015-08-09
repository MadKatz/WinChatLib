using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinChatNet.Messenger;
using WinChatNet.Socket;
using System.Net.Sockets;

namespace WinChatNet.Channels
{
    public interface ICommunicationChannel : IMessenger
    {
        event EventHandler Disconnected;

        CommunicationState CommunicationState { get; set; }
        IWCSocket WCSocket { get; set; }

        void Disconnect();
        void Start();
    }
}
