using System;
using System.IO;
using System.Linq;
using CdmsLogFileParser.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CdmsLogFileParser.Tests
{
    [TestClass]
    public class LogFileParserTests : TestBase
    {
        const string fileName1 = @"SampleData\cf9d80b1-f7ab-4a8f-887a-2331412d845c.log";
        const string fileName2 = @"SampleData\eaa55364-2d05-4361-962c-c6c34096b03b.log";

        [TestMethod]
        [DeploymentItem(fileName1, "SampleData")]
        public void ReadLogFile()
        {
            LogFile logFile = new LogFile();
            
            logFile.FileInfo = new FileInfo(fileName1);
            Assert.IsTrue(logFile.FileInfo.Exists, "File does not exist: " + logFile.FileInfo.FullName);

            logFile.Lines = File.ReadLines(logFile.FileInfo.FullName).ToList();

            Assert.IsTrue(logFile.Lines.Count > 0);
        }

        [TestMethod]
        public void IdentifiesLogFileLineType_Unknown()
        {
            var line = new LogFileLine();

            Assert.AreEqual(LogFileLineType.Unknown, line.LogFileLineType);
        }


        [TestMethod]
        public void IdentifiesLogFileLineType_Date()
        {
            var parser = new CdmsProviderLogFileParser();
            var line = new LogFileLine();
            line.Text = "11/20/2014 7:00:33 PM";
            parser.SetLogFileLineType(line);

            Assert.AreNotEqual(LogFileLineType.Unknown, line.LogFileLineType);
            Assert.AreEqual(LogFileLineType.Date, line.LogFileLineType);
        }


        [TestMethod]
        public void IdentifiesLogFileLineType_CorrelationId()
        {
            var parser = new CdmsProviderLogFileParser();
            var line = new LogFileLine();
            line.Text = "correlationId: cf9d80b1-f7ab-4a8f-887a-2331412d845c";
            parser.SetLogFileLineType(line);

            Assert.AreEqual(LogFileLineType.CorrelationId, line.LogFileLineType);
        }

        [TestMethod]
        public void IdentifiesLogFileLineType_MachineName()
        {
            var parser = new CdmsProviderLogFileParser();
            var line = new LogFileLine();
            line.Text = "MachineName: WECO27607";
            parser.SetLogFileLineType(line);

            Assert.AreEqual(LogFileLineType.MachineName, line.LogFileLineType);
        }

        [TestMethod]
        public void IdentifiesLogFileLineType_CR()
        {
            var parser = new CdmsProviderLogFileParser();
            var line = new LogFileLine();
            line.Text = NL;
            parser.SetLogFileLineType(line);

            Assert.AreEqual(LogFileLineType.CR, line.LogFileLineType);
        }

        [TestMethod]
        public void IdentifiesLogFileLineType_CR2()
        {
            var parser = new CdmsProviderLogFileParser();
            var line = new LogFileLine();
            line.Text = "";
            parser.SetLogFileLineType(line);

            Assert.AreEqual(LogFileLineType.CR, line.LogFileLineType);
        }

        [TestMethod]
        public void IdentifiesLogFileLineType_CdmsRequestType_ProductListRequest()
        {
            var parser = new CdmsProviderLogFileParser();
            var line = new LogFileLine();
            line.Text = "ProductListRequest";
            parser.SetLogFileLineType(line);

            Assert.AreEqual(LogFileLineType.CdmsRequestType, line.LogFileLineType);
            Assert.AreEqual(CdmsRequestType.ProductListRequest, line.CdmsRequestType);
        }

        [TestMethod]
        public void IdentifiesLogFileLineType_CdmsRequestType_ProductListResponse()
        {
            var parser = new CdmsProviderLogFileParser();
            var line = new LogFileLine();
            line.Text = "ProductListResponse";
            parser.SetLogFileLineType(line);

            Assert.AreEqual(LogFileLineType.CdmsRequestType, line.LogFileLineType);
            Assert.AreEqual(CdmsRequestType.ProductListResponse, line.CdmsRequestType);
        }

        [TestMethod]
        public void IdentifiesLogFileLineType_CdmsRequestType_LabelCheckMixCheckMixRequest()
        {
            var parser = new CdmsProviderLogFileParser();
            var line = new LogFileLine();
            line.Text = "LabelCheckMix CheckMixRequest";
            parser.SetLogFileLineType(line);

            Assert.AreEqual(LogFileLineType.CdmsRequestType, line.LogFileLineType);
            Assert.AreEqual(CdmsRequestType.LabelCheckMixCheckMixRequest, line.CdmsRequestType);
        }

        [TestMethod]
        public void IdentifiesLogFileLineType_CdmsRequestType_CheckJob_Request()
        {
            var parser = new CdmsProviderLogFileParser();
            var line = new LogFileLine();
            line.Text = "Check Job_Request";
            parser.SetLogFileLineType(line);

            Assert.AreEqual(LogFileLineType.CdmsRequestType, line.LogFileLineType);
            Assert.AreEqual(CdmsRequestType.CheckJob_Request, line.CdmsRequestType);
        }

        [TestMethod]
        public void IdentifiesLogFileLineType_CdmsRequestType_CheckJob_Response()
        {
            var parser = new CdmsProviderLogFileParser();
            var line = new LogFileLine();
            line.Text = "Check Job_Response";
            parser.SetLogFileLineType(line);

            Assert.AreEqual(LogFileLineType.CdmsRequestType, line.LogFileLineType);
            Assert.AreEqual(CdmsRequestType.CheckJob_Response, line.CdmsRequestType);
        }

        [TestMethod]
        public void IdentifiesLogFileLineType_CdmsRequestType_LabelCheckMixComplete()
        {
            var parser = new CdmsProviderLogFileParser();
            var line = new LogFileLine();
            line.Text = "LabelCheckMix complete";
            parser.SetLogFileLineType(line);

            Assert.AreEqual(LogFileLineType.CdmsRequestType, line.LogFileLineType);
            Assert.AreEqual(CdmsRequestType.LabelCheckMixComplete, line.CdmsRequestType);
        }

        [TestMethod]
        public void IdentifiesLogFileLineType_CdmsRequestType_AnswerQuestionRequest()
        {
            var parser = new CdmsProviderLogFileParser();
            var line = new LogFileLine();
            line.Text = "AnswerQuestionRequest";
            parser.SetLogFileLineType(line);

            Assert.AreEqual(LogFileLineType.CdmsRequestType, line.LogFileLineType);
            Assert.AreEqual(CdmsRequestType.AnswerQuestionRequest, line.CdmsRequestType);
        }

        [TestMethod]
        public void IdentifiesLogFileLineType_CdmsRequestType_AnswerRequest()
        {
            var parser = new CdmsProviderLogFileParser();
            var line = new LogFileLine();
            line.Text = "Answer request";
            parser.SetLogFileLineType(line);

            Assert.AreEqual(LogFileLineType.CdmsRequestType, line.LogFileLineType);
            Assert.AreEqual(CdmsRequestType.AnswerRequest, line.CdmsRequestType);
        }

        [TestMethod]
        public void IdentifiesLogFileLineType_CdmsRequestType_AnswerJob_Response()
        {
            var parser = new CdmsProviderLogFileParser();
            var line = new LogFileLine();
            line.Text = "Answer Job_Response";
            parser.SetLogFileLineType(line);

            Assert.AreEqual(LogFileLineType.CdmsRequestType, line.LogFileLineType);
            Assert.AreEqual(CdmsRequestType.AnswerJob_Response, line.CdmsRequestType);
        }

        [TestMethod]
        public void IdentifiesLogFileLineType_CdmsRequestType_AnswerQuestionComplete()
        {
            var parser = new CdmsProviderLogFileParser();
            var line = new LogFileLine();
            line.Text = "AnswerQuestion complete";
            parser.SetLogFileLineType(line);

            Assert.AreEqual(LogFileLineType.CdmsRequestType, line.LogFileLineType);
            Assert.AreEqual(CdmsRequestType.AnswerQuestionComplete, line.CdmsRequestType);
        }

    }
}
