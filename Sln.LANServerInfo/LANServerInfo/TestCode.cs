using Microsoft.Management.Infrastructure;
using Microsoft.Management.Infrastructure.Options;
using System;
using System.IO;
using System.Management;
using System.Net.NetworkInformation;
using System.Security;
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


        //Need it
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



        public static void RemoteDiskSpace()
        {
            ConnectionOptions opt = new ConnectionOptions();
            ObjectQuery oQuery = new ObjectQuery("SELECT Size, FreeSpace, Name, FileSystem FROM Win32_LogicalDisk WHERE DriveType = 3");

            StreamReader oReader = new StreamReader("computers.txt");
            StreamWriter writer = new StreamWriter("diskSpace.html");
            writer.WriteLine("<html><body><table border=\"0\" cellpadding=\"5\">");
            writer.WriteLine(@"<tr>                                 
                                       <th>Machine</th>
                                       <th>Drive</th>
                                      <th>Size GB</th>
                                       <th>Free Space GB</th>
                                        <th>Free Space %</th>
                                      <th>FileSystem</th>
                                  </tr>");
            string sLine = string.Empty;

            while (sLine != null)
            {
                sLine = oReader.ReadLine();

                if (sLine != null)
                {
                    ManagementScope scope = new ManagementScope("\\\\" + sLine + "\\root\\cimv2", opt);

                    ManagementObjectSearcher moSearcher = new ManagementObjectSearcher(scope, oQuery);
                    ManagementObjectCollection collection = moSearcher.Get();
                    writer.WriteLine("<tr><td valign=\"top\">" + sLine + "</td></tr>");
                    Console.Write("Trying " + sLine + "...");
                    foreach (ManagementObject res in collection)
                    {
                        decimal size = Convert.ToDecimal(res["Size"]) / 1024 / 1024 / 1024;
                        decimal freeSpace = Convert.ToDecimal(res["FreeSpace"]) / 1024 / 1024 / 1024;
                        writer.WriteLine("<tr><td></td>");
                        writer.WriteLine("<td>" + res["Name"] + "</td>");
                        writer.WriteLine("<td>" + decimal.Round(size, 2) + " GB </td>");
                        writer.WriteLine("<td>" + decimal.Round(freeSpace, 2) + " GB </td>");
                        writer.WriteLine("<td>" + decimal.Round(freeSpace / size, 2) * 100 + "% </td>");
                        writer.WriteLine("<td>" + res["FileSystem"] + "</td>");
                        writer.WriteLine("</tr>");
                    }
                    Console.WriteLine("done!");
                }

            }
            Console.WriteLine("Please open diskSpace.html for result");
            writer.WriteLine("</table></body></html>");
            writer.Close();
            oReader.Close();

        }


        public static void DiskUsageForNetworkComputer(string PCName)
        {
            try
            {
                ConnectionOptions connection = new ConnectionOptions();

                ManagementScope scope = new ManagementScope("\\\\" + PCName + "\\root\\CIMV2", connection);
                scope.Connect();

                var query = new ObjectQuery("SELECT * FROM Win32_DiskDrive");

                var searcher = new ManagementObjectSearcher(scope, query);

                foreach (ManagementObject queryObj in searcher.Get())
                {


                    var dIndex = Convert.ToString(queryObj["Index"]);

                    long size = (Convert.ToInt64(queryObj["Size"]) / 1000000000);
                    //int Full = size / free;
                    //dFree = Convert.ToString(queryObj["Index"]);
                    var dSize = Convert.ToString(size);
                    var dCaption = Convert.ToString(queryObj["Caption"]);
                    var dDescript = Convert.ToString(queryObj["Description"]);
                    var dStatus = Convert.ToString(queryObj["Status"]);

                    //dgvBIOS.Rows.Add(dDescript, dIndex);
                    //dgvBIOS.Rows.Add(".... Drive Model", dCaption);
                    //dgvBIOS.Rows.Add(".... Drive Size", dSize + " GB");

                    string dLett = null;
                    if (dIndex == "0")
                    {
                        dLett = "C";
                    }
                    else if (dIndex == "1")
                    {
                        dLett = "E";
                    }
                    else if (dIndex == "2")
                    {
                        dLett = "F";
                    }
                    else if (dIndex == "3")
                    {
                        dLett = "G";
                    }

                    ObjectQuery q2 = new ObjectQuery(@"SELECT * FROM Win32_LogicalDisk WHERE Caption='" + dLett + ":'");
                    ManagementObjectSearcher s2 = new ManagementObjectSearcher(scope, q2);
                    foreach (ManagementObject qO2 in s2.Get())
                    {
                        long free = Convert.ToInt64(qO2["FreeSpace"]) / 1000000000;

                        //dgvBIOS.Rows.Add(".... Free Space", Convert.ToString(free) + " GB");
                    }

                    //dgvBIOS.Rows.Add(".... Drive Status", dStatus);

                }
            }
            catch (Exception err)
            {
                Console.WriteLine(err.Message);
            }
        }


        public static void WMITest(string PCName)
        {
            ConnectionOptions co = new ConnectionOptions();
            co.Impersonation = ImpersonationLevel.Impersonate;
            co.Authentication = AuthenticationLevel.Packet;
            co.Timeout = new TimeSpan(0, 0, 30);
            co.EnablePrivileges = true;
            co.Username = "\\";
            co.Password = "";

            //ManagementScope scope = new ManagementScope("\\\\" + PCName + "\\root\\CIMV2", connection);

            ManagementPath mp = new ManagementPath();
            mp.NamespacePath = @"\root\cimv2";
            mp.Server = PCName;

            ManagementScope ms = new ManagementScope(mp, co);
            ms.Connect();

            ManagementObjectSearcher srcd;
            srcd = new ManagementObjectSearcher
            (
                ms, new ObjectQuery("select * from Win32_DisplayConfiguration")
            );
        }


        public static void WMITest2(string PCName)
        {
            ConnectionOptions remoteConnectionOptions = new ConnectionOptions();
            remoteConnectionOptions.Impersonation = ImpersonationLevel.Impersonate;
            remoteConnectionOptions.EnablePrivileges = true;
            remoteConnectionOptions.Authentication = AuthenticationLevel.Packet;
            remoteConnectionOptions.Username = "Devzone";
            remoteConnectionOptions.Password = "dev321";

            //ManagementScope managementScope = new ManagementScope(@"\\" + PCName + @"\root\CIMV2", remoteConnectionOptions);

            ManagementScope myscope = new ManagementScope(@"\\" + PCName + "\\Username", remoteConnectionOptions);
            myscope.Connect();

            //ConnectionOptions options = new ConnectionOptions();
            //options.Authentication = AuthenticationLevel.PacketPrivacy;
            //ManagementScope managementScope = new ManagementScope(@"\\" + PCName + @"\root\WebAdministration", options);
            //managementScope.Connect();


            //ConnectionOptions oConn = new ConnectionOptions();
            //ManagementScope oScope = null;

            //oConn.Username = "Devzone";
            //oConn.Password = "dev321";
            //oConn.Authority = "ntlmdomain:" + "devstation";

            //oScope = new ManagementScope("\\\\" + PCName + "\\root\\CIMV2", oConn);

            //oScope.Connect();

        }


        public static void remoteConnection(string servername, string username, string password)
        {
            try
            {
                ConnectionOptions options = new ConnectionOptions();
                options.Authentication = AuthenticationLevel.PacketPrivacy;
                //ManagementScope managementScope = new ManagementScope(@"\\devstation\root\WebAdministration", options);
                //managementScope.Connect();

                ConnectionOptions rcOptions = new ConnectionOptions();
                rcOptions.Authentication = AuthenticationLevel.Packet;
                rcOptions.Impersonation = ImpersonationLevel.Impersonate;
                rcOptions.EnablePrivileges = true;
                rcOptions.Username = servername + @"\" + username;
                rcOptions.Password = password;

                ManagementScope mScope = new ManagementScope(string.Format(@"\\{0}\root\cimv2", servername), rcOptions);
                mScope.Connect();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public static void ConnectingToWMI(string Computer_B)
        {
            //string Namespace = @"root\cimv2";
            //string OSQuery = "SELECT * FROM Win32_OperatingSystem";
            //CimSession mySession = CimSession.Create(Computer_B);
            //IEnumerable<CimInstance> queryInstance = mySession.QueryInstances(Namespace, "WQL", OSQuery);


            //foreach (CimInstance cimInstance in queryInstance)
            //{
            //    Console.WriteLine("Process name: {0}", cimInstance.CimInstanceProperties["Name"].Value);
            //}


            //*************************************
            //string domain = "DESKTOP-3KU7CQB";
            //string username = "shahed";
            //string password = "dev321";

            string domain = "devstation";
            string username = "Prodev";
            string password = "dev123456";


            //PerformanceCounter freeSpaceCounter = null;
            //using (((WindowsIdentity)HttpContext.Current.User.Identity).Impersonate())
            //{
            //    freeSpaceCounter = new PerformanceCounter("LogicalDisk",
            //                               "Free Megabytes", "D:", "RemoteMachine12");
            //}

            ConnectionOptions con = new ConnectionOptions();
            con.Username = "Administrator";
            con.Password = "admin";

            ManagementScope scope = new ManagementScope(@"\\" + Computer_B + @"\root\cimv2", con);
            scope.Connect();




            SecureString securepassword = new SecureString();
            foreach (char c in password)
            {
                securepassword.AppendChar(c);
            }

            // create Credentials
            CimCredential Credentials = new CimCredential(PasswordAuthenticationMechanism.Default,
                                                          Computer_B,
                                                          username,
                                                          securepassword);

            // create SessionOptions using Credentials
            WSManSessionOptions SessionOptions = new WSManSessionOptions();
            SessionOptions.AddDestinationCredentials(Credentials);
            // create Session using computer, SessionOptions
            CimSession Session = CimSession.Create(Computer_B, SessionOptions);


            var allVolumes = Session.QueryInstances(@"root\cimv2", "WQL", "SELECT * FROM Win32_Volume");
            var allPDisks = Session.QueryInstances(@"root\cimv2", "WQL", "SELECT * FROM Win32_DiskDrive");

            // Loop through all volumes
            foreach (CimInstance oneVolume in allVolumes)
            {
                // Show volume information

                if (oneVolume.CimInstanceProperties["DriveLetter"].ToString()[0] > ' ')
                {
                    Console.WriteLine("Volume ‘{0}’ has {1} bytes total, {2} bytes available",
                                      oneVolume.CimInstanceProperties["DriveLetter"],
                                      oneVolume.CimInstanceProperties["Size"],
                                      oneVolume.CimInstanceProperties["SizeRemaining"]);
                }

            }

            // Loop through all physical disks
            foreach (CimInstance onePDisk in allPDisks)
            {
                // Show physical disk information
                Console.WriteLine("Disk {0} is model {1}, serial number {2}",
                                  onePDisk.CimInstanceProperties["DeviceId"],
                                  onePDisk.CimInstanceProperties["Model"].ToString().TrimEnd(),
                                  onePDisk.CimInstanceProperties["SerialNumber"]);
            }




        }


        public static void RemotePCConnTest(string REMOTE_COMPUTER_NAME)
        {
            var processToRun = new[] { "notepad.exe" };
            var connection = new ConnectionOptions();
            connection.Username = "Administrator";
            connection.Password = "admin";
            var wmiScope = new ManagementScope(string.Format("\\\\{0}\\root\\cimv2", REMOTE_COMPUTER_NAME), connection);
            var wmiProcess = new ManagementClass(wmiScope, new ManagementPath("Win32_Process"), new ObjectGetOptions());
            wmiProcess.InvokeMethod("Create", processToRun);

            wmiScope.Connect();
        }



        public static void GetDomainName()
        {
            var abc = IPGlobalProperties.GetIPGlobalProperties().DomainName;
            var ab2 = Environment.UserDomainName;
        }


    }
}
