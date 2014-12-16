using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CdmsLogFileParser.Models
{
    public class JobSummary
    {
        public string FileExtension;
        public string Folder;
        public string FileCount;
        public List<string> FileNames;
        public List<FileInfo> FileInfos;
    }
}
