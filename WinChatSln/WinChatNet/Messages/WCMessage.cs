using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinChatNet.Messages
{
    [SerializableAttribute]
    public class WCMessage : IWCMessage
    {
        public string MessageID { get; private set; }
        public string Message { get; set; }
        public WCMessageType MessageType { get; set; }

        public WCMessage()
        {
            MessageID = Guid.NewGuid().ToString();
        }

        public WCMessage(string message, WCMessageType messageType) : this()
        {
            Message = message;
            MessageType = messageType;
        }
    }
}
