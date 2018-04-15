using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Server
{
    internal static class Program
    {
        private static void Main()
        {
            var listenerSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            var ipaddr = IPAddress.Any;
            var ipep = new IPEndPoint(ipaddr, 23000);

            listenerSocket.Bind(ipep);
            listenerSocket.Listen(5);
            Console.WriteLine("Waiting client...");
            var client = listenerSocket.Accept();
            Console.WriteLine("Client has connected: {0} (IP End Point: {1})", client, client.RemoteEndPoint);

            var buff = new byte[128];
            while (true)
            {
                int numBytes;
                try
                {
                    numBytes = client.Receive(buff);
                }
                catch (SocketException)
                {
                    break;
                }

                Console.WriteLine("Number of received bytes: " + numBytes);

                var receivedText = Encoding.ASCII.GetString(buff, 0, numBytes);
                if (receivedText.Equals("exit", StringComparison.OrdinalIgnoreCase)) break;

                Console.WriteLine("Received data: " + receivedText);

                client.Send(buff);

                Array.Clear(buff, 0, buff.Length);
            }

            client.Close();
            client.Dispose();
        }
    }
}