using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using WinChatNet.Channels;
using WinChatNet.Messages;
using WinChatNet.NetworkAdapter;
using WinChatNet.NetworkAdapter.BinaryNetworkAdapter;
using WinChatNet.Socket.Tcp;

namespace WinChatNet.Channels.Tcp
{
    public class TcpCommunicationChannel : CommunicationChannelBase
    {
        private const int ReceiveBufferSize = 4 * 1024;
        private byte[] buffer;
        private bool running;
        protected object communication_lock;
        public TcpCommunicationChannel(TcpClient tcp_client)
        {
            WCSocket = new TcpSocket(tcp_client);
            buffer = new byte[ReceiveBufferSize];
            communication_lock = new Object();
            WireProtocol = new BinaryNetworkAdapter();
        }
        public override void SendMessage(WCMessage message)
        {
            if (message == null)
            {
                return;
            }
            byte[] data = null;
            try
            {
                data = WireProtocol.GetBytes(message);
            }
            catch (Exception)
            {
                
                throw;
            }
            int total_sent = 0;
            lock(communication_lock)
            {
                try
                {
                    while (total_sent < data.Length)
                    {
                        int sent = ((TcpSocket)WCSocket).Tcp_client.Client.Send(data, total_sent, data.Length - total_sent, SocketFlags.None);
                        if (sent <= 0)
                        {
                            throw new Exception("Failed to send TCP message.");
                        }
                        total_sent += sent;
                        LastMessageSentDateStamp = DateTime.Now;
                        OnMessageSent(message);
                    }
                }
                catch (Exception)
                {
                    
                    throw;
                }
            }
        }

        public override void Disconnect()
        {
            if (CommunicationState == CommunicationState.Disconnected)
            {
                return;
            }
            running = false;
            if (((TcpSocket)WCSocket).Tcp_client.Connected)
            {
                try
                {
                    ((TcpSocket)WCSocket).Tcp_client.Close();
                }
                catch (Exception)
                {
                }
                ((TcpSocket)WCSocket).Tcp_client.Client.Dispose();
            }
            CommunicationState = Channels.CommunicationState.Disconnected;
            OnDisconnect();
        }

        protected override void StartCommunicationLoop()
        {
            running = true;
            try
            {
                ((TcpSocket)WCSocket).Tcp_client.Client.BeginReceive(buffer, 0, ReceiveBufferSize, 0, new AsyncCallback(CallBack), null);
            }
            catch (Exception)
            {
                Disconnect();
            }
        }

        protected void CallBack(IAsyncResult ar)
        {
            if (!running)
            {
                return;
            }
            var data_recieved = ((TcpSocket)WCSocket).Tcp_client.Client.EndReceive(ar);
            if (data_recieved < 1)
            {
                throw new Exception("failed to grab any messages. Socket closed?");
            }
            LastMessageRecievedDateStamp = DateTime.Now;
            var data = new byte[data_recieved];
            Array.Copy(buffer, data, data_recieved);
            var messages = WireProtocol.CreateMessages(data);
            foreach (var msg in messages)
            {
                OnMessageRecieved(msg);
            }
            if (running)
            {
                ((TcpSocket)WCSocket).Tcp_client.Client.BeginReceive(buffer, 0, ReceiveBufferSize, 0, new AsyncCallback(CallBack), null);
            }
        }
    }
}
