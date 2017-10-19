using LANServerInfo.Model;
using System;
using System.Collections.Generic;
using System.Management;
using System.Net;
using System.Net.NetworkInformation;

namespace LANServerInfo
{
    public static class ServerDetails
    {
        public static string GetServerInfoByIPAddress(string strMachineName)
        {
            string machineName = string.Empty;
            try
            {
                IPHostEntry hostEntry = Dns.GetHostEntry(strMachineName);

                machineName = hostEntry.HostName;
                Console.WriteLine("Machine Name: " + hostEntry.HostName);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return machineName;
        }

        public static void GetServerPingStatus(string strMachineName)
        {
            var ping = new Ping();
            var reply = ping.Send(strMachineName, 60 * 1000);
        }




        public static List<SytemInfo> Win32_ListItems(string strMachineName)
        {
            List<SytemInfo> list = new List<SytemInfo>();
            SytemInfo oSytemInfo = new SytemInfo();

            ConnectionOptions conn = new ConnectionOptions();
            conn.Username = "Administrator";
            conn.Password = "admin";

            ManagementScope wmiScope = new ManagementScope(string.Format("\\\\{0}\\root\\cimv2", strMachineName), conn);


            foreach (var item in Global.tblWin32)
            {
                list.Add(new SytemInfo { PropertyName = "************" + item, PropertyValue = item + "************" });

                ObjectQuery query = new ObjectQuery("select * from " + item + "");
                ManagementObjectSearcher moSearcher = new ManagementObjectSearcher(wmiScope, query);

                foreach (ManagementObject mObj in moSearcher.Get())
                {
                    foreach (PropertyData prop in mObj.Properties)
                    {
                        Console.WriteLine("{0}: {1}", prop.Name, prop.Value);

                        oSytemInfo = new SytemInfo();
                        oSytemInfo.PropertyName = prop.Name;
                        oSytemInfo.PropertyValue = prop.Value;
                        list.Add(oSytemInfo);
                    }
                }

                list.Add(new SytemInfo { PropertyName = null, PropertyValue = "\n\n\n\n\n\n" });

            }


            return list;
        }

        public static List<SytemInfo> Win32_OperatingSystem(string strMachineName)
        {
            List<SytemInfo> list = new List<SytemInfo>();
            SytemInfo oSytemInfo = new SytemInfo();

            ConnectionOptions conn = new ConnectionOptions();
            //conn.Username = "Administrator";
            //conn.Password = "admin";

            ManagementScope wmiScope = new ManagementScope(string.Format("\\\\{0}\\root\\cimv2", strMachineName), conn);


            ObjectQuery query = new ObjectQuery("select * from Win32_PerfRawData_PerfProc_Process");
            ManagementObjectSearcher moSearcher = new ManagementObjectSearcher(wmiScope, query);

            foreach (ManagementObject mObj in moSearcher.Get())
            {
                foreach (PropertyData prop in mObj.Properties)
                {
                    Console.WriteLine("{0}: {1}", prop.Name, prop.Value);

                    oSytemInfo = new SytemInfo();
                    oSytemInfo.PropertyName = prop.Name;
                    oSytemInfo.PropertyValue = prop.Value;
                    list.Add(oSytemInfo);
                }
            }

            return list;
        }


    }
}
