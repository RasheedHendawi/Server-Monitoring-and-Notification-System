using System.Management;
using Shared.Models;

namespace Task1_ServerStatistics.Services
{
    public class StatisticsCollector
    {
        public ServerStatistics Collect(string serverIdentifier)
        {
            var memoryInfo = GC.GetGCMemoryInfo();
            var cpuUsage = GetCpuUsage();

            return new ServerStatistics
            {
                MemoryUsage = memoryInfo.HeapSizeBytes / (1024.0 * 1024),
                AvailableMemory = memoryInfo.TotalAvailableMemoryBytes / (1024.0 * 1024),
                CpuUsage = cpuUsage,
                Timestamp = DateTime.UtcNow,
                ServerIdentifier = serverIdentifier
            };
        }

        private double GetCpuUsage()
        {
            double cpuUsage = 0.0;

            try
            {
                var searcher = new ManagementObjectSearcher("select * from Win32_PerfFormattedData_PerfOS_Processor where Name='_Total'");

                foreach (var obj in searcher.Get())
                {
                    cpuUsage = Convert.ToDouble(obj["PercentProcessorTime"]);
                }

                return Math.Round(cpuUsage, 2);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving CPU usage: {ex.Message}");
                return 0.0;
            }
        }
    }
}
