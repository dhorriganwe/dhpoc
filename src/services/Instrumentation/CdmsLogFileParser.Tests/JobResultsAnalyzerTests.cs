using System;
using CdmsLogFileParser.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CdmsLogFileParser.Tests
{
    [TestClass]
    public class JobResultsAnalyzerTests
    {
        readonly JobResultsAnalyzer _jobResultsAnalyzer = new JobResultsAnalyzer();
        readonly ParseJobWorkflow _parseJobWorkflow = new ParseJobWorkflow();

        [TestMethod]
        public void GeneratesAverages()
        {
            var folder = @"c:\logs";
            var jobSummary = _parseJobWorkflow.SummarizeFolderContents(folder);
            _parseJobWorkflow.ProcessLogFiles(jobSummary);

            _jobResultsAnalyzer.GenerateAverages(jobSummary);

            foreach (var summary in jobSummary.RequestTypeSummaries)
            {
                Console.WriteLine("{0}:  {1}  ({2} requests)", summary.Key, summary.Value.AverageDuration, summary.Value.Count);
            }
                
            Assert.IsTrue(jobSummary.RequestTypeSummaries.Count > 1);
            Assert.AreNotEqual(jobSummary.RequestTypeSummaries["ProductListResponse"], jobSummary.RequestTypeSummaries["Check Job_Response"]);
            foreach (var summary in jobSummary.RequestTypeSummaries)
            {
                Assert.IsTrue(summary.Value.AverageDuration > 0);

                switch (summary.Key)
                {
                    case "ProductListResponse":
                        Assert.AreEqual(871, summary.Value.Count);
                        break;
                    case "Check Job_Response":
                        Assert.AreEqual(576, summary.Value.Count);
                        break;
                    case "Answer Job_Response":
                        Assert.AreEqual(754, summary.Value.Count);
                        break;
                    default:
                        Assert.Fail("Expected valid requestType key.  Actual:" + summary.Key);
                        break;
                }
            }
        }
    }
}
