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
using System.Diagnostics;
using System.Net;

namespace WinChatNet.Channels.Tcp
{
    public class TcpCommunicationChannel : CommunicationChannelBase
    {
        private const int ReceiveBufferSize = 4 * 1024;
        private byte[] buffer;
        private bool running;
        protected object communication_lock;
        protected IPAddress ip;
        protected int port;
        protected TcpClient client;

        public TcpCommunicationChannel(TcpClient client, IPAddress ip, int port) : base()
        {
            this.client = client;
            this.ip = ip;
            this.port = port;
            buffer = new byte[ReceiveBufferSize];
            communication_lock = new Object();
            WireProtocol = new BinaryNetworkAdapter();
        }

        public override void SendMessage(IWCMessage message)
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
                        int sent = client.Client.Send(data, total_sent, data.Length - total_sent, SocketFlags.None);
                        if (sent <= 0)
                        {
                            throw new Exception("Failed to send TCP message.");
                        }
                        total_sent += sent;
                        LastMessageSentDateStamp = DateTime.Now;
                        SendMessageSentEvent(message);
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                }
            }
        }

        public override void Disconnect(Boolean triggerEvent)
        {
            if (CommunicationState == CommunicationState.Disconnected)
            {
                return;
            }
            running = false;
            if (client.Client.Connected)
            {
                try
                {
                    client.Client.Close();
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                }
                client.Client.Dispose();
            }
            CommunicationState = Channels.CommunicationState.Disconnected;
            if (triggerEvent)
            {
                SendDisconnectEvent();
            }
        }

        protected override void StartCommunicationLoop()
        {
            running = true;
            try
            {
                client.Client.BeginReceive(buffer, 0, ReceiveBufferSize, 0, new AsyncCallback(Send_CallBack), null);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                Disconnect(true);
            }
        }

        protected void Send_CallBack(IAsyncResult ar)
        {
            if (!running)
            {
                return;
            }
            int data_recieved = 0;
            try
            {
                data_recieved = client.Client.EndReceive(ar);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                Disconnect(true);
            }
            if (data_recieved < 1)
            {
                //throw new Exception("failed to grab any messages. Socket closed?");
                return;
            }
            LastMessageRecievedDateStamp = DateTime.Now;
            var data = new byte[data_recieved];
            Array.Copy(buffer, data, data_recieved);
            var messages = WireProtocol.CreateMessages(data);
            foreach (var msg in messages)
            {
                SendMessageRecievedEvent(msg);
            }
            if (running)
            {
                try
                {
                    client.Client.BeginReceive(buffer, 0, ReceiveBufferSize, 0, new AsyncCallback(Send_CallBack), null);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                    Disconnect(true);
                }
            }
        }
    }
}
