using System;
using System.IO;
using System.Linq;
using System.Text;
using CdmsLogFileParser.Models;

namespace CdmsLogFileParser
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine();
            Console.WriteLine(DateTime.Now.ToString());
            try
            {
                string logFileFolder = "";

                if (!HandleArgs(args, out logFileFolder))
                    DisplayUsage(args);
                else
                {
                    Console.WriteLine("LogFileFolder: " + logFileFolder);
                    Console.WriteLine("Processing log files...");

                    var parseJobWorkflow = new ParseJobWorkflow();

                    var jobSummary = parseJobWorkflow.ProcessLogFiles(logFileFolder);

                    DisplaySummary(jobSummary);
                }
            }
            catch (Exception e)
            {
                System.Console.WriteLine(e);
            }

            Console.WriteLine();
            Console.WriteLine(DateTime.Now.ToString());
            Console.WriteLine();
            //Console.WriteLine("any key to exit...");
            //Console.ReadKey();
        }

        private static void DisplaySummary(JobSummary jobSummary)
        {
            Console.WriteLine("Folder: {0}", jobSummary.Folder);
            Console.WriteLine("FileExtension: {0}", jobSummary.FileExtension);
            Console.WriteLine("FileCount: {0}", jobSummary.FileCount);
            Console.WriteLine("OutputFileName: {0}", jobSummary.OutputFileName);

            Console.WriteLine("Cdms Response time averages:");
            foreach (var requestTypeSummary in jobSummary.RequestTypeSummaries)
            {
                Console.WriteLine("{0}:  {1}ms  ({2} requests)", requestTypeSummary.Key, requestTypeSummary.Value.AverageDuration, requestTypeSummary.Value.Count);
            }
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
            var sb = new StringBuilder();
            args.ToList().ForEach(a => sb.AppendFormat("{0} ", a));

            Console.WriteLine("******************************************************************************");

            Console.WriteLine("USAGE");
            Console.WriteLine("");
            Console.WriteLine(" CdmsLogFileParser.exe [logFileFolder]");
            Console.WriteLine("");

            Console.WriteLine("ARGUMENTS");
            Console.WriteLine("");
            Console.WriteLine(" logFileFolder  Folder of CDMS log files");
            Console.WriteLine(" ");

            System.Console.WriteLine("Invalid arguments: " + sb);
            System.Console.WriteLine("");
            System.Console.WriteLine("");
        }

    }
}
