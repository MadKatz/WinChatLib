using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using WinChatNet.Messages;
using WinChatNet.NetworkAdapter;

namespace WinChatNet.NetworkAdapter.BinaryNetworkAdapter
{
    public class BinaryNetworkAdapter : NetworkAdapterBase
    {
        public override byte[] GetBytes(IWCMessage message)
        {
            if (message == null)
            {
                return null;
            }
            byte[] data = Serialize(message);
            byte[] message_length = new byte[headersize];
            BitConverter.GetBytes(data.Length).CopyTo(message_length, 0);
            byte[] result = new byte[data.Length + headersize];
            message_length.CopyTo(result, 0);
            data.CopyTo(result, headersize);
            return result;
        }

        public override IEnumerable<IWCMessage> CreateMessages(byte[] data)
        {
            List<IWCMessage> messages = new List<IWCMessage>();
            if (data == null)
            {
                return messages;
            }
            if (data.Length < 4)
            {
                throw new Exception("data length was less then header size");
            }
            int data_read = 0;
            int data_length = data.Length;
            while (data_read < data_length)
            {
                if (data_read + headersize >= data_length)
                {
                    throw new Exception("data_read + headersize is greater then or equal to data_length");
                }
                byte[] header = new byte[headersize];
                Array.Copy(data, data_read, header, 0, headersize);
                int msg_length = BitConverter.ToInt32(header, 0);
                data_read += headersize;
                if (msg_length > 0)
                {
                    byte[] msg = new byte[msg_length];
                    Array.Copy(data, data_read, msg, 0, msg_length);
                    data_read += msg_length;
                    messages.Add(Deserialize(msg));
                }
            }
            return messages;
        }

        protected byte[] Serialize(object anySerializableObject)
        {
            using (var memoryStream = new MemoryStream())
            {
                (new BinaryFormatter()).Serialize(memoryStream, anySerializableObject);
                return memoryStream.ToArray();
            }
        }

        protected IWCMessage Deserialize(byte[] data)
        {
            using (var memoryStream = new MemoryStream(data))
                return (IWCMessage)(new BinaryFormatter()).Deserialize(memoryStream);
        }
    }
}
