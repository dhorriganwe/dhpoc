using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CdmsLogFileParser.Helpers;
using CdmsLogFileParser.Models;

namespace CdmsLogFileParser
{
    public class LogFileParserJobWorkflow
    {
        private readonly CdmsLogFileWorkflow _logFileWorkflow = new CdmsLogFileWorkflow();

        public JobSummary SummarizeFolderContents(string logFileFolder)
        {
            var jobSummary = new JobSummary();
            jobSummary.Folder = logFileFolder;
            jobSummary.FileExtension = Configurations.FileExtension;

            DirectoryInfo di = new DirectoryInfo(logFileFolder);

            jobSummary.FileInfos = di.GetFiles(jobSummary.FileExtension).ToList();
            jobSummary.FileCount = jobSummary.FileInfos.Count.ToString();
            jobSummary.FileNames = jobSummary.FileInfos.Select(fi => fi.FullName).ToList();

            return jobSummary;
        }

        public JobSummary ProcessLogFiles(string logFileFolder)
        {
            var jobSummary = SummarizeFolderContents(logFileFolder);

            ProcessLogFiles(jobSummary);

            BuildOutputCsvString(jobSummary);

            WriteOutputCsvFile(jobSummary);

            return jobSummary;
        }

        private void WriteOutputCsvFile(JobSummary jobSummary)
        {
            var fileName = BuildOutputFilename(jobSummary.Folder);

            File.WriteAllText(fileName, jobSummary.OutputCsvText.ToString());
        }

        private void BuildOutputCsvString(JobSummary jobSummary)
        {
            List<CdmsRequestItem> allRequestItems = new List<CdmsRequestItem>();
            foreach (var logFile in jobSummary.LogFiles)
            {
                allRequestItems.AddRange(logFile.CdmsRequestItems);
            }

            var sb = new StringBuilder();
            sb.AppendFormat("{0};{1};{2};{3};{4};{5}{6}",
                "CorrelationId",
                "MachineName",
                "CdmsRequestType",
                "TimeStamp",
                "CdmsPerformance",
                "ProviderPerformance",
                Environment.NewLine);

            foreach (var requestItem in allRequestItems)
            {
                sb.AppendFormat("{0};{1};{2};{3};{4};{5}{6}",
                    requestItem.FileCorrelationId,
                    requestItem.FileMachineName,
                    requestItem.RequestType,
                    requestItem.RequestTimeStamp,
                    requestItem.CdmsPerformance,
                    requestItem.ProviderPerformance,
                    Environment.NewLine);
            }

            jobSummary.OutputCsvText = sb;
        }

        private void ProcessLogFiles(JobSummary jobSummary)
        {
            foreach (var fileInfo in jobSummary.FileInfos)
            {
                var logFile = new LogFile(fileInfo.FullName);

                _logFileWorkflow.ProcessFile(logFile);

                jobSummary.LogFiles.Add(logFile);
            }
        }

        public string BuildOutputFilename(string directory)
        {
            var now = DateTime.Now;
            string filename = string.Format("ProductDetails{0}{1,2:D2}{2,2:D2}{3,2:D2}{4,2:D2}{5,2:D2}",
                now.Year,
                now.Month,
                now.Day,
                now.Hour,
                now.Minute,
                now.Second);

            filename = Path.Combine(
                directory,
                filename +
                ".csv");

            return filename;
        }


    }
}
