
namespace CdmsLogFileParser.Models
{
    public enum LogFileLineType
    {
        Unknown,
        CR,
        TimeStamp,
        CorrelationId,
        MachineName,
        CdmsRequestType,
        PerformanceData,
        Content
    }
}
