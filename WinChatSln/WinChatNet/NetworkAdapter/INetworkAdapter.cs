using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinChatNet.Messages;

namespace WinChatNet.NetworkAdapter
{
    public interface INetworkAdapter
    {
        byte[] GetBytes(IWCMessage message);

        IEnumerable<IWCMessage> CreateMessages(byte[] data);
    }
}
