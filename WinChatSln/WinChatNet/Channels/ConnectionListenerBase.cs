using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinChatNet.Channels
{
    public abstract class ConnectionListenerBase : IConnectionListener
    {
        public event EventHandler<CommunicationChannelEventArg> CommunicationChannelConnected;

        public abstract void Start();
        public abstract void Stop();

        protected virtual void OnConnected(ICommunicationChannel communicationChannel)
        {
            EventHandler<CommunicationChannelEventArg> handler = CommunicationChannelConnected;
            if (handler != null)
            {
                handler(this, new CommunicationChannelEventArg(communicationChannel));
            }
        }
    }
}
