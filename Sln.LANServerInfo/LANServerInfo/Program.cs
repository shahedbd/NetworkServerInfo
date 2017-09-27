
using System;
using System.Net;
namespace LANServerInfo
{
    class Program
    {
        //DESKTOP-3KU7CQB
        static string MyIPAddress = "192.168.1.111";
        static string MyMACAddress = "00155D72E9D2";

        static string MyIPAddressLapTop = "192.168.1.108";

        static string IPGoogle = "google.com";
        static string IPGoogle2 = "216.58.203.238";

        static string IPAspnet = "bing.com";
        static string IPAspnet2 = "204.79.197.200";

        static string MyIPAddressSJB = "192.168.1.101";

        static string UserName = "Prodev";
        static string PassWord = "dev123456";

        static void Main(string[] args)
        {
            IPAddress ip = IPAddress.Parse(MyIPAddress);

            //ServerDetails.GetServerInfoByIPAddress("devstation");

            //StorageDetails.GetDriveInfoBytes2(MyIPAddressLapTop);
            StorageDetails.RAMTotSize(MyIPAddressLapTop);

            //TestCode.ConnectingToWMI(MyIPAddressLapTop);

            //var abc = PortDetails.GetNetStatPorts();

            //TestCode.ServiceRunning();

            //TestCode.DiskUsageForNetworkComputer(MyIPAddressLapTop);

            //TestCode.remoteConnection(MyIPAddressLapTop, UserName, PassWord);

            //ServerDetails.GetServerInfoByIPAddress(MyIPAddressLapTop);

            //var abc = TestCode.GetMacAddress3(MyIPAddress);


            //Helper.GetServerPingStatus(MyIPAddress);

            //Helper.GetServerInfoByIPAddress(MyIPAddressLapTop);

            //Helper.GetMACAddressByIP(MyIPAddress);

            //Helper.GetMACAddress();

            //Helper.FreeSpaceNetwork2(MyIPAddress);
            //Helper.FreeSpaceNetwork(MyIPAddress);

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
