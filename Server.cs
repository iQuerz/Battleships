using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.IO;

namespace Battleships
{
    class Server
    {
        private IPAddress hostIP { get; }
        public Server(string hostIP)
        {
            this.hostIP = IPAddress.Parse(hostIP);
        }
        public string recieve()
        {
            try
            {
                TcpListener listener = new TcpListener(hostIP, 8001);
                listener.Start();
                Socket socket = listener.AcceptSocket();
                byte[] byteData = new byte[200];
                int length = socket.Receive(byteData);
                string output = "";
                for (int i = 0; i < length; i++)
                {
                    output += Convert.ToChar(byteData[i]);
                }
                return output;
            }
            catch(Exception e)
            {
                return "Error... " + e.StackTrace;
            }
        }
    }
}
