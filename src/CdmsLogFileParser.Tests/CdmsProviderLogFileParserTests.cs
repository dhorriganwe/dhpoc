using CdmsLogFileParser.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CdmsLogFileParser.Tests
{
    [TestClass]
    public class CdmsProviderLogFileParserTests : TestBase
    {
        private readonly CdmsProviderLogFileParser _parser = new CdmsProviderLogFileParser();

        [TestMethod]
        public void IdentifiesLogFileLineType_Unknown()
        {
            var line = new LogFileLine();

            Assert.AreEqual(LogFileLineType.Unknown, line.LogFileLineType);
        }

        [TestMethod]
        public void IdentifiesLogFileLineType_Date()
        {
            var line = new LogFileLine();
            line.Text = "11/20/2014 7:00:33 PM";
            _parser.SetLogFileLineType(line);

            Assert.AreNotEqual(LogFileLineType.Unknown, line.LogFileLineType);
            Assert.AreEqual(LogFileLineType.TimeStamp, line.LogFileLineType);
            Assert.AreEqual(CdmsRequestType.NA, line.CdmsRequestType);
        }

        [TestMethod]
        public void IdentifiesLogFileLineType_CorrelationId()
        {
            var line = new LogFileLine();
            line.Text = "correlationId: cf9d80b1-f7ab-4a8f-887a-2331412d845c";
            _parser.SetLogFileLineType(line);

            Assert.AreEqual(LogFileLineType.CorrelationId, line.LogFileLineType);
            Assert.AreEqual(CdmsRequestType.NA, line.CdmsRequestType);
        }

        [TestMethod]
        public void IdentifiesLogFileLineType_MachineName()
        {
            var line = new LogFileLine();
            line.Text = "MachineName: WECO27607";
            _parser.SetLogFileLineType(line);

            Assert.AreEqual(LogFileLineType.MachineName, line.LogFileLineType);
            Assert.AreEqual(CdmsRequestType.NA, line.CdmsRequestType);
        }

        [TestMethod]
        public void IdentifiesLogFileLineType_CR()
        {
            var line = new LogFileLine();
            line.Text = NL;
            _parser.SetLogFileLineType(line);

            Assert.AreEqual(LogFileLineType.CR, line.LogFileLineType);
            Assert.AreEqual(CdmsRequestType.NA, line.CdmsRequestType);
        }

        [TestMethod]
        public void IdentifiesLogFileLineType_CR2()
        {
            var line = new LogFileLine();
            line.Text = "";
            _parser.SetLogFileLineType(line);

            Assert.AreEqual(LogFileLineType.CR, line.LogFileLineType);
            Assert.AreEqual(CdmsRequestType.NA, line.CdmsRequestType);
        }

        [TestMethod]
        public void IdentifiesLogFileLineType_PerformanceData()
        {
            var line = new LogFileLine();
            line.Text = "cdms:843ms pvdr:845ms";
            _parser.SetLogFileLineType(line);

            Assert.AreEqual(LogFileLineType.PerformanceData, line.LogFileLineType);
            Assert.AreEqual(CdmsRequestType.NA, line.CdmsRequestType);
        }

        [TestMethod]
        public void IdentifiesLogFileLineType_Content()
        {
            var line = new LogFileLine();
            line.Text = "  <ProductUseSelections>";
            _parser.SetLogFileLineType(line);

            Assert.AreEqual(LogFileLineType.Content, line.LogFileLineType);
            Assert.AreEqual(CdmsRequestType.NA, line.CdmsRequestType);
        }

        [TestMethod]
        public void IdentifiesLogFileLineType_CdmsRequestType_ProductListRequest()
        {
            var line = new LogFileLine();
            line.Text = "ProductListRequest";
            _parser.SetLogFileLineType(line);

            Assert.AreEqual(LogFileLineType.CdmsRequestType, line.LogFileLineType);
            Assert.AreEqual(CdmsRequestType.ProductListRequest, line.CdmsRequestType);
        }

        [TestMethod]
        public void IdentifiesLogFileLineType_CdmsRequestType_ProductListResponse()
        {
            var line = new LogFileLine();
            line.Text = "ProductListResponse";
            _parser.SetLogFileLineType(line);

            Assert.AreEqual(LogFileLineType.CdmsRequestType, line.LogFileLineType);
            Assert.AreEqual(CdmsRequestType.ProductListResponse, line.CdmsRequestType);
        }

        [TestMethod]
        public void IdentifiesLogFileLineType_CdmsRequestType_LabelCheckMixCheckMixRequest()
        {
            var line = new LogFileLine();
            line.Text = "LabelCheckMix CheckMixRequest";
            _parser.SetLogFileLineType(line);

            Assert.AreEqual(LogFileLineType.CdmsRequestType, line.LogFileLineType);
            Assert.AreEqual(CdmsRequestType.LabelCheckMixCheckMixRequest, line.CdmsRequestType);
        }

        [TestMethod]
        public void IdentifiesLogFileLineType_CdmsRequestType_CheckJob_Request()
        {
            var line = new LogFileLine();
            line.Text = "Check Job_Request";
            _parser.SetLogFileLineType(line);

            Assert.AreEqual(LogFileLineType.CdmsRequestType, line.LogFileLineType);
            Assert.AreEqual(CdmsRequestType.CheckJob_Request, line.CdmsRequestType);
        }

        [TestMethod]
        public void IdentifiesLogFileLineType_CdmsRequestType_CheckJob_Response()
        {
            var line = new LogFileLine();
            line.Text = "Check Job_Response";
            _parser.SetLogFileLineType(line);

            Assert.AreEqual(LogFileLineType.CdmsRequestType, line.LogFileLineType);
            Assert.AreEqual(CdmsRequestType.CheckJob_Response, line.CdmsRequestType);
        }

        [TestMethod]
        public void IdentifiesLogFileLineType_CdmsRequestType_LabelCheckMixComplete()
        {
            var line = new LogFileLine();
            line.Text = "LabelCheckMix complete";
            _parser.SetLogFileLineType(line);

            Assert.AreEqual(LogFileLineType.CdmsRequestType, line.LogFileLineType);
            Assert.AreEqual(CdmsRequestType.LabelCheckMixComplete, line.CdmsRequestType);
        }

        [TestMethod]
        public void IdentifiesLogFileLineType_CdmsRequestType_AnswerQuestionRequest()
        {
            var line = new LogFileLine();
            line.Text = "AnswerQuestionRequest";
            _parser.SetLogFileLineType(line);

            Assert.AreEqual(LogFileLineType.CdmsRequestType, line.LogFileLineType);
            Assert.AreEqual(CdmsRequestType.AnswerQuestionRequest, line.CdmsRequestType);
        }

        [TestMethod]
        public void IdentifiesLogFileLineType_CdmsRequestType_AnswerRequest()
        {
            var line = new LogFileLine();
            line.Text = "Answer request";
            _parser.SetLogFileLineType(line);

            Assert.AreEqual(LogFileLineType.CdmsRequestType, line.LogFileLineType);
            Assert.AreEqual(CdmsRequestType.AnswerRequest, line.CdmsRequestType);
        }

        [TestMethod]
        public void IdentifiesLogFileLineType_CdmsRequestType_AnswerJob_Response()
        {
            var line = new LogFileLine();
            line.Text = "Answer Job_Response";
            _parser.SetLogFileLineType(line);

            Assert.AreEqual(LogFileLineType.CdmsRequestType, line.LogFileLineType);
            Assert.AreEqual(CdmsRequestType.AnswerJob_Response, line.CdmsRequestType);
        }

        [TestMethod]
        public void IdentifiesLogFileLineType_CdmsRequestType_AnswerQuestionComplete()
        {
            var line = new LogFileLine();
            line.Text = "AnswerQuestion complete";
            _parser.SetLogFileLineType(line);

            Assert.AreEqual(LogFileLineType.CdmsRequestType, line.LogFileLineType);
            Assert.AreEqual(CdmsRequestType.AnswerQuestionComplete, line.CdmsRequestType);
        }

        [TestMethod]
        public void ParsePerformanceLine1()
        {
            var item = new CdmsRequestItem();
            item.RequestPerfData = "cdms:1012ms pvdr:1016ms";

            _parser.ParsePerformanceLineToValues(item);

            Assert.AreEqual("1012", item.CdmsPerformance);
            Assert.AreEqual("1016", item.ProviderPerformance);
        }

        [TestMethod]
        public void ParsePerformanceLine2()
        {
            var item = new CdmsRequestItem();
            item.RequestPerfData = "cdms:734ms pvdr:738ms";

            _parser.ParsePerformanceLineToValues(item);

            Assert.AreEqual("734", item.CdmsPerformance);
            Assert.AreEqual("738", item.ProviderPerformance);
        }

        [TestMethod]
        public void ParsePerformanceLine3()
        {
            var item = new CdmsRequestItem();
            item.RequestPerfData = "cdms:843ms";

            _parser.ParsePerformanceLineToValues(item);

            Assert.AreEqual("843", item.CdmsPerformance);
            Assert.AreEqual("", item.ProviderPerformance);
        }
    }
}
