using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Management;
using System.Text;

namespace LANServerInfo.Processes
{
    public static class WMIProcess
    {
        public static void GetWMIRunningProcessList(string IPOrComputerName)
        {
            ManagementScope wmiScope = WMIConnection.GetWMIManagementScope(IPOrComputerName);
            ObjectQuery queryProcessor = new ObjectQuery("SELECT * FROM Win32_Process");
            ManagementObjectSearcher mgtObjProcessor = new ManagementObjectSearcher(wmiScope, queryProcessor);

            ManagementObjectCollection collection = mgtObjProcessor.Get();

            var items = new List<Win32_ProcessModel>();
            foreach (ManagementObject obj in collection)
            {
                var item = new Win32_ProcessModel();
                item.Caption = (string)obj["Caption"];
                item.CommandLine = (string)obj["CommandLine"];
                item.CreationClassName = (string)obj["CreationClassName"];
                item.CreationDate = (DateTime?)obj["CreationDate"];
                item.CSCreationClassName = (string)obj["CSCreationClassName"];
                item.CSName = (string)obj["CSName"];
                item.Description = (string)obj["Description"];
                item.ExecutablePath = (string)obj["ExecutablePath"];
                item.ExecutionState = (ushort?)obj["ExecutionState"];
                item.Handle = (string)obj["Handle"];
                item.HandleCount = (uint?)obj["HandleCount"];
                item.InstallDate = (DateTime?)obj["InstallDate"];
                item.KernelModeTime = (ulong?)obj["KernelModeTime"];
                item.MaximumWorkingSetSize = (uint?)obj["MaximumWorkingSetSize"];
                item.MinimumWorkingSetSize = (uint?)obj["MinimumWorkingSetSize"];
                item.Name = (string)obj["Name"];
                item.OSCreationClassName = (string)obj["OSCreationClassName"];
                item.OSName = (string)obj["OSName"];
                item.OtherOperationCount = (ulong?)obj["OtherOperationCount"];
                item.OtherTransferCount = (ulong?)obj["OtherTransferCount"];
                item.PageFaults = (uint?)obj["PageFaults"];
                item.PageFileUsage = (uint?)obj["PageFileUsage"];
                item.ParentProcessId = (uint?)obj["ParentProcessId"];
                item.PeakPageFileUsage = (uint?)obj["PeakPageFileUsage"];
                item.PeakVirtualSize = (ulong?)obj["PeakVirtualSize"];
                item.PeakWorkingSetSize = (uint?)obj["PeakWorkingSetSize"];
                item.Priority = (uint?)obj["Priority"];
                item.PrivatePageCount = (ulong?)obj["PrivatePageCount"];
                item.ProcessId = (uint?)obj["ProcessId"];
                item.QuotaNonPagedPoolUsage = (uint?)obj["QuotaNonPagedPoolUsage"];
                item.QuotaPagedPoolUsage = (uint?)obj["QuotaPagedPoolUsage"];
                item.QuotaPeakNonPagedPoolUsage = (uint?)obj["QuotaPeakNonPagedPoolUsage"];
                item.QuotaPeakPagedPoolUsage = (uint?)obj["QuotaPeakPagedPoolUsage"];
                item.ReadOperationCount = (ulong?)obj["ReadOperationCount"];
                item.ReadTransferCount = (ulong?)obj["ReadTransferCount"];
                item.SessionId = (uint?)obj["SessionId"];
                item.Status = (string)obj["Status"];
                item.TerminationDate = (DateTime?)obj["TerminationDate"];
                item.ThreadCount = (uint?)obj["ThreadCount"];
                item.UserModeTime = (ulong?)obj["UserModeTime"];
                item.VirtualSize = (ulong?)obj["VirtualSize"];
                item.WindowsVersion = (string)obj["WindowsVersion"];
                item.WorkingSetSize = (ulong?)obj["WorkingSetSize"];
                item.WriteOperationCount = (ulong?)obj["WriteOperationCount"];
                item.WriteTransferCount = (ulong?)obj["WriteTransferCount"];

                items.Add(item);
            }
        }


        public static List<string> GetWMIRunningProcessName(string IPOrComputerName)
        {
            List<string> list = new List<string>();
            ObjectQuery queryProcessor = new ObjectQuery("SELECT * FROM Win32_Process");
            var mngObgColl = WMIConnection.GetWMIManagementObjectCollection(IPOrComputerName, queryProcessor);

            int i = 1;
            foreach (ManagementObject obj in mngObgColl)
            {
                list.Add("Processes " + i + ": " + (string)obj["Name"]);
                Console.WriteLine("Processes " + i + ": " + (string)obj["Name"]);
                i++;
            }

            //Write to file
            list.Sort();
            var fileToCreate = Global.OutputDirCustom + "\\" + Global.fileNames[3];
            File.WriteAllLines(fileToCreate, list, Encoding.UTF8);
            Process.Start("notepad.exe", fileToCreate);

            return list;
        }


        public static void Proce01()
        {

            int i = 1;
            StringBuilder sb = new StringBuilder();
            ManagementClass MgmtClass = new ManagementClass("Win32_Process");
            foreach (ManagementObject mo in MgmtClass.GetInstances()) //Goes through each running process and grabs each instance
            {
                Console.WriteLine("Process " + i + ":" + mo["Name"] + "  ID:" + mo["ProcessId"] + "   Handles:" + mo["HandleCount"] + "  Threads:" + mo["ThreadCount"]);
                i++;
            }
        }

        public static void Proce02()
        {
            int i = 1;
            ManagementObjectSearcher searcher =
            new ManagementObjectSearcher("root\\CIMV2",
                "SELECT Name, WorkingSetPrivate FROM Win32_PerfRawData_PerfProc_Process");
            foreach (ManagementObject queryObj in searcher.Get())
            {
                Console.WriteLine("{0} " + i + ": {1}", queryObj["Name"], queryObj["WorkingSetPrivate"]);
                i++;
            }


            //The above works great
            //I'm having trouble getting the same enumeration for getprocesses to return the memory.
            foreach (Process p in Process.GetProcesses("."))
                Console.WriteLine("Memory Allocation:" + p.PrivateMemorySize64.ToString());

            Console.WriteLine();
        }

    }
}
