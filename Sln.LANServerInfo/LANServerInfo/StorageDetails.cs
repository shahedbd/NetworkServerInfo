using System;
using System.Management;

namespace LANServerInfo
{
    public static class StorageDetails
    {
        public static void GetDriveInfoBytes(string strMachineName)
        {
            try
            {
                ConnectionOptions conn = new ConnectionOptions();
                string strNameSpace = @"\\";
                if (strMachineName != "") strNameSpace += strMachineName;
                else strNameSpace += ".";
                strNameSpace += @"\root\cimv2";

                ManagementScope managementScope = new ManagementScope(strNameSpace, conn);
                ObjectQuery query = new ObjectQuery("select * from Win32_LogicalDisk where DriveType=3");
                ManagementObjectSearcher moSearcher = new ManagementObjectSearcher(managementScope, query);
                ManagementObjectCollection moCollection = moSearcher.Get();
                foreach (ManagementObject oReturn in moCollection)
                {
                    //VolumeName
                    Console.WriteLine(" Volume Name: {0}", oReturn["VolumeName"].ToString());
                    Console.WriteLine(" Description: {0}", oReturn["Description"].ToString());
                    Console.WriteLine(" File system: {0}", oReturn["FileSystem"].ToString());
                    Console.WriteLine(" Available space to current user: {0, 15} bytes", oReturn["FreeSpace"].ToString());
                    Console.WriteLine(" Total size of drive:            {0, 15} bytes ", oReturn["Size"].ToString());

                    Console.WriteLine();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }


        public static void GetDriveInfoBytes2(string REMOTE_COMPUTER_NAME)
        {
            try
            {

                ConnectionOptions conn = new ConnectionOptions();
                conn.Username = "Administrator";
                conn.Password = "admin";
                ManagementScope wmiScope = new ManagementScope(string.Format("\\\\{0}\\root\\cimv2", REMOTE_COMPUTER_NAME), conn);
                //wmiScope.Connect();


                //string strNameSpace = @"\\";
                //if (REMOTE_COMPUTER_NAME != "") strNameSpace += REMOTE_COMPUTER_NAME;
                //else strNameSpace += ".";
                //strNameSpace += @"\root\cimv2";
                //ManagementScope managementScope = new ManagementScope(strNameSpace, conn);

                ObjectQuery query = new ObjectQuery("select * from Win32_LogicalDisk where DriveType=3");
                ManagementObjectSearcher moSearcher = new ManagementObjectSearcher(wmiScope, query);
                ManagementObjectCollection moCollection = moSearcher.Get();


                foreach (ManagementObject oReturn in moCollection)
                {
                    decimal freeSpace = Convert.ToDecimal(oReturn["FreeSpace"]) / 1024 / 1024 / 1024;
                    decimal size = Convert.ToDecimal(oReturn["Size"]) / 1024 / 1024 / 1024;

                    //VolumeName
                    Console.WriteLine(" Volume Name: {0}", oReturn["VolumeName"].ToString());
                    Console.WriteLine(" Description: {0}", oReturn["Description"].ToString());
                    Console.WriteLine(" File system: {0}", oReturn["FileSystem"].ToString());
                    Console.WriteLine(" Available space to current user: " + decimal.Round(freeSpace, 2) + "GB");
                    Console.WriteLine(" Total size of drive: " + decimal.Round(size, 2) + "GB");

                    Console.WriteLine();
                }


                Console.WriteLine("********************************XXXXXXXXXXXXXXXXXXXXXX");
                ManagementObjectSearcher ComSerial = new ManagementObjectSearcher("SELECT * FROM Win32_BIOS");
                foreach (ManagementObject wmi in ComSerial.Get())
                {
                    try
                    {
                        Console.WriteLine("SL No: " + wmi.GetPropertyValue("SerialNumber").ToString());
                    }
                    catch { }
                }


                ManagementObjectSearcher mgtObj = new ManagementObjectSearcher("root\\CIMV2", "Select * from Win32_OPeratingSystem");
                foreach (ManagementObject obj in mgtObj.Get())
                {
                    Console.WriteLine("Total Ram Usage: " + GetTotalRamUsage(obj));
                    Console.WriteLine("Free Physical Memory: " + GetFreePhysicalMemory(obj));
                    Console.WriteLine("Total Physical Memory: " + GetTotalRamSize(obj));
                }

                //ManagementObjectSearcher searcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_ComputerSystem");
                //foreach (ManagementObject obj in searcher.Get())
                //{
                //    Console.WriteLine("Total Physical Memory: " + GetTotalRamSize(obj));
                //}




                //SelectQuery sq = new SelectQuery("select * from Win32_PerfFormattedData_PerfOS_Processor");
                //ManagementObjectSearcher mos = new ManagementObjectSearcher(wmiScope, sq);
                //foreach (ManagementObject mo in mos.Get())
                //{
                //    var usage = mo["PercentProcessorTime"];
                //    Console.WriteLine(usage);
                //}



                Console.WriteLine("********************************XXXXXXXXXXXXXXXXXXXXXX");
                string cpuInfo = string.Empty;
                ManagementClass mc = new ManagementClass("win32_processor");
                ManagementObjectCollection moc = mc.GetInstances();

                foreach (ManagementObject mo in moc)
                {
                    if (cpuInfo == "")
                    {
                        //Get only the first CPU's ID
                        cpuInfo = mo.Properties["processorID"].Value.ToString();
                        Console.WriteLine("SL No: " + cpuInfo);
                        break;
                    }
                }


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }


        public static long RAMTotSize(string REMOTE_COMPUTER_NAME)
        {
            ConnectionOptions conn = new ConnectionOptions();
            conn.Username = "Administrator";
            conn.Password = "admin";
            ManagementScope wmiScope = new ManagementScope(string.Format("\\\\{0}\\root\\cimv2", REMOTE_COMPUTER_NAME), conn);

            ObjectQuery query = new ObjectQuery("SELECT Capacity FROM Win32_PhysicalMemory");
            ManagementObjectSearcher searcher = new ManagementObjectSearcher(wmiScope, query);

            long Capacity = 0;
            foreach (ManagementObject WniPART in searcher.Get())
            {
                Capacity = Capacity + Convert.ToInt64(WniPART.Properties["Capacity"].Value);
            }

            Capacity = Capacity / 1024 / 1024 / 1024;

            return Capacity;

        }



        //Method which takes Management Object and returns a Int64 value
        //of total Physical Memory.
        public static long GetTotalRamUsage(ManagementObject m)
        {
            return Convert.ToInt64(m["TotalVisibleMemorySize"]);
        }
        //Method which takes Management Object and returns a Int64 value of
        //Free Physical Memory.
        public static long GetFreePhysicalMemory(ManagementObject m)
        {
            return Convert.ToInt64(m["FreePhysicalMemory"]);
        }

        public static long GetTotalRamSize(ManagementObject m)
        {
            return Convert.ToInt64(m["TotalPhysicalMemory"]);
        }


        public static void SampleWMI(string ComputerName)
        {
            ConnectionOptions ConnOption = new ConnectionOptions();
            ConnOption.Username = "Administrator";
            ConnOption.Password = "admin";
            ConnOption.Impersonation = ImpersonationLevel.Impersonate;

            ManagementScope scope = new ManagementScope("\\\\" + ComputerName + "\\root\\cimv2", ConnOption);
            scope.Connect();

            //Query system for Operating System information
            ObjectQuery query = new ObjectQuery("SELECT * FROM Win32_OperatingSystem");
            ManagementObjectSearcher searcher = new ManagementObjectSearcher(scope, query);

            ManagementObjectCollection queryCollection = searcher.Get();
            foreach (ManagementObject m in queryCollection)
            {
                //Display the remote computer information
                Console.WriteLine("Computer Name     : {0}", m["csname"]);
                Console.WriteLine("Windows Directory : {0}", m["WindowsDirectory"]);
                Console.WriteLine("Operating System  : {0}", m["Caption"]);
                Console.WriteLine("Version           : {0}", m["Version"]);
                Console.WriteLine("Manufacturer      : {0}", m["Manufacturer"]);
            }

            Console.WriteLine();
        }

    }
}
