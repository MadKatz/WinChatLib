using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinChatNet.Messages;
using WinChatNet.Messenger;

namespace WinChatNet.Server
{
    public interface IWCServer
    {
        void Start();
        void Stop(String shutDownMessage);
        void SendMessage(IWCMessage message);
    }
}
