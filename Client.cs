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
    class Client
    {
        private string hostIP { get; }
        private string errorMsg;

        public Client(string hostIP)
        {
            this.hostIP = hostIP;
            errorMsg = "null";
        }
        public string getErrorMsg()
        {
            return errorMsg;
        }

        public void send(string data)
        {
            try
            {
                TcpClient tcpClient = new TcpClient();
                tcpClient.Connect(hostIP, 8001);
                Stream stream = tcpClient.GetStream();
                ASCIIEncoding ascii = new ASCIIEncoding();
                byte[] byteData = ascii.GetBytes(data);
                stream.Write(byteData, 0, byteData.Length);
                tcpClient.Close();
            }
            catch (Exception e)
            {
                this.send(data);
            }
        }
    }
}
