﻿using Network;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Client
{
    public class Program
    {
        static void Main(string[] args)
        {
            SentMessage("Alex");
        }

        public static void SentMessage(string From, string ip = "127.0.0.1")
        {
            UdpClient udpClient = new UdpClient();
            IPEndPoint iPEndPoint = new IPEndPoint(IPAddress.Parse(ip), 12345);
            while (true)
            {
                Console.Write("Введите сообщение (или 'Exit' для выхода): ");
                string input = Console.ReadLine();

                if (input.ToLower() == "exit")
                {
                    break;
                }
                Message message = new Message() { Text = input, DateTime = DateTime.Now, NiknameFrom = From, NiknameTo = "All" };
                string json = message.SerializeMessageToJson();

                byte[] data = Encoding.UTF8.GetBytes(json);
                udpClient.Send(data, data.Length, iPEndPoint);
                byte[] buffer = udpClient.Receive(ref iPEndPoint);
                var answer = Encoding.UTF8.GetString(buffer);
                Console.WriteLine(answer);
            }
        }
    }
}