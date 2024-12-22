namespace Task2_AnomalyDetection.Services
{
    public class AnomalyDetector(double memoryAnomalyThreshold, double cpuAnomalyThreshold, double memoryUsageThreshold, double cpuUsageThreshold)
    {
        private readonly double _memoryAnomalyThreshold = memoryAnomalyThreshold;
        private readonly double _cpuAnomalyThreshold = cpuAnomalyThreshold;
        private readonly double _memoryUsageThreshold = memoryUsageThreshold;
        private readonly double _cpuUsageThreshold = cpuUsageThreshold;

        public bool IsMemoryAnomaly(double current, double previous)
            => current > (previous * (1 + _memoryAnomalyThreshold));

        public bool IsCpuAnomaly(double current, double previous)
            => current > (previous * (1 + _cpuAnomalyThreshold));

        public bool IsMemoryHighUsage(double used, double available)
            => (used / (used + available)) > _memoryUsageThreshold;

        public bool IsCpuHighUsage(double current)
            => current > _cpuUsageThreshold;
    }
}
