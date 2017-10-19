using System.IO;

namespace LANServerInfo
{
    public static class Global
    {
        public static readonly string[] tblWin32 =
            {
                "Win32_ComputerSystem",
                "Win32_DisplayConfiguration",
                "Win32_OperatingSystem",
                "win32_processor",
                "Win32_BIOS",
                "Win32_LogicalDisk",
                "Win32_DiskDrive",
                "Win32_PhysicalMemory",
                "Win32_NetworkAdapter",
                "Win32_NetworkAdapterConfiguration",
                "Win32_PerfFormattedData_PerfOS_Processor",
                "Win32_Share",
                "Win32_Volume",
                "Win32_Process"
        };

        public static readonly string OutputDirCurrent = Path.Combine(Directory.GetCurrentDirectory(), "Output");
        public static readonly string OutputDirCustom = Path.GetDirectoryName(Path.GetDirectoryName(Directory.GetCurrentDirectory())) + "\\Output";

        public static readonly string[] fileNames = { "TMP.txt", "TMP2.txt", "SystemProcessDetails.txt", "SystemRunningProcessName.txt" };

    }
}



















