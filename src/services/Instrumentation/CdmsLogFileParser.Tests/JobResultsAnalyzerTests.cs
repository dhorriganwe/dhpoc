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
        public void TestMethod1()
        {
            var folder = @"c:\logs";
            JobSummary jobSummary = _parseJobWorkflow.SummarizeFolderContents(folder);
            _parseJobWorkflow.ProcessLogFiles(jobSummary);

            _jobResultsAnalyzer.GenerateAverages(jobSummary);
        }
    }
}
