﻿using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Network
{
    internal class Program
    {
        static void Main(string[] args)
        {
            CancellationTokenSource cts = new CancellationTokenSource();
            Server("Hello", cts.Token);
            Console.WriteLine("Нажмите любую клавишу для завершения работы сервера");
            Console.ReadKey();
            cts.Cancel();
        }
        public static void Server(string name, CancellationToken token)
        {
            UdpClient udpClient = new UdpClient(12345);
            IPEndPoint iPEndPoint = new IPEndPoint(IPAddress.Any, 0);
        {
            UdpClient udpClient = new UdpClient(12345);
            IPEndPoint iPEndPoint = new IPEndPoint(IPAddress.Any, 0);
            Console.WriteLine("Ждем сообщение от клиента. Для завершения нажмите любую клавишу");
            while (!token.IsCancellationRequested)
            {
                 byte[] buffer;
                try
                {
                    buffer = udpClient.Receive(ref iPEndPoint);
                }
                catch (SocketException ex) when (ex.SocketErrorCode == SocketError.Interrupted)
                {
                    Console.WriteLine("Работа сервера была прервана");
                    return;
                }

                var messageText = Encoding.UTF8.GetString(buffer);
                Task.Run(() =>
                {
                    Message message = Message.DesirializeFromJson(messageText);
                    message.Print();

                    byte[] reply = Encoding.UTF8.GetBytes("Сообщение получено");
                    udpClient.Send(reply, reply.Length, iPEndPoint);
                    }, token);
            }
        }
    }
}
