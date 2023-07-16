using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LanTestProgram
{
    internal class Program
    {
        // метод работы клиента
        static void RunClient(string serverIpAddrStr, int serverPort)
        {
            Socket client = null;
            try
            {
                // 1. создать сокет клиента
                client = new Socket(AddressFamily.InterNetwork,
                        SocketType.Stream, ProtocolType.IP);
                Console.WriteLine("клиент: создан сокет клиента");

                // 2. подключится к серверу
                IPAddress serverIp = IPAddress.Parse(serverIpAddrStr);
                IPEndPoint serverEndPoint = new IPEndPoint(serverIp, serverPort);
                client.Connect(serverEndPoint); // подключение к серверу
                Console.WriteLine($"клиент: {client.LocalEndPoint} подключился к {client.RemoteEndPoint}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"клиент: что-то пошло не так: {ex.Message}");
            }
            finally
            {
                client?.Close();
            }
        }

        // метод работы сервера
        static void RunServer(string serverIpAddrStr, int serverPort)
        {
            Socket server = null; // сокет сервера
            Socket remoteClient = null; // сокет клиента
            try
            {
                // 1. Создать сокет
                server = new Socket(AddressFamily.InterNetwork,
                    SocketType.Stream, ProtocolType.IP);
                Console.WriteLine("сервер: создан сокет сервера");

                // 2. Связать сокет с endpoint-ом на хосте (ip:порт)
                IPAddress serverIp = IPAddress.Parse(serverIpAddrStr);
                IPEndPoint serverEndPoint = new IPEndPoint(serverIp, serverPort);
                server.Bind(serverEndPoint);
                Console.WriteLine($"сервер: сервер привязан к endpoint {serverEndPoint}");

                // 3. Перевести сокет  в слущающий режим
                server.Listen(1);
                Console.WriteLine("сервер: сервер переведен в режим ожидания");

                // 4. ожидание входящего подключения
                Console.WriteLine("сервер: ожидание входящего подключения ...");
                remoteClient = server.Accept();    // блокирующая операция
                Console.WriteLine($"сервер: подключен клиент {remoteClient.RemoteEndPoint}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"сервер: что-то пошло не так: {ex.Message}");
            }
            finally
            {
                server?.Close();    // сокет закрывается в любом случае (если не пустой)
                remoteClient?.Close();
            }
        }

        static void Main(string[] args)
        {
            string serverIpAddrStr = "192.168.88.126";
            int port = 1024;
            //
            Thread serverThread = new Thread(() => RunServer(serverIpAddrStr, port));
            //Thread clientThread = new Thread(() => RunClient(serverIpAddrStr, port));
            //
            serverThread.Start();
            //clientThread.Start();
            // 
            serverThread.Join();
            //clientThread.Join();
        }
    }
}
