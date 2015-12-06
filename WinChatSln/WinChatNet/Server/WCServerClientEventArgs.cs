using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinChatNet.Messages;

namespace WinChatNet.Server
{
    public class WCServerClientEventArgs : EventArgs
    {
        public Guid ClientID { get; set; }
        public String Username { get; set; }
        public IWCMessage Message { get; set; }

        public WCServerClientEventArgs(Guid clientID, String username, IWCMessage message)
        {
            ClientID = clientID;
            Username = username;
            Message = message;
        }
    }
}
