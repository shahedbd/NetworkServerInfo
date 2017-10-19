using System;
using System.Management;

namespace LANServerInfo
{
    public static class WMIConnection
    {
        public static ManagementScope GetWMIManagementScope(string IPOrComputerName)
        {
            ManagementScope wmiScope = null;
            try
            {
                ConnectionOptions conn = new ConnectionOptions();
                //Optional
                conn.Username = "Administrator";
                conn.Password = "admin";

                wmiScope = new ManagementScope(string.Format("\\\\{0}\\root\\cimv2", IPOrComputerName), conn);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return wmiScope;
        }

        public static ManagementObjectSearcher GetWMIManagementObjectSearcher(string IPOrComputerName, ObjectQuery queryObj)
        {
            ManagementObjectSearcher mgtObjSrg = null;
            try
            {
                ConnectionOptions conn = new ConnectionOptions();
                //Optional
                conn.Username = "Administrator";
                conn.Password = "admin";

                ManagementScope wmiScope = new ManagementScope(string.Format("\\\\{0}\\root\\cimv2", IPOrComputerName), conn);
                mgtObjSrg = new ManagementObjectSearcher(wmiScope, queryObj);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return mgtObjSrg;
        }

        public static ManagementObjectCollection GetWMIManagementObjectCollection(string IPOrComputerName, ObjectQuery queryObj)
        {
            ManagementObjectCollection mgmObjColl = null;
            try
            {
                ConnectionOptions conn = new ConnectionOptions();
                //Optional
                //conn.Username = "Administrator";
                //conn.Password = "admin";

                ManagementScope wmiScope = new ManagementScope(string.Format("\\\\{0}\\root\\cimv2", IPOrComputerName), conn);
                ManagementObjectSearcher mgtObjSrg = new ManagementObjectSearcher(wmiScope, queryObj);
                mgmObjColl = mgtObjSrg.Get();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return mgmObjColl;
        }
    }
}
