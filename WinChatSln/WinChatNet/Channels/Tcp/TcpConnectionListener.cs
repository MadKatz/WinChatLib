using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using WinChatNet.Channels;
using System.Diagnostics;

namespace WinChatNet.Channels.Tcp
{
    class TcpConnectionListener : ConnectionListenerBase
    {
        protected bool _running;
        protected TcpListener listener;
        protected Task t;
        public int Port { get; set; }
        public IPAddress IPToListenOn { get; set; }

        public TcpConnectionListener(IPAddress ip, int port) : base()
        {
            _running = false;
            Port = port;
            IPToListenOn = ip;
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
            catch (Exception ex)
            {

                Debug.WriteLine(ex.Message);
            }
        }

        protected void StartSocket()
        {
            listener = new TcpListener(IPToListenOn, Port);
            listener.Start();
            _running = true;
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
                    IPEndPoint ipep = (IPEndPoint)client.Client.RemoteEndPoint;
                    SendConnectedEvent(new TcpCommunicationChannel(client, ipep.Address, Port));
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                }
            }
        }
    }
}
