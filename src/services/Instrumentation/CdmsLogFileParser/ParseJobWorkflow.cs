using System;
using System.IO;
using System.Linq;
using System.Text;
using CdmsLogFileParser.Helpers;
using CdmsLogFileParser.Models;

namespace CdmsLogFileParser
{
    public class ParseJobWorkflow
    {
        private readonly LogFileWorkflow _logFileWorkflow = new LogFileWorkflow();
        private readonly JobResultsAnalyzer _jobResultsAnalyzer = new JobResultsAnalyzer();

        public JobSummary ProcessLogFiles(string logFileFolder)
        {
            var jobSummary = SummarizeFolderContents(logFileFolder);

            Console.WriteLine("FileCount: {0}", jobSummary.FileCount);

            ProcessLogFiles(jobSummary);

            BuildOutputCsvString(jobSummary);

            WriteOutputCsvFile(jobSummary);

            SummarizeResults(jobSummary);

            WriteSummaryFile(jobSummary);

            return jobSummary;
        }

        private void WriteSummaryFile(JobSummary jobSummary)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendFormat("{0}/***********************************************/{0}", Environment.NewLine);


            sb.AppendFormat("{0}{1}", DateTime.Now.ToString(), Environment.NewLine);
            sb.AppendFormat("Folder: {0}{1}", jobSummary.Folder, Environment.NewLine);
            sb.AppendFormat("FileExtension: {0}{1}", jobSummary.FileExtension, Environment.NewLine);
            sb.AppendFormat("FileCount: {0}{1}", jobSummary.FileCount, Environment.NewLine);
            sb.AppendFormat("OutputFileName: {0}{1}", jobSummary.OutputFileName, Environment.NewLine);

            sb.AppendFormat("Cdms Response time averages:{0}", Environment.NewLine);
            foreach (var requestTypeSummary in jobSummary.RequestTypeSummaries)
            {
                sb.AppendFormat("{0}:  {1}ms  ({2} requests){3}", requestTypeSummary.Key, requestTypeSummary.Value.AverageDuration, requestTypeSummary.Value.Count, Environment.NewLine);
            }

            var fileName = BuildSummaryFilename(jobSummary.Folder);
            File.AppendAllText(fileName, sb.ToString());
        }

        public void ProcessLogFiles(JobSummary jobSummary)
        {
            int processedFileCount = 0;
            foreach (var fileInfo in jobSummary.FileInfos)
            {
                var logFile = new LogFile(fileInfo.FullName);

                try
                {
                    _logFileWorkflow.ProcessFile(logFile);
                }
                catch (Exception e)
                {
                    throw new Exception(string.Format("processedFileCount:{0}   fileInfo.FullName: {1}", processedFileCount, fileInfo.FullName), e);
                }

                jobSummary.LogFiles.Add(logFile);
                processedFileCount++;
            }

            foreach (var logFile in jobSummary.LogFiles)
            {
                jobSummary.AllRequestItems.AddRange(logFile.CdmsRequestItems);
            }
        }

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

        private void SummarizeResults(JobSummary jobSummary)
        {
            _jobResultsAnalyzer.GenerateAverages(jobSummary);
        }

        private void WriteOutputCsvFile(JobSummary jobSummary)
        {
            var fileName = BuildOutputFilename(jobSummary.Folder);

            File.WriteAllText(fileName, jobSummary.OutputCsvText.ToString());

            jobSummary.OutputFileName = fileName;
        }

        private void BuildOutputCsvString(JobSummary jobSummary)
        {
            var sb = new StringBuilder();
            sb.AppendFormat("{0};{1};{2};{3};{4};{5}{6}",
                "CorrelationId",
                "MachineName",
                "CdmsRequestType",
                "TimeStamp",
                "CdmsPerformance",
                "ProviderPerformance",
                Environment.NewLine);

            foreach (var requestItem in jobSummary.AllRequestItems)
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

        public string BuildOutputFilename(string directory)
        {
            var now = DateTime.Now;
            string filename = string.Format("CdmsPerformanceData{0}{1,2:D2}{2,2:D2}{3,2:D2}{4,2:D2}{5,2:D2}",
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

        public string BuildSummaryFilename(string directory)
        {
            var now = DateTime.Now;
            string filename = string.Format("Summary{0}{1,2:D2}{2,2:D2}{3,2:D2}{4,2:D2}{5,2:D2}",
                now.Year,
                now.Month,
                now.Day,
                now.Hour,
                now.Minute,
                now.Second);

            filename = Path.Combine(
                directory,
                filename +
                ".sum");

            return filename;
        }


    }
}
