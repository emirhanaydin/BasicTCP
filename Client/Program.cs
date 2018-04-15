using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Client
{
    internal static class Program
    {
        private static void Main()
        {
            var client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            Console.Write("IP Address: ");
            var ipaddrInput = Console.ReadLine();

            if (!IPAddress.TryParse(ipaddrInput ?? throw new InvalidOperationException(), out var ipaddr))
                if (ipaddrInput.Equals("localhost", StringComparison.OrdinalIgnoreCase))
                    ipaddr = IPAddress.Parse("127.0.0.1");
                else
                    throw new InvalidOperationException("IP Address is not valid.");

            Console.Write("Port number: ");
            var portInput = Console.ReadLine();

            if (!ushort.TryParse(portInput ?? throw new InvalidOperationException(), out var port))
                throw new InvalidOperationException("Port number is not valid.");

            client.Connect(ipaddr, port);

            Console.WriteLine("Connected to the server. Write \"exit\" to close.");

            while (true)
            {
                var inputCommand = Console.ReadLine();

                if (string.Equals(inputCommand, "exit", StringComparison.OrdinalIgnoreCase)) break;

                client.Send(Encoding.ASCII.GetBytes(inputCommand ?? throw new InvalidOperationException()));

                var buffReceived = new byte[128];
                var numReceived = client.Receive(buffReceived);

                Console.WriteLine("Received: " + Encoding.ASCII.GetString(buffReceived, 0, numReceived));
            }

            client.Shutdown(SocketShutdown.Both);
            client.Close();
            client.Dispose();
        }
    }
}