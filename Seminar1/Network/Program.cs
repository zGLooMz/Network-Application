﻿using System.Net.Sockets;
using System.Net;
using System.Text;

namespace Network
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Server("Hello");
        }
        public static void Server(string name)
        {
            UdpClient udpClient = new UdpClient(12345);
            IPEndPoint iPEndPoint = new IPEndPoint(IPAddress.Any, 0);
            Console.WriteLine("Ждем сообщение от клиента. Для завершения нажмите любую клавишу.");

            while (!Console.KeyAvailable)
            {
                byte[] buffer = udpClient.Receive(ref iPEndPoint);
                var messageText = Encoding.UTF8.GetString(buffer);
                ThreadPool.QueueUserWorkItem(obj =>
                {
                    Message message = Message.DesirializeFromJson(messageText);
                    message.Print();

                    byte[] reply = Encoding.UTF8.GetBytes("Сообщение получено");
                    udpClient.Send(reply, reply.Length, iPEndPoint);
                });
            }
         }
    }
}