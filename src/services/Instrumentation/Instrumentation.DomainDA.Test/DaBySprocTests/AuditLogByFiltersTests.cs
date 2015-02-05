using System;
using System.Collections.Generic;
using System.Linq;
using Instrumentation.DomainDA.DataServices;
using Instrumentation.DomainDA.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Instrumentation.DomainDA.Test.DaBySprocTests
{
    [TestClass]
    public class AuditLogByFiltersTests
    {
        private static string NL = Environment.NewLine;

        [TestMethod]
        public void GetAuditLogByFilters()
        {
            IAuditLogDataService auditLogDataService = new AuditLogDataService();

            var applicationName = "AgVerdict-WebApp";
            var traceLevel = "error";
            var maxRowCount = 10;
            var startDate = "1/1/2015";
            var endDate = "2/1/2015";

            IList<AuditLog> auditLogs = auditLogDataService.GetAuditLogByFilters(maxRowCount, startDate, endDate, traceLevel, applicationName);

            Assert.IsTrue(auditLogs.Count > 0);

            foreach (var auditLog in auditLogs)
            {
                Console.WriteLine(string.Format("{0} {1} {2} {3}",
                    auditLog.Id,
                    auditLog.TraceLevel,
                    auditLog.AuditedOn,
                    auditLog.ApplicationName));
            }
        }

        [TestMethod]
        public void GetAuditLogByFilters_TraceLevel()
        {
            IAuditLogDataService auditLogDataService = new AuditLogDataService();

            var applicationName = "AgVerdict-WebApp";
            var traceLevel = "error";
            var maxRowCount = 10;
            var startDate = "1/1/2015";
            var endDate = "2/1/2015";

            IList<AuditLog> auditLogs = auditLogDataService.GetAuditLogByFilters(maxRowCount, startDate, endDate, traceLevel, applicationName);

            var count1 = auditLogs.Count;
            Assert.IsTrue(auditLogs.Count > 0);

            foreach (var auditLog in auditLogs)
            {
                Console.WriteLine(string.Format("{0} {1} {2} {3}",
                    auditLog.Id,
                    auditLog.TraceLevel,
                    auditLog.AuditedOn,
                    auditLog.ApplicationName));
            }

            traceLevel = "info";
            auditLogs = auditLogDataService.GetAuditLogByFilters(maxRowCount, startDate, endDate, traceLevel, applicationName);

            var count12 = auditLogs.Count;

            foreach (var auditLog in auditLogs)
            {
                Console.WriteLine(string.Format("{0} {1} {2} {3}",
                    auditLog.Id,
                    auditLog.TraceLevel,
                    auditLog.AuditedOn,
                    auditLog.ApplicationName));
            }

            Assert.AreNotEqual(count1, count12);
        }

        [TestMethod]
        public void GetAuditLogByFilters_ApplicationName()
        {
            IAuditLogDataService auditLogDataService = new AuditLogDataService();

            var applicationName = "AgVerdict-WebApp";
            var traceLevel = "error";
            var maxRowCount = 100;
            var startDate = "1/1/2015";
            var endDate = "2/1/2015";

            IList<AuditLog> auditLogs = auditLogDataService.GetAuditLogByFilters(maxRowCount, startDate, endDate, traceLevel, applicationName);

            var count1 = auditLogs.Count;
            Assert.IsTrue(auditLogs.Count > 0);

            foreach (var auditLog in auditLogs)
            {
                Console.WriteLine(string.Format("{0} {1} {2} {3}",
                    auditLog.Id,
                    auditLog.TraceLevel,
                    auditLog.AuditedOn,
                    auditLog.ApplicationName));
            }

            applicationName = "AgVerdict-Geo";
            auditLogs = auditLogDataService.GetAuditLogByFilters(maxRowCount, startDate, endDate, traceLevel, applicationName);

            var count12 = auditLogs.Count;

            foreach (var auditLog in auditLogs)
            {
                Console.WriteLine(string.Format("{0} {1} {2} {3}",
                    auditLog.Id,
                    auditLog.TraceLevel,
                    auditLog.AuditedOn,
                    auditLog.ApplicationName));
            }

            Assert.AreNotEqual(count1, count12);
        }

        [TestMethod]
        public void GetAuditLogByFilters_ApplicationName0rows()
        {
            IAuditLogDataService auditLogDataService = new AuditLogDataService();

            var applicationName = "xx";
            var traceLevel = "error";
            var maxRowCount = 10;
            var startDate = "1/1/2015";
            var endDate = "2/1/2015";

            IList<AuditLog> auditLogs = auditLogDataService.GetAuditLogByFilters(maxRowCount, startDate, endDate, traceLevel, applicationName);

            Assert.AreEqual(0, auditLogs.Count);
        }

        [TestMethod]
        public void GetAuditLogByFilters_TraceLevelAndApplicationNameFilter()
        {
            IAuditLogDataService auditLogDataService = new AuditLogDataService();

            var applicationName = "appName1";
            var maxRowCount = 100;
            var traceLevel = "";
            var startDate = "1/1/2014";
            var endDate = "2/1/2016";

            IList<AuditLog> auditLogsAppName1All = auditLogDataService.GetAuditLogByFilters(maxRowCount, startDate, endDate, traceLevel, applicationName);
            Assert.IsTrue(auditLogsAppName1All.Count > 0);

            traceLevel = "error";
            IList<AuditLog> auditLogsAppName1Error = auditLogDataService.GetAuditLogByFilters(maxRowCount, startDate, endDate, traceLevel, applicationName);
            Assert.IsTrue(auditLogsAppName1Error.Count > 0);

            traceLevel = "info";
            IList<AuditLog> auditLogsAppName1Info = auditLogDataService.GetAuditLogByFilters(maxRowCount, startDate, endDate, traceLevel, applicationName);
            Assert.IsTrue(auditLogsAppName1Info.Count > 0);

            Assert.IsTrue(auditLogsAppName1All.Count > auditLogsAppName1Error.Count);
            Assert.IsTrue(auditLogsAppName1All.Count > auditLogsAppName1Info.Count);

            Assert.AreEqual(auditLogsAppName1All.Count, auditLogsAppName1Error.Count + auditLogsAppName1Info.Count);
        }

        [TestMethod]
        public void GetAuditLogByFilters_ApplicationNameAndTraceLevelFilter()
        {
            IAuditLogDataService auditLogDataService = new AuditLogDataService();

            var applicationName = "";
            var maxRowCount = 100;
            var traceLevel = "error";
            var startDate = "1/1/2014";
            var endDate = "2/1/2016";

            IList<AuditLog> auditLogsErrorAll = auditLogDataService.GetAuditLogByFilters(maxRowCount, startDate, endDate, traceLevel, applicationName);
            Assert.IsTrue(auditLogsErrorAll.Count > 0);

            applicationName = "appName1";
            IList<AuditLog> auditLogsErrorAppName1 = auditLogDataService.GetAuditLogByFilters(maxRowCount, startDate, endDate, traceLevel, applicationName);
            Assert.IsTrue(auditLogsErrorAppName1.Count > 0);

            applicationName = "appName2";
            IList<AuditLog> auditLogsErrorAppName2 = auditLogDataService.GetAuditLogByFilters(maxRowCount, startDate, endDate, traceLevel, applicationName);
            Assert.IsTrue(auditLogsErrorAppName2.Count > 0);

            Assert.IsTrue(auditLogsErrorAll.Count > auditLogsErrorAppName1.Count);
            Assert.IsTrue(auditLogsErrorAll.Count > auditLogsErrorAppName2.Count);

            Assert.IsTrue(auditLogsErrorAll.Count > auditLogsErrorAppName1.Count + auditLogsErrorAppName2.Count);
        }

        [TestMethod]
        public void AddAuditLogRows()
        {
            AddAuditLogRow1();
            AddAuditLogRow2();
        }

        public string AddAuditLogRow1()
        {
            var auditLogDataService = new AuditLogDataService();

            AuditLog auditLog = DefaultAuditLog();

            auditLog.ApplicationName = "appName1";
            auditLog.TraceLevel = "info";
            auditLogDataService.AddAuditLog(auditLog);

            auditLog.ApplicationName = "appName1";
            auditLog.TraceLevel = "error";
            auditLogDataService.AddAuditLog(auditLog);

            return auditLog.Id;
        }

        public string AddAuditLogRow2()
        {
            var auditLogDataService = new AuditLogDataService();

            AuditLog auditLog = DefaultAuditLog();

            auditLog.ApplicationName = "appName2";
            auditLog.TraceLevel = "info";
            auditLogDataService.AddAuditLog(auditLog);

            auditLog.ApplicationName = "appName2";
            auditLog.TraceLevel = "error";
            auditLogDataService.AddAuditLog(auditLog);

            return auditLog.Id;
        }

        private AuditLog DefaultAuditLog()
        {
            AuditLog auditLog = new AuditLog();
            auditLog.EventId = "eventid";
            auditLog.ApplicationName = "appname";
            auditLog.FeatureName = "feature";
            auditLog.Category = "category1";
            auditLog.MessageCode = "code1";
            auditLog.Messages = "real message";
            auditLog.TraceLevel = "traceLevel";
            auditLog.LoginName = "login1";

            return auditLog;
        }

    }
}
