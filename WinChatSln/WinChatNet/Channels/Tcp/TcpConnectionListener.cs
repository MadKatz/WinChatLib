using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using WinChatNet.Channels;
using WinChatNet.Socket.Tcp;

namespace WinChatNet.Channels.Tcp
{
    class TcpConnectionListener : ConnectionListenerBase
    {
        protected bool _running;
        protected TcpListener listener;
        protected Task t;
        public TcpSocket Socket { get; set; }

        public TcpConnectionListener(int port)
        {
            _running = false;
            Socket.IPEndPoint.Port = port;
        }

        public override void Start()
        {
            if (_running)
            {
                return;
            }
            StartSocket();
            t = new Task(Listen);
            t.Start();
        }

        public override void Stop()
        {
            try
            {
                StopSocket();
            }
            catch (Exception)
            {

                throw;
            }
        }

        protected void StartSocket()
        {
            try
            {
                listener = new TcpListener(IPAddress.Any, Socket.IPEndPoint.Port);
                listener.Start();
                _running = true;
            }
            catch (Exception)
            {

                throw;
            }
        }

        protected void StopSocket()
        {
            listener.Stop();
            _running = false;
        }

        protected void Listen()
        {
            while (_running)
            {
                try
                {
                    TcpClient client = listener.AcceptTcpClient();
                    OnConnected(new TcpCommunicationChannel(client));
                }
                catch (Exception)
                {

                    throw;
                }
            }
        }
    }
}
