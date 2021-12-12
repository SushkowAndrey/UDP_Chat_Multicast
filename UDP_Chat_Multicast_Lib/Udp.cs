using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace UDP_Chat_Multicast_Lib
{
    public static class Udp
    {
        public static async Task SendAsync(byte[] data, string ip, int port)
        {
            using var sender = new UdpClient();
            await sender.SendAsync(data, data.Length, ip, port);
        }

        public static async Task<UdpReceiveResult> ReceiveAsync(int port)
        {
            using var receiver = new UdpClient(port);
            return await receiver.ReceiveAsync();
        }

        public static async Task<UdpReceiveResult> ReceiveMulticastAsync(int port, string multicast_ip)
        {
            using var receiver = new UdpClient(port);
            receiver.JoinMulticastGroup(IPAddress.Parse(multicast_ip));
            return await receiver.ReceiveAsync();
        }
    }
}