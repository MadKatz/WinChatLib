using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;

namespace WinChatNet.Socket
{
    public abstract class WCSocket : IWCSocket
    {
        public IPEndPoint IPEndPoint { get; set; }
    }
}
