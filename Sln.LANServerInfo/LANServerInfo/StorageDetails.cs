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

    }
}
