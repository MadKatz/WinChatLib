using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinChatNet.Messenger;
using System.Net.Sockets;

namespace WinChatNet.Channels
{
    public interface ICommunicationChannel : IMessenger
    {
        event EventHandler Disconnected;

        CommunicationState CommunicationState { get; set; }

        void Disconnect(Boolean triggerEvent);
        void Start();
    }
}
