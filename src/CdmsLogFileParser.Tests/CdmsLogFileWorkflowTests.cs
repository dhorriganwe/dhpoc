using System;
using System.IO;
using System.Linq;
using CdmsLogFileParser.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CdmsLogFileParser.Tests
{
    [TestClass]
    public class CdmsLogFileWorkflowTests : TestBase
    {
        //const string fileName1 = @"SampleData\cf9d80b1-f7ab-4a8f-887a-2331412d845c.log";
        const string fileName2 = @"SampleData\93e213af-81f4-4a44-95d4-eac7c9534149.log";

        private readonly CdmsLogFileWorkflow _workflow = new CdmsLogFileWorkflow();

        [TestMethod]
        [DeploymentItem(fileName2, "SampleData")]
        public void ReadLogFile()
        {
            var logFile = new LogFile(fileName2);
            Assert.IsTrue(logFile.FileInfo.Exists, "File does not exist: " + logFile.FileInfo.FullName);

            logFile.Lines = File.ReadLines(logFile.FileInfo.FullName).ToList();

            Assert.IsTrue(logFile.Lines.Count > 0);
        }

        [TestMethod]
        [DeploymentItem(fileName2, "SampleData")]
        public void WorkflowIdentifiesCdmsPerfLines()
        {
            var logFile = new LogFile(fileName2);
            Assert.IsTrue(logFile.FileInfo.Exists, "File does not exist: " + logFile.FileInfo.FullName);

            _workflow.ProcessFile(logFile);

            Assert.IsTrue(logFile.Lines.Count > 10);
            Assert.IsTrue(logFile.CdmsPerfLines.Count > 10);
            Assert.IsTrue(logFile.Lines.Count > logFile.CdmsPerfLines.Count);

            Console.WriteLine("logFile.CdmsPerfLines.Count: {0}{1}", logFile.CdmsPerfLines.Count, NL);

            logFile.CdmsPerfLines.ForEach(cpl => Console.WriteLine(cpl.Text));
        }

        [TestMethod]
        [DeploymentItem(fileName2, "SampleData")]
        public void WorkflowPopulatesCdmsRequestItems()
        {
            var logFile = new LogFile(fileName2);

            _workflow.ProcessFile(logFile);

            Assert.IsTrue(logFile.CdmsRequestItems.Count > 1);

            Console.WriteLine("logFile.CdmsRequestItems.Count: {0}{1}", logFile.CdmsRequestItems.Count, NL);

            Console.WriteLine();

            foreach (var item in logFile.CdmsRequestItems)
            {
                Console.WriteLine("FileMachineName: {0}", item.FileMachineName);
                Console.WriteLine("FileCorrelationId: {0}", item.FileCorrelationId);
                Console.WriteLine("RequestTimeStamp: {0}", item.RequestTimeStamp);
                Console.WriteLine("RequestType: {0}", item.RequestType);
                Console.WriteLine("RequestPerfData: {0}", item.RequestPerfData);
                Console.WriteLine("CdmsPerformance: {0}", item.CdmsPerformance);
                Console.WriteLine("ProviderPerformance: {0}", item.ProviderPerformance);
                Console.WriteLine();
            }
        }
    }
}
