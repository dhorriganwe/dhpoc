using System.Collections.Generic;
using System.IO;

namespace CdmsLogFileParser.Models
{
    public class LogFile
    {
        public LogFile()
        {}

        public LogFile(string filePath)
        {
            FileInfo = new FileInfo(filePath);
        }

        public string Status = "Pass";
        public string Summary;
        public FileInfo FileInfo;
        public List<string> Lines = new List<string>();
        public List<LogFileLine> LogFileLines = new List<LogFileLine>();
        public List<LogFileLine> CdmsPerfLines = new List<LogFileLine>();
        public List<CdmsRequestItem> CdmsRequestItems = new List<CdmsRequestItem>();
    }
}
