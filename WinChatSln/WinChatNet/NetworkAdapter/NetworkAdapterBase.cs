using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinChatNet.Messages;

namespace WinChatNet.NetworkAdapter
{
    public abstract class NetworkAdapterBase : INetworkAdapter
    {
        protected const int headersize = 4;

        public abstract byte[] GetBytes(IWCMessage message);

        public abstract IEnumerable<IWCMessage> CreateMessages(byte[] data);
    }
}
