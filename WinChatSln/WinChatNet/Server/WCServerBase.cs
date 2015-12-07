using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinChatNet.Channels;
using WinChatNet.NetworkAdapter;

namespace WinChatNet.Server
{
    public abstract class WCServerBase : IWCServer
    {
        protected IConnectionListener listener;
        public INetworkAdapter WireProtocol { get; set; }
        public bool Running { get; protected set; }

        public WCServerBase()
        {
            Running = false;
        }

        public abstract void Start();

        public abstract void Stop(String shutDownMessage);

        public abstract void SendServerMessage(String text);
    }
}
