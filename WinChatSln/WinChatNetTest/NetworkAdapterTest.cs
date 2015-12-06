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
            IWCMessage test_textmsg = new WCMessage(teststr, WCMessageType.MESSAGE);
            BinaryNetworkAdapter nAdapter = new BinaryNetworkAdapter();
            var testdata = nAdapter.GetBytes(test_textmsg);
            List<IWCMessage> messages = (List<IWCMessage>)nAdapter.CreateMessages(testdata);
            Assert.IsTrue(messages.Count == 1, "Message count is not 1");
            IWCMessage received_msg = messages[0];
            Assert.IsTrue(received_msg.Message.Equals(teststr), "message text is not equal");
            Assert.IsTrue(received_msg.MessageType == WCMessageType.MESSAGE, "message type is not equal");
        }

        [TestMethod]
        public void Test_GetBytes_MultiMessage()
        {
            string teststr1 = "test msg1";
            string teststr2 = "test msg2";
            IWCMessage test_textmsg1 = new WCMessage(teststr1, WCMessageType.MESSAGE);
            IWCMessage test_textmsg2 = new WCMessage(teststr2, WCMessageType.SERVER);
            BinaryNetworkAdapter nAdapter = new BinaryNetworkAdapter();
            var testdata1 = nAdapter.GetBytes(test_textmsg1);
            var testdata2 = nAdapter.GetBytes(test_textmsg2);
            var data = new byte[testdata1.Length + testdata2.Length];
            Array.Copy(testdata1, 0, data, 0, testdata1.Length);
            Array.Copy(testdata2, 0, data, testdata1.Length, testdata2.Length);
            List<IWCMessage> messages = (List<IWCMessage>)nAdapter.CreateMessages(data);
            Assert.IsTrue(messages.Count == 2, "Message count is not 2");
            IWCMessage received_msg1 = messages[0];
            Assert.IsTrue(received_msg1.Message.Equals(teststr1), "message text is not equal");
            Assert.IsTrue(received_msg1.MessageType == WCMessageType.MESSAGE, "message type is not equal");
            IWCMessage received_msg2 = messages[1];
            Assert.IsTrue(received_msg2.Message.Equals(teststr2), "message text is not equal");
            Assert.IsTrue(received_msg2.MessageType == WCMessageType.SERVER, "message type is not equal");
        }
    }
}
