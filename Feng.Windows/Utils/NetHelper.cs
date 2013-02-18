using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;

namespace Feng.Windows.Utils
{
    public static class NetHelper
    {
        /// <summary>
        /// CheckPortOpen
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="port"></param>
        /// <returns></returns>
        public static bool CheckPortOpen(string ip, int port)
        {
            TcpClient TcpScan = new TcpClient();

            try
            {
                TcpScan.Connect(ip, port);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
