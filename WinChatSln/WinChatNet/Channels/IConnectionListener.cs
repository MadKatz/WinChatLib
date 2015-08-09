using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinChatNet.Channels
{
    public interface IConnectionListener
    {
        event EventHandler<CommunicationChannelEventArg> CommunicationChannelConnected;

        void Start();
        void Stop();
    }
}
