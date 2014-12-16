
namespace CdmsLogFileParser.Models
{
    public class CdmsRequestItem
    {
        public string FileCorrelationId;
        public string FileMachineName;

        public string RequestTimeStamp;
        public string RequestType;
        public string RequestPerfData;

        public string CdmsPerformance = "";
        public string ProviderPerformance = "";
    }
}
