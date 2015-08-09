using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinChatNet.Messages
{
    [SerializableAttribute]
    public class WCTextMessage : WCMessage
    {
        public string Text { get; set; }
        //TODO: switch out for command object
        public WCMessageType MessageType { get; set; }

        public WCTextMessage()
        { }

        public WCTextMessage(WCMessageType messageType)
        {
            MessageType = messageType;
        }

        public WCTextMessage(string text)
        {
            Text = text;
        }

        public WCTextMessage(string text, WCMessageType messageType) : this(messageType)
        {
            Text = text;
        }

        public WCTextMessage(string text, string repliedmessageID)
            : this(text)
        {
            RepliedMessageID = repliedmessageID;
        }

        public WCTextMessage(string text, string repliedmessageID, WCMessageType messageType)
            : this(text, messageType)
        {
            RepliedMessageID = repliedmessageID;
        }
    }
}
