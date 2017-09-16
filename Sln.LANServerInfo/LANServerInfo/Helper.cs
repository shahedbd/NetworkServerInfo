using NativeWifi;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Management;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading;




namespace LANServerInfo
{
    public static class Helper
    {
        public static string GetMachineNameFromIPAddress(string ipAdress)
        {
            string machineName = string.Empty;
            try
            {
                IPHostEntry hostEntry = Dns.GetHostEntry(ipAdress);
                machineName = hostEntry.HostName;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return machineName;
        }


        public static void TrafficCount()
        {
            Console.WriteLine("Active TCP Connections");
            IPGlobalProperties properties = IPGlobalProperties.GetIPGlobalProperties();
            TcpConnectionInformation[] connections = properties.GetActiveTcpConnections();

            //Port 80 is the standard HTTP port, and port 443 is the standard HTTPS port. You might want to filter for both.
            foreach (TcpConnectionInformation c in connections)
            {
                if ((c.RemoteEndPoint.Port == 80) || (c.RemoteEndPoint.Port == 443))
                {
                    Console.WriteLine("{0} <==> {1}:{2}",
                                      c.LocalEndPoint.ToString(),
                                      c.RemoteEndPoint.ToString(),
                                      c.RemoteEndPoint.Port);
                }
            }



            //IPHostEntry entry = Dns.GetHostEntry(hostNameOrAddress: "www.google.com");
            //foreach (IPAddress addr in entry.AddressList)
            //{
            //    // connect, on sucess call 'break'
            //}

            //foreach (IPAddress ip in Dns.GetHostAddresses("www.google.com"))
            //{
            //    Console.WriteLine(ip.ToString());
            //}

        }


        public static void SocketTest()
        {
            Socket s = null;

            IPEndPoint remoteIpEndPoint = s.RemoteEndPoint as IPEndPoint;
            IPEndPoint localIpEndPoint = s.LocalEndPoint as IPEndPoint;

            if (remoteIpEndPoint != null)
            {
                // Using the RemoteEndPoint property.
                Console.WriteLine("I am connected to " + remoteIpEndPoint.Address + "on port number " + remoteIpEndPoint.Port);
            }

            if (localIpEndPoint != null)
            {
                // Using the LocalEndPoint property.
                Console.WriteLine("My local IpAddress is :" + localIpEndPoint.Address + "I am connected on port number " + localIpEndPoint.Port);
            }
        }



        //Need it
        public static void CHKServer()
        {
            string MyIPAddress = "192.168.1.111";
            string MyIPAddressLapTop = "192.168.1.105";
            string GoogleIPAddress = "172.217.24.110";


            var ping = new Ping();

            //var Googlereply = ping.Send(GoogleIPAddress, 60 * 1000);

            var reply = ping.Send(MyIPAddressLapTop, 60 * 1000); // 1 minute time out (in ms)
            // or...
            reply = ping.Send(new IPAddress(new byte[] { 192, 168, 1, 105 }), 3000);
        }

        public static string LocalIPAddress()
        {
            IPHostEntry host;
            List<string> foundLocals = new List<string>();

            host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (IPAddress ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                    foundLocals.Add(ip.ToString());
            }

            return null;
        }

        public static string GetOpenPort()
        {
            //TcpClient c;
            //I want to check here if port is free.
            //c = new TcpClient(ip, port);


            int PortStartIndex = 1000;
            int PortEndIndex = 2000;
            IPGlobalProperties properties = IPGlobalProperties.GetIPGlobalProperties();
            IPEndPoint[] tcpEndPoints = properties.GetActiveTcpListeners();

            List<int> usedPorts = tcpEndPoints.Select(p => p.Port).ToList<int>();
            int unusedPort = 0;

            for (int port = PortStartIndex; port < PortEndIndex; port++)
            {
                if (!usedPorts.Contains(port))
                {
                    unusedPort = port;
                    break;
                }
            }
            return unusedPort.ToString();
        }



        public static void ShowIP(string ipAdress)
        {
            //string name = Dns.GetHostName();
            try
            {
                IPAddress[] addrs = Dns.Resolve(ipAdress).AddressList;
                foreach (IPAddress addr in addrs)
                    Console.WriteLine("{0}/{1}", ipAdress, addr);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }


        //Need  it
        public static string GetServerInfoByIPAddress(string ipAdress)
        {
            string machineName = string.Empty;
            try
            {
                IPHostEntry hostEntry = Dns.GetHostEntry(ipAdress);

                machineName = hostEntry.HostName;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return machineName;
        }

        //WLAN IP get
        public static string NetworkGateway()
        {
            string ip = null;

            foreach (NetworkInterface f in NetworkInterface.GetAllNetworkInterfaces())
            {
                if (f.OperationalStatus == OperationalStatus.Up)
                {
                    foreach (GatewayIPAddressInformation d in f.GetIPProperties().GatewayAddresses)
                    {
                        ip = d.Address.ToString();
                    }
                }
            }

            return ip;
        }


        public static void GetWLANIPList()
        {
            var abc = Dns.GetHostName();

            //00
            var addresses0 = Dns.GetHostEntry((Dns.GetHostName()))
                    .AddressList
                    .Where(x => x.AddressFamily == AddressFamily.InterNetwork)
                    .Select(x => x.ToString())
                    .ToArray();



            //01
            var addresses1 = Dns.GetHostEntry((Dns.GetHostName()))
                    .AddressList
                    .Where(x => x.AddressFamily == AddressFamily.InterNetwork)
                    .Select(x => x.ToString())
                    .ToArray();


            //02
            var addresses2 = Dns.GetHostEntryAsync((Dns.GetHostName()))
                .Result
                .AddressList
                .Where(x => x.AddressFamily == AddressFamily.InterNetwork)
                .Select(x => x.ToString())
                .ToArray();


            //03
            string[] strIP = null;
            int count = 0;

            IPHostEntry HostEntry = Dns.GetHostEntry((Dns.GetHostName()));
            if (HostEntry.AddressList.Length > 0)
            {
                strIP = new string[HostEntry.AddressList.Length];
                foreach (IPAddress ip in HostEntry.AddressList)
                {
                    if (ip.AddressFamily == AddressFamily.InterNetwork)
                    {
                        strIP[count] = ip.ToString();
                        count++;
                    }
                }
            }
        }


        public static void WLANClient()
        {
            WlanClient client = new WlanClient();
            foreach (WlanClient.WlanInterface wlanIface in client.Interfaces)
            {
                // Lists all available networks
                Wlan.WlanAvailableNetwork[] networks = wlanIface.GetAvailableNetworkList(0);
                foreach (Wlan.WlanAvailableNetwork network in networks)
                {
                    Console.WriteLine("Found network with SSID {0}.", GetStringForSSID(network.dot11Ssid));
                }
            }
        }


        static string GetStringForSSID(Wlan.Dot11Ssid ssid)
        {
            return Encoding.ASCII.GetString(ssid.SSID, 0, (int)ssid.SSIDLength);
        }


        public static bool PingHost(string nameOrAddress)
        {
            bool pingable = false;
            Ping pinger = new Ping();
            try
            {
                PingReply reply = pinger.Send(nameOrAddress);
                pingable = reply.Status == IPStatus.Success;
            }
            catch (PingException ex)
            {
                throw ex;
            }
            return pingable;
        }


        public static void ShowActiveTcpConnections()
        {
            Console.WriteLine("Active TCP Connections");
            IPGlobalProperties properties = IPGlobalProperties.GetIPGlobalProperties();

            //string MyIPAddress = "192.168.43.162";
            //string GoogleIPAddress = "172.217.24.110";

            //var abc = properties.HostName;


            //IPGlobalProperties ipProps = IPGlobalProperties.GetIPGlobalProperties();
            //List<ushort> excludedPorts = new List<ushort>();

            //excludedPorts.AddRange(from n in ipProps.GetActiveTcpConnections()
            //                       where n.LocalEndPoint.Address.Equals("192.168.43.162")
            //                       select (ushort)n);


            TcpConnectionInformation[] connections = properties.GetActiveTcpConnections();
            foreach (TcpConnectionInformation c in connections)
            {
                Console.WriteLine("{0} <==> {1}", c.LocalEndPoint.ToString(), c.RemoteEndPoint.ToString());
            }
        }

        public static void ShowNetworkTraffic()
        {
            PerformanceCounterCategory performanceCounterCategory = new PerformanceCounterCategory("Network Interface");
            string instance = performanceCounterCategory.GetInstanceNames()[0]; // 1st NIC !
            PerformanceCounter performanceCounterSent = new PerformanceCounter("Network Interface", "Bytes Sent/sec", instance);
            PerformanceCounter performanceCounterReceived = new PerformanceCounter("Network Interface", "Bytes Received/sec", instance);

            for (int i = 0; i < 10; i++)
            {
                Console.WriteLine("bytes sent: {0}k\tbytes received: {1}k", performanceCounterSent.NextValue() / 1024, performanceCounterReceived.NextValue() / 1024);
                Thread.Sleep(500);
            }
        }



        public static void StorageInfo()
        {
            ManagementPath path = new ManagementPath()
            {
                NamespacePath = @"root\cimv2",
                //Server = "<REMOTE HOST OR IP>"
                Server = "<192.168.1.111>"
            };
            ManagementScope scope = new ManagementScope(path);
            string condition = "DriveLetter = 'C:'";
            string[] selectedProperties = new string[] { "FreeSpace" };
            SelectQuery query = new SelectQuery("Win32_Volume", condition, selectedProperties);

            using (ManagementObjectSearcher searcher = new ManagementObjectSearcher(scope, query))
            using (ManagementObjectCollection results = searcher.Get())
            {
                ManagementObject volume = results.Cast<ManagementObject>().SingleOrDefault();

                if (volume != null)
                {
                    ulong freeSpace = (ulong)volume.GetPropertyValue("FreeSpace");

                    // Use freeSpace here...
                }
            }
        }


        public static Hashtable ReadFreeSpaceOnNetworkDrives(String FullComputerName)
        {
            ManagementPath path = new ManagementPath()
            {
                NamespacePath = @"root\cimv2",
                //Server = "<REMOTE HOST OR IP>"
                Server = "<192.168.1.111>"
            };

            ManagementScope scope = new ManagementScope(path);
            scope.Connect();
            //create Hashtable instance to hold our info
            Hashtable driveInfo = new Hashtable();
            //query the win32_logicaldisk for type 4 (Network drive)
            SelectQuery query = new SelectQuery("select name, FreeSpace from win32_logicaldisk where drivetype=4");

            //execute the query using WMI
            ManagementObjectSearcher searcher = new ManagementObjectSearcher(scope, query);
            //loop through each drive found
            foreach (ManagementObject drive in searcher.Get())
            {
                //add the name & freespace to our hashtable
                driveInfo.Add("Drive", drive["name"]);
                driveInfo.Add("Space", drive["FreeSpace"]);
            }
            return driveInfo;
        }


        public static void GetDriverInfoLocal()
        {
            foreach (System.IO.DriveInfo label in System.IO.DriveInfo.GetDrives())
            {

                if (label.IsReady)
                {
                    var A = label.Name + " " + label.TotalSize;
                    var B = label.Name + " " + label.TotalFreeSpace;
                }
            }
        }

        public static void FreeSpaceNetwork(string srvname)
        {
            try
            {
                ConnectionOptions conn = new ConnectionOptions();
                string strNameSpace = @"\\";
                if (srvname != "")
                    strNameSpace += srvname;
                else
                    strNameSpace += ".";
                strNameSpace += @"\root\cimv2";
                System.Management.ManagementScope managementScope = new System.Management.ManagementScope(strNameSpace, conn);
                System.Management.ObjectQuery query = new System.Management.ObjectQuery("select * from Win32_LogicalDisk where DriveType=3");
                ManagementObjectSearcher moSearcher = new ManagementObjectSearcher(managementScope, query);
                ManagementObjectCollection moCollection = moSearcher.Get();
                foreach (ManagementObject oReturn in moCollection)
                {
                    //foreach (PropertyData prop in oReturn.Properties)
                    //{
                    //    Console.WriteLine(prop.Name + " " + prop.Value);
                    //}

                    Console.WriteLine("Drive {0}", oReturn["Name"].ToString());
                    Console.WriteLine("Drive {0}", oReturn["Description"].ToString());
                    Console.WriteLine("  File system: {0}", oReturn["FileSystem"].ToString());
                    Console.WriteLine("  Available space to current user:{0, 15} bytes", oReturn["FreeSpace"].ToString());
                    Console.WriteLine("  Total size of drive:            {0, 15} bytes ", oReturn["Size"].ToString());

                    //Console.WriteLine("  Volume label: {0}", oReturn["VolumeName"].ToString());
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

    }
}
