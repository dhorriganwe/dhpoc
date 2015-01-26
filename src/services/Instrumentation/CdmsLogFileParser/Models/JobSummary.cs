using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CdmsLogFileParser.Models
{
    public class JobSummary
    {
        public string FileExtension;
        public string Folder;
        public string FileCount;
        public List<string> FileNames;
        public List<FileInfo> FileInfos;
        public List<LogFile> LogFiles = new List<LogFile>();
        public List<CdmsRequestItem> AllRequestItems = new List<CdmsRequestItem>();
        public StringBuilder OutputCsvText;
        public string OutputFileName;
        public string SummaryFileName;
        public Dictionary<string, RequestTypeSummary> RequestTypeSummaries = new Dictionary<string, RequestTypeSummary>();
    }
}
