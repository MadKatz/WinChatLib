using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinChatNet.Messages
{
    [SerializableAttribute]
    public class WCMessageEventArg : EventArgs
    {
        public IWCMessage Message { get; set; }

        public WCMessageEventArg(IWCMessage message)
        {
            Message = message;
        }
    }
}
