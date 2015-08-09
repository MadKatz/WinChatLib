using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinChatNet.Channels
{
    public class CommunicationChannelEventArg : EventArgs
    {
        public ICommunicationChannel CommunicationChannel { get; set; }

        public CommunicationChannelEventArg(ICommunicationChannel communicationChannel)
        {
            CommunicationChannel = communicationChannel;
        }
    }
}
