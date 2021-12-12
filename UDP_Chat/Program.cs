using System;
using System.Text;
using System.Threading.Tasks;
using UDP_Chat_Multicast_Lib;

namespace UDP_Chat
{
    class Program
    {
        private static string multicast_ip;
        private static int port;
        private static string user_name;
        static async Task Main()
        {
            Console.Write("Введите ip-адрес рассылки: ");
            multicast_ip = Console.ReadLine();
            Console.Write("Введите порт: ");
            port = Convert.ToInt32(Console.ReadLine());
            Console.Write("Введите имя: ");
            user_name = Console.ReadLine();

            await Task.Run(Receive);

            await Send();
        }

        static async Task Send()
        {
            while (true)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write("Введите сообщение: ");
                var message = Console.ReadLine();
                Console.ResetColor();

                var data = Encoding.Unicode.GetBytes($"{user_name}|{message}");
                await Udp.SendAsync(data, multicast_ip, port);
            }
        }

        static async Task Receive()
        {
            while (true)
            {
                var result = await Udp.ReceiveMulticastAsync(port, multicast_ip);
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