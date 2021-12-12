using System;
using System.Text;
using System.Threading.Tasks;
using UDP_Chat_Multicast_Lib;

namespace UDP_Chat
{
    internal static class Program
    {
        private static string _multicastIp;
        private static int _port;
        private static string _userName;

        private static async Task Main()
        {
            Console.Write("Введите ip-адрес рассылки: ");
            _multicastIp = Console.ReadLine();
            Console.Write("Введите порт: ");
            _port = Convert.ToInt32(Console.ReadLine());
            Console.Write("Введите имя: ");
            _userName = Console.ReadLine();

            await Task.Run(Receive);

            await Send();
        }

        private static async Task Send()
        {
            while (true)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write("Введите сообщение: ");
                var message = Console.ReadLine();
                Console.ResetColor();

                var data = Encoding.Unicode.GetBytes($"{_userName}|{message}");
                await Udp.SendAsync(data, _multicastIp, _port);
            }
        }

        private static async Task Receive()
        {
            while (true)
            {
                var result = await Udp.ReceiveMulticastAsync(_port, _multicastIp);
                var data = result.Buffer;
                var temp = Encoding.Unicode.GetString(data);
                var t = temp.Split('|');
                var user = t[0];
                var message = t[1];
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.Write($"{user} -> ");
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(message);
                Console.ResetColor();
            }
        }
    }
}