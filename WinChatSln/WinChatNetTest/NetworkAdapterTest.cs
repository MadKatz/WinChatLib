using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WinChatNet.Messages;
using WinChatNet.NetworkAdapter.BinaryNetworkAdapter;

namespace WinChatNetTest
{
    [TestClass]
    public class NetworkAdapterTest
    {
        [TestMethod]
        public void Test_GetBytes_SingleMessage()
        {
            string teststr = "test msg";
            WCTextMessage test_textmsg = new WCTextMessage(teststr, WCMessageType.MESSAGE);
            BinaryNetworkAdapter nAdapter = new BinaryNetworkAdapter();
            var testdata = nAdapter.GetBytes(test_textmsg);
            List<IWCMessage> messages = (List<IWCMessage>)nAdapter.CreateMessages(testdata);
            Assert.IsTrue(messages.Count == 1, "Message count is not 1");
            WCTextMessage received_msg = (WCTextMessage)messages[0];
            Assert.IsTrue(received_msg.Text.Equals(teststr), "message text is not equal");
            Assert.IsTrue(received_msg.MessageType == WCMessageType.MESSAGE, "message type is not equal");
        }

        [TestMethod]
        public void Test_GetBytes_MultiMessage()
        {
            string teststr1 = "test msg1";
            string teststr2 = "test msg2";
            WCTextMessage test_textmsg1 = new WCTextMessage(teststr1, WCMessageType.MESSAGE);
            WCTextMessage test_textmsg2 = new WCTextMessage(teststr2, WCMessageType.SERVER);
            BinaryNetworkAdapter nAdapter = new BinaryNetworkAdapter();
            var testdata1 = nAdapter.GetBytes(test_textmsg1);
            var testdata2 = nAdapter.GetBytes(test_textmsg2);
            var data = new byte[testdata1.Length + testdata2.Length];
            Array.Copy(testdata1, 0, data, 0, testdata1.Length);
            Array.Copy(testdata2, 0, data, testdata1.Length, testdata2.Length);
            List<IWCMessage> messages = (List<IWCMessage>)nAdapter.CreateMessages(data);
            Assert.IsTrue(messages.Count == 2, "Message count is not 2");
            WCTextMessage received_msg1 = (WCTextMessage)messages[0];
            Assert.IsTrue(received_msg1.Text.Equals(teststr1), "message text is not equal");
            Assert.IsTrue(received_msg1.MessageType == WCMessageType.MESSAGE, "message type is not equal");
            WCTextMessage received_msg2 = (WCTextMessage)messages[1];
            Assert.IsTrue(received_msg2.Text.Equals(teststr2), "message text is not equal");
            Assert.IsTrue(received_msg2.MessageType == WCMessageType.SERVER, "message type is not equal");
        }
    }
}
