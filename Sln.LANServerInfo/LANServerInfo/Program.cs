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


            //ServerInfos.GetServerDetails(MyIPAddressLapTop);

            //WMIProcess.GetWMIRunningProcessName(MyIPAddress);

            Console.WriteLine("The End********************");

            //ConnectionOptions conn = new ConnectionOptions();
            //conn.Username = "Administrator";
            //conn.Password = "admin";
            //ManagementScope wmiScope = new ManagementScope(string.Format("\\\\{0}\\root\\cimv2", MyIPAddressLapTop), conn);
            //ServerInfos.SystemProcessInfo(wmiScope);


            //Helper.GetServerPingStatus(MyIPAddress);

            //List<SytemInfo> list = ServerDetails.Win32_ListItems(MyIPAddressLapTop);
            //var fileToCreate = Global.OutputDirCustom + "\\" + Global.fileNames[0];
            ////File.WriteAllLines(fileToCreate, abc, Encoding.UTF8);

            //string abc = string.Empty;
            //foreach (SytemInfo x in list)
            //{
            //    abc = abc + x.PropertyName + ": " + x.PropertyValue + "\n";
            //}

            //File.WriteAllText(fileToCreate, abc);


            //Console.WriteLine("The End********************");


            //ManagementObjectSearcher wmi = new ManagementObjectSearcher("SELECT * FROM meta_class WHERE __CLASS LIKE 'Win32_%'");
            //foreach (ManagementObject obj in wmi.Get())
            //    Console.WriteLine(obj["__CLASS"]);

            //ManagementClass osClass = new ManagementClass("Win32_OperatingSystem");
            //foreach (ManagementObject queryObj in osClass.GetInstances())
            //{
            //    foreach (PropertyData prop in queryObj.Properties)
            //    {
            //        //add these to your arraylist or dictionary 
            //        Console.WriteLine("{0}: {1}", prop.Name, prop.Value);
            //    }
            //}


            //ObjectQuery wql = new ObjectQuery("SELECT * FROM Win32_OperatingSystem");
            //ManagementObjectSearcher searcher = new ManagementObjectSearcher(wql);
            //ManagementObjectCollection results = searcher.Get();

            //foreach (ManagementObject result in results)
            //{
            //    Console.WriteLine("Total Visible Memory: {0}KB", result["TotalVisibleMemorySize"]);
            //    Console.WriteLine("Free Physical Memory: {0}KB", result["FreePhysicalMemory"]);
            //    Console.WriteLine("Total Virtual Memory: {0}KB", result["TotalVirtualMemorySize"]);
            //    Console.WriteLine("Free Virtual Memory: {0}KB", result["FreeVirtualMemory"]);
            //}


            //StorageDetails.SampleWMI(MyIPAddressLapTop);

            //ServerDetails.GetServerInfoByIPAddress("devstation");

            //StorageDetails.GetDriveInfoBytes2(MyIPAddressLapTop);

            //var abc = StorageDetails.RAMTotSize(MyIPAddressLapTop);

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
