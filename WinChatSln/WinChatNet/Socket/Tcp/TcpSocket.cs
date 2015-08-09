using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using WinChatNet.Socket;

namespace WinChatNet.Socket.Tcp
{
    public class TcpSocket : WCSocket
    {
        public TcpClient Tcp_client { get; set; }

        public TcpSocket()
        {
        }

        public TcpSocket(TcpClient client)
        {
            Tcp_client = client;
        }
    }
}
