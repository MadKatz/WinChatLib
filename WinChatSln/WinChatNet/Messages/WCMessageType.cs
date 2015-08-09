using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinChatNet.Messages
{
    [SerializableAttribute]
    public enum WCMessageType
    {
        CONNECTREQUEST,
        CONNECTACCEPT,
        CONNECTDENY,
        MESSAGE,
        SERVER,
    }
}
