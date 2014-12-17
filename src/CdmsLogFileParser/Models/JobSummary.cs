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
        public StringBuilder OutputCsvText;
    }
}
