using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Management;
using System.Net.NetworkInformation;
using System.Text;

namespace LANServerInfo.ProductionLevel
{
    public static class ServerInfos
    {

        public static void GetServerDetails(string IPOrComputerName)
        {
            List<string> list = new List<string>();
            try
            {
                var PingReply = GetServerPingStatus(IPOrComputerName);
                if (PingReply.Status == 0)
                {

                    list.Add(" Server Status*************************************************");
                    list.Add(" IP Address: " + PingReply.Address);
                    list.Add(" Server Ping Satus: " + PingReply.Status);


                    list.Add("\n\n\n\n Processor info*************************************************");
                    ObjectQuery queryProcessor = new ObjectQuery("select * from win32_processor");
                    var mngObgCollProcessor = WMIConnection.GetWMIManagementObjectCollection(IPOrComputerName, queryProcessor);
                    foreach (ManagementObject mObj in mngObgCollProcessor)
                    {
                        list.Add(" Processor Name: " + ProcessorName(mObj));
                    }




                    list.Add("\n\n\n\n Hard disk info\n*************************************************");
                    ObjectQuery queryHD = new ObjectQuery("select * from Win32_LogicalDisk where DriveType=3");
                    var mngObgCollHD = WMIConnection.GetWMIManagementObjectCollection(IPOrComputerName, queryHD);

                    foreach (ManagementObject mObj in mngObgCollHD)
                    {
                        list.Add(" Volume Name: " + GetStorageVolumeName(mObj));
                        list.Add(" Description: " + GetStorageDescription(mObj));
                        list.Add(" File system: " + GetStorageFileSystem(mObj));
                        list.Add(" Available space to current user: " + GetStorageFreeSpace(mObj));
                        list.Add(" Total size of drive: " + GetStorageSize(mObj) + "\n\n");

                    }

                    list.Add("\n\n\n\n Memory info*************************************************");
                    ObjectQuery queryOPeratingSystem = new ObjectQuery("select * from Win32_OPeratingSystem");
                    var mngObgOPeratingSystem = WMIConnection.GetWMIManagementObjectCollection(IPOrComputerName, queryOPeratingSystem);
                    foreach (ManagementObject obj in mngObgOPeratingSystem)
                    {

                        list.Add(" Total Visible Memory Size: " + TotalVisibleMemorySize(obj));
                        list.Add(" Total Virtual Memory Size: " + TotalVisibleMemorySize(obj));

                        list.Add(" Free Physical Memory: " + FreePhysicalMemory(obj));
                        list.Add(" Free Virtual Memory: " + FreeVirtualMemory(obj));
                    }


                    list.Add("\n\n\n\n System info*************************************************");
                    foreach (ManagementObject obj in mngObgOPeratingSystem)
                    {
                        list.Add(" Computer Name: " + ComputerName(obj));
                        list.Add(" Operating System Name: " + OperatingSystemName(obj));

                        list.Add(" Current Time Zone: " + CurrentTimeZone(obj));
                        list.Add(" Manufacturer: " + Manufacturer(obj));
                        list.Add(" OS Architecture: " + OSArchitecture(obj));
                        list.Add(" OS Language: " + OSLanguage(obj));


                        list.Add(" Registered User: " + RegisteredUser(obj));
                        list.Add(" Serial Number: " + SerialNumber(obj));
                        list.Add(" System Device: " + SystemDevice(obj));
                        list.Add(" System Directory: " + SystemDirectory(obj));
                        list.Add(" System Drive: " + SystemDrive(obj));

                        list.Add(" Version: " + Version(obj));
                        list.Add(" Windows Directory: " + WindowsDirectory(obj));


                        list.Add(" Operating System Install Date: " + OperatingSystemInstallDate(obj));
                        list.Add(" Operating System Last Boot Up Time: " + OperatingSystemLastBootUpTime(obj));
                        list.Add(" Operating System Local Date Time: " + OperatingSystemLocalDateTime(obj));
                    }



                    list.Add("\n\n\n\n Network info*************************************************");
                    ObjectQuery queryNetworkAdapter = new ObjectQuery(@"SELECT * FROM   Win32_NetworkAdapter WHERE  Manufacturer != 'Microsoft' AND NOT PNPDeviceID LIKE 'ROOT\\%'");
                    var mngObgNetworkAdapter = WMIConnection.GetWMIManagementObjectCollection(IPOrComputerName, queryNetworkAdapter);

                    IList<ManagementObject> managementObjectList = mngObgNetworkAdapter
                                                              .Cast<ManagementObject>()
                                                              .OrderBy(p => Convert.ToUInt32(p.Properties["Index"].Value))
                                                             .ToList();

                    // Let's just show all the properties for all physical adapters.
                    foreach (ManagementObject mo in managementObjectList)
                    {
                        foreach (PropertyData pd in mo.Properties)
                        {
                            Console.WriteLine(pd.Name + ": " + (pd.Value ?? "N/A"));
                            list.Add(pd.Name + ": " + (pd.Value ?? "N/A"));
                        }

                    }

                    //Write to file
                    var fileToCreate = Global.OutputDirCustom + "\\" + Global.fileNames[1];
                    File.WriteAllLines(fileToCreate, list, Encoding.UTF8);
                    Process.Start("notepad.exe", fileToCreate);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }



        public static void SystemProcessInfo(string IPOrComputerName)
        {
            List<string> list = new List<string>();
            list.Add("\n\n\n\n System Process info*************************************************");

            ObjectQuery queryProcess = new ObjectQuery("select * from Win32_Process");
            var mngObgProcess = WMIConnection.GetWMIManagementObjectCollection(IPOrComputerName, queryProcess);

            IList<ManagementObject> managementObjectList2 = mngObgProcess.Cast<ManagementObject>().ToList();


            //IList<ManagementObject> managementObjectList = mgtObjProcess.Get()
            //                                          .Cast<ManagementObject>()
            //                                          .OrderBy(p => Convert.ToUInt32(p.Properties["Index"].Value))
            //                                         .ToList();

            // Let's just show all the properties for all physical adapters.
            foreach (ManagementObject mo in managementObjectList2)
            {
                foreach (PropertyData pd in mo.Properties)
                {
                    Console.WriteLine(pd.Name + ": " + (pd.Value ?? "N/A"));
                    list.Add(pd.Name + ": " + (pd.Value ?? "N/A"));
                }
            }

            //Write to file
            var fileToCreate = Global.OutputDirCustom + "\\" + Global.fileNames[2];
            File.WriteAllLines(fileToCreate, list, Encoding.UTF8);
            Process.Start("notepad.exe", fileToCreate);
        }



        public static string GetStorageVolumeName(ManagementObject mObj)
        {
            return Convert.ToString(mObj["VolumeName"]);
        }

        public static string GetStorageDescription(ManagementObject mObj)
        {
            return Convert.ToString(mObj["Description"]);
        }

        public static string GetStorageFileSystem(ManagementObject mObj)
        {
            return Convert.ToString(mObj["FileSystem"]);
        }


        public static string GetStorageFreeSpace(ManagementObject mObj)
        {
            decimal freeSpace = Convert.ToDecimal(mObj["FreeSpace"]) / 1024 / 1024 / 1024;
            var FreeSpaceInGB = decimal.Round(freeSpace, 2);
            return FreeSpaceInGB.ToString() + "GB";
        }

        public static string GetStorageSize(ManagementObject mObj)
        {
            decimal freeSpace = Convert.ToDecimal(mObj["Size"]) / 1024 / 1024 / 1024;
            var FreeSpaceInGB = decimal.Round(freeSpace, 2);
            return FreeSpaceInGB.ToString() + "GB";
        }




        public static string TotalVisibleMemorySize(ManagementObject mObj)
        {
            decimal freeSpace = Convert.ToDecimal(mObj["TotalVisibleMemorySize"]) / 1024 / 1024;
            var FreeSpaceInGB = decimal.Round(freeSpace, 2);
            return FreeSpaceInGB.ToString() + "GB";
        }

        public static string TotalVirtualMemorySize(ManagementObject mObj)
        {
            decimal freeSpace = Convert.ToDecimal(mObj["TotalVirtualMemorySize"]) / 1024 / 1024;
            var FreeSpaceInGB = decimal.Round(freeSpace, 2);
            return FreeSpaceInGB.ToString() + "GB";
        }

        public static string FreePhysicalMemory(ManagementObject mObj)
        {

            decimal freeSpace = Convert.ToDecimal(mObj["FreePhysicalMemory"]) / 1024 / 1024;
            var FreeSpaceInGB = decimal.Round(freeSpace, 2);
            return FreeSpaceInGB.ToString() + "GB";
        }

        public static string FreeVirtualMemory(ManagementObject mObj)
        {

            decimal freeSpace = Convert.ToDecimal(mObj["FreeVirtualMemory"]) / 1024 / 1024;
            var FreeSpaceInGB = decimal.Round(freeSpace, 2);
            return FreeSpaceInGB.ToString() + "GB";
        }

        public static string ComputerName(ManagementObject mObj)
        {
            return mObj["CSName"].ToString();
        }

        public static string OperatingSystemName(ManagementObject mObj)
        {
            return mObj["Caption"].ToString();
        }

        public static string CurrentTimeZone(ManagementObject mObj)
        {
            return mObj["CurrentTimeZone"].ToString();
        }

        public static string Manufacturer(ManagementObject mObj)
        {
            return mObj["Manufacturer"].ToString();
        }
        public static string OSArchitecture(ManagementObject mObj)
        {
            return mObj["OSArchitecture"].ToString();
        }
        public static string OSLanguage(ManagementObject mObj)
        {
            return mObj["OSLanguage"].ToString();
        }
        public static string RegisteredUser(ManagementObject mObj)
        {
            return mObj["RegisteredUser"].ToString();
        }

        public static string SerialNumber(ManagementObject mObj)
        {
            return mObj["SerialNumber"].ToString();
        }
        public static string SystemDevice(ManagementObject mObj)
        {
            return mObj["SystemDevice"].ToString();
        }

        public static string SystemDirectory(ManagementObject mObj)
        {
            return mObj["SystemDirectory"].ToString();
        }
        public static string SystemDrive(ManagementObject mObj)
        {
            return mObj["SystemDrive"].ToString();
        }
        public static string Version(ManagementObject mObj)
        {
            return mObj["Version"].ToString();
        }
        public static string WindowsDirectory(ManagementObject mObj)
        {
            return mObj["WindowsDirectory"].ToString();
        }



        public static string OperatingSystemInstallDate(ManagementObject mObj)
        {
            return mObj["InstallDate"].ToString();
        }

        public static string OperatingSystemLastBootUpTime(ManagementObject mObj)
        {
            return mObj["LastBootUpTime"].ToString();
        }

        public static string OperatingSystemLocalDateTime(ManagementObject mObj)
        {
            return mObj["LocalDateTime"].ToString();
        }


        public static string ProcessorName(ManagementObject mObj)
        {
            return mObj["Name"].ToString();
        }




        public static PingReply GetServerPingStatus(string strMachineName)
        {
            var ping = new Ping();
            PingReply reply = ping.Send(strMachineName, 60 * 1000);
            return reply;
        }


    }
}
