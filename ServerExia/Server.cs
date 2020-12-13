using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace ServerExia
{
    class Server
    {

        static void Main(string[] args)
        {
            Socket Myserver = SeConnecter();
            Socket ClientHandler = AccepterConnection(Myserver);
            EcouterReseau(ClientHandler);
            Deconnecter(ClientHandler);
        }

        private static Socket SeConnecter()
        {
            IPHostEntry ipHost = Dns.GetHostEntry(Dns.GetHostName());
            IPAddress ipAddr = ipHost.AddressList[0];
            IPEndPoint localEndPoint = new IPEndPoint(ipAddr, 11111);

            Socket listener = new Socket(ipAddr.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            try
            {
                listener.Bind(localEndPoint);
                listener.Listen(10);

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

            return listener; 
        }

        private static Socket AccepterConnection(Socket socket)
        {


            Socket clientSocket = socket;

            try
            {
                Console.WriteLine("Attente Connexion  ... ");
                clientSocket.Accept();
            }
            catch (Exception e) 
            {
                Console.WriteLine(e.ToString());
            }

            return clientSocket; 

        }


        private static void EcouterReseau(Socket socket)
        {
            while (true)
            {
                byte[] bytes = new Byte[1024];
                string data = null;

                while (true)
                {
                    int numByte = socket.Receive(bytes);

                    data += Encoding.ASCII.GetString(bytes, 0, numByte);

                    if (data.IndexOf("<EOF>") > -1)
                        break;
                }

                Console.WriteLine("Message Recu -> {0} ", data);

            }
        }


        private static void Deconnecter(Socket socket)
        {
            byte[] message = Encoding.ASCII.GetBytes("Test Server");
            socket.Send(message);
            socket.Shutdown(SocketShutdown.Both);
            socket.Close();
        }


    }
}
