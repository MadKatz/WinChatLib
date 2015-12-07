using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinChatNet.Server
{
    public interface IWCServer
    {
        void Start();
        void Stop(String shutDownMessage);
        void SendServerMessage(String text);
    }
}
