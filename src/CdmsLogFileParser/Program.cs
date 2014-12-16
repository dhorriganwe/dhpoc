using System;
using System.IO;
using System.Linq;
using System.Text;
using CdmsLogFileParser.Helpers;
using CdmsLogFileParser.Models;

namespace CdmsLogFileParser
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                string logFileFolder = "";

                if (!HandleArgs(args, out logFileFolder))
                    DisplayUsage(args);
                else
                {
                    var jobSummary = SummarizeFolderContents(logFileFolder);
                    DisplaySummary(jobSummary);

                    ProcessFileList(jobSummary);
                }
            }
            catch (Exception e)
            {
                System.Console.WriteLine(e);
            }

            Console.WriteLine();
            Console.WriteLine("any key to exit...");
            Console.ReadKey();
        }

        private static void ProcessFileList(JobSummary jobSummary)
        {
            
        }

        private static void DisplaySummary(JobSummary jobSummary)
        {
            var sb = new StringBuilder();
            foreach (var fileName in jobSummary.FileNames)
            {
                sb.AppendFormat("{0}{1}", fileName, System.Environment.NewLine);
            }
            Console.WriteLine(sb);


            Console.WriteLine("Folder: {0}", jobSummary.Folder);
            Console.WriteLine("FileExtension: {0}", jobSummary.FileExtension);
            Console.WriteLine("FileCount: {0}", jobSummary.FileCount);
        }

        private static JobSummary SummarizeFolderContents(string logFileFolder)
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

        private static bool HandleArgs(
            string[] args,
            out string logFileFolder)
        {
            logFileFolder = "";

            if (args == null || args.Length == 0)
            {
                return false;
            }

            logFileFolder = args[0].ToLower();

            if (!Directory.Exists(logFileFolder))
                return false;

            return true;
        }

        private static void DisplayUsage(string[] args)
        {
            //PrintConfigurations();


            StringBuilder sb = new StringBuilder();
            args.ToList().ForEach(a => sb.AppendFormat("{0} ", a));

            System.Console.WriteLine("******************************************************************************");

            System.Console.WriteLine("USAGE");
            System.Console.WriteLine("");
            System.Console.WriteLine(" CdmsLogFileParser.exe [logFileFolder]");
            System.Console.WriteLine("");

            System.Console.WriteLine("ARGUMENTS");
            System.Console.WriteLine("");
            System.Console.WriteLine(" logFileFolder  Folder of CDMS log files");
            System.Console.WriteLine(" ");

            System.Console.WriteLine("Invalid arguments: " + sb);
            System.Console.WriteLine("");
            System.Console.WriteLine("");
        }

    }
}
