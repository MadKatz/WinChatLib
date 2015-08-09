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
        public string RepliedMessageID { get; set; }

        public WCMessage()
        {
            MessageID = Guid.NewGuid().ToString();
        }

        public WCMessage(string repliedmessageID) : this()
        {
            RepliedMessageID = repliedmessageID;
        }
    }
}
