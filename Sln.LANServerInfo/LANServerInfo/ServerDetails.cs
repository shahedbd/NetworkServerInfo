using System;
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
    }
}
