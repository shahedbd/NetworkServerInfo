using System;
using System.Management;
using System.Net.NetworkInformation;
using System.ServiceProcess;
using System.Text;

namespace LANServerInfo
{
    public static class TestCode
    {

        public static void ConnectingtWMIRemotely(string strMachineName)
        {
            ManagementScope ms1 = new ManagementScope();
            ms1.Options.Username = "Devzone";
            ms1.Options.Password = "dev123";
            ms1.Options.Authority = string.Format("NTLMDOMAIN:{0}", ms1);
            ms1.Options.Impersonation = ImpersonationLevel.Impersonate;

            using (ManagementClass exportedShares = new ManagementClass(ms1, new ManagementPath("\\\\devstation\\root\\cimv2:Win32_Share"), null))
            {

                ManagementObjectCollection shares = exportedShares.GetInstances();
                foreach (ManagementObject share in shares)
                {
                    Console.Write("Name: " + share["Name"].ToString());
                }
            }
        }



        public static void ConnectingtWMIRemotelyAAA(string strMachineName)
        {

            //ConnectionOptions options = new ConnectionOptions();

            ConnectionOptions options = new ConnectionOptions();
            options.Username = "Devzone";
            options.Password = "dev123";
            options.Authority = "ntlmdomain: NAMEOFDOMAIN";
            options.Authentication = AuthenticationLevel.PacketPrivacy;

            ManagementScope scope1 = new ManagementScope(@"\\" + strMachineName + "\\root\\CIMV2", options);
            scope1.Connect();

            ManagementScope managementScope = new ManagementScope(@"\\" + strMachineName + "\\root\\WebAdministration", options);
            managementScope.Connect();


            ManagementScope scope = new ManagementScope("\\\\" + strMachineName + "\\root\\cimv2");
            scope.Connect();

            ObjectQuery query = new ObjectQuery("SELECT * FROM Win32_OperatingSystem");
            ManagementObjectSearcher searcher = new ManagementObjectSearcher(scope, query);
        }


        public static void ConnectingtWMIRemotelyOLD2(string strMachineName)
        {
            ConnectionOptions co = new ConnectionOptions();
            co.Impersonation = ImpersonationLevel.Impersonate;
            co.Authentication = AuthenticationLevel.Packet;
            co.Timeout = new TimeSpan(0, 0, 30);
            co.EnablePrivileges = true;
            co.Username = "Devzone";
            co.Password = "dev123";

            ManagementPath mp = new ManagementPath();
            mp.NamespacePath = @"\root\cimv2";
            mp.Server = "";               ///Regard this!!!!

            ManagementScope ms = new ManagementScope(mp, co);
            ms.Connect();

            ManagementObjectSearcher srcd;
            srcd = new ManagementObjectSearcher
            (
                ms, new ObjectQuery("select * from Win32_DisplayConfiguration")
            );
        }


        public static void SystemManufacturer()
        {
            SelectQuery query = new SelectQuery(@"Select * from Win32_ComputerSystem");

            //initialize the searcher with the query it is supposed to execute
            using (ManagementObjectSearcher searcher = new ManagementObjectSearcher(query))
            {
                //execute the query
                foreach (ManagementObject process in searcher.Get())
                {
                    //print system info
                    process.Get();
                    Console.WriteLine("/*********Operating System Information ***************/");
                    Console.WriteLine("{0}{1}", "System Manufacturer:", process["Manufacturer"]);
                    Console.WriteLine("{0}{1}", "System Model:", process["Model"]);


                }
            }

            ManagementObjectSearcher searcher1 = new ManagementObjectSearcher("SELECT * FROM Win32_BIOS");
            ManagementObjectCollection collection = searcher1.Get();


            foreach (ManagementObject obj in collection)
            {
                if (((string[])obj["BIOSVersion"]).Length > 1)
                    Console.WriteLine("BIOS VERSION: " + ((string[])obj["BIOSVersion"])[0] + " - " + ((string[])obj["BIOSVersion"])[1]);
                else
                    Console.WriteLine("BIOS VERSION: " + ((string[])obj["BIOSVersion"])[0]);
            }
        }


        public static string GetMACAddress()
        {
            NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();
            string sMacAddress = string.Empty;
            foreach (NetworkInterface adapter in nics)
            {
                if (sMacAddress == string.Empty)// only return MAC Address from first card  
                {
                    IPInterfaceProperties properties = adapter.GetIPProperties();
                    sMacAddress = adapter.GetPhysicalAddress().ToString();
                }
            }
            return sMacAddress;
        }



        public static string GetMACAddress2(string strMachineName)
        {
            string macAdd = string.Empty;
            ManagementScope theScope = new ManagementScope("\\\\" + strMachineName + "\\root\\cimv2");
            StringBuilder theQueryBuilder = new StringBuilder();
            theQueryBuilder.Append("SELECT MACAddress FROM Win32_NetworkAdapter");
            ObjectQuery theQuery = new ObjectQuery(theQueryBuilder.ToString());
            ManagementObjectSearcher theSearcher = new ManagementObjectSearcher(theScope, theQuery);
            ManagementObjectCollection theCollectionOfResults = theSearcher.Get();

            foreach (ManagementObject theCurrentObject in theCollectionOfResults)
            {
                macAdd = "MAC Address: " + theCurrentObject["MACAddress"].ToString();
            }

            return macAdd;
        }


        public static string GetMacAddress3(string ipAddress)
        {
            string macAddress = string.Empty;
            System.Diagnostics.Process pProcess = new System.Diagnostics.Process();
            pProcess.StartInfo.FileName = "arp";
            pProcess.StartInfo.Arguments = "-a " + ipAddress;
            pProcess.StartInfo.UseShellExecute = false;
            pProcess.StartInfo.RedirectStandardOutput = true;
            pProcess.StartInfo.CreateNoWindow = true;
            pProcess.Start();
            string strOutput = pProcess.StandardOutput.ReadToEnd();
            string[] substrings = strOutput.Split('-');
            if (substrings.Length >= 8)
            {
                macAddress = substrings[3].Substring(Math.Max(0, substrings[3].Length - 2))
                         + "-" + substrings[4] + "-" + substrings[5] + "-" + substrings[6]
                         + "-" + substrings[7] + "-"
                         + substrings[8].Substring(0, 2);
                return macAddress;
            }

            else
            {
                return "not found";
            }
        }


        public static void ServiceRunning()
        {
            ServiceController[] services = ServiceController.GetServices();

            foreach (ServiceController service in services)
            {
                Console.WriteLine("The {0} service is currently:   {1}  Port: {2}", service.DisplayName, service.Status, service.MachineName);
            }
        }

    }
}
