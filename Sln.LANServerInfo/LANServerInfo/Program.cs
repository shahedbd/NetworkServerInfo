
using System;
using System.Net;
namespace LANServerInfo
{
    class Program
    {
        static void Main(string[] args)
        {
            string MyIPAddress = "192.168.1.111";
            string MyIPAddressLapTop = "192.168.1.106";
            IPAddress ip = IPAddress.Parse(MyIPAddressLapTop);


            Helper.FreeSpaceNetwork(MyIPAddress);
            //Helper.FreeSpaceNetwork("DESKTOP-3KU7CQB");

            //Helper.GetDriverInfoLocal();

            //Helper.GetMachineNameFromIPAddress(MyIPAddressLapTop);


            //Helper.GetMachineNameFromIPAddress(MyIPAddress);

            //Helper.ReadFreeSpaceOnNetworkDrives("DESKTOP-3KU7CQB");

            //Helper.ShowActiveTcpConnections();

            //Helper.ShowNetworkTraffic();

            //Helper.CHKServer();

            //Helper.LocalIPAddress();

            //Helper.GetOpenPort();



            //Helper.ShowIP(MyIPAddress);

            //IPHostEntry entry5 = Dns.GetHostEntry(MyIPAddress);

            //List<string> list = new List<string>();

            //foreach (var item in entry5.AddressList)
            //{
            //    var abcd = 5;
            //}




            //If you just want to check if the network is up then use:
            //bool networkUp = System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable();

            //To check a specific interface's status (or other info) use:
            //NetworkInterface[] networkCards = NetworkInterface.GetAllNetworkInterfaces();



            //var abc2 = IPUtility.GetAvailablePort(1, 10000, ip, true);


            //var abc = Helper.PingHost(MyIPAddress);

            //var entry = System.Net.Dns.GetHostEntry("google.com"); // or vice-versa...
            //var name = System.Net.Dns.GetHostEntry(MyIPAddress); // localhost ;)


            //string machineName = Helper.GetMachineNameFromIPAddress(MyIPAddress);

            Console.ReadKey();

        }
    }
}
