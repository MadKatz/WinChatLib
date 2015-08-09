using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinChatNet.Messenger;

namespace WinChatNet.Server
{
    public interface IWCServer : IMessenger
    {
        void Start();
        void Stop();
    }
}
