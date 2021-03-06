﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinChatNet.Messages
{
    public interface IWCMessage
    {
        string MessageID { get; }
        string Message { get; set; }
        WCMessageType MessageType { get; set; }
    }
}
