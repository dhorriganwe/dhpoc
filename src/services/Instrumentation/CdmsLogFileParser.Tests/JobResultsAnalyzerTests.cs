using System;
using CdmsLogFileParser.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CdmsLogFileParser.Tests
{
    [TestClass]
    public class JobResultsAnalyzerTests
    {
        JobResultsAnalyzer _jobResultsAnalyzer = new JobResultsAnalyzer();
        ParseJobWorkflow _parseJobWorkflow = new ParseJobWorkflow();

        [TestMethod]
        public void GeneratesAverages()
        {
            var folder = @"c:\logs";
            JobSummary jobSummary = _parseJobWorkflow.SummarizeFolderContents(folder);
            _parseJobWorkflow.ProcessLogFiles(jobSummary);

            _jobResultsAnalyzer.GenerateAverages(jobSummary);

            foreach (var average in jobSummary.Averages)
            {
                Console.WriteLine("{0}:  {1}", average.Key, average.Value);
            }
                
            Assert.IsTrue(jobSummary.Averages.Count > 1);
            Assert.AreNotEqual(jobSummary.Averages["ProductListResponse"], jobSummary.Averages["Check Job_Response"]);
            foreach (var average in jobSummary.Averages)
            {
                Assert.IsTrue(average.Value > 0);
            }
        }
    }
}
