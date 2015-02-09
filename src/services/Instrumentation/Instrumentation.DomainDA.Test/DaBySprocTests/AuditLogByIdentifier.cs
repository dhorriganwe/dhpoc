using System;
using System.Collections.Generic;
using System.Linq;
using Instrumentation.DomainDA.DataServices;
using Instrumentation.DomainDA.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Instrumentation.DomainDA.Test.DaBySprocTests
{
    [TestClass]
    public class AuditLogByIdentifier
    {
        private static string NL = Environment.NewLine;
        IAuditLogDataService _auditLogDataService = new AuditLogDataService();

        [TestMethod]
        public void GetById()
        {
            var id = AddAuditLogRow();
            AuditLog auditLog = _auditLogDataService.GetById(id);

            Assert.AreEqual(auditLog.Id, id);

            Console.WriteLine(string.Format("{0} {1} {2} {3} {4} {5} {6} {7} {8} {9} ",
                NL,
                auditLog.Id,
                auditLog.EventId,
                auditLog.ApplicationName,
                auditLog.FeatureName,
                auditLog.Category,
                auditLog.MessageCode,
                //al.Messages,
                auditLog.TraceLevel,
                auditLog.LoginName,
                auditLog.AuditedOn));
        }


        [TestMethod]
        public void GetByEventId()
        {
            var eventId = "eventid";
            List<AuditLog> auditLogs = _auditLogDataService.GetByEventId(eventId).ToList();

            Assert.IsTrue(auditLogs.Count > 0);
            auditLogs.ForEach(al => Assert.AreEqual(al.EventId, eventId));

            auditLogs.ForEach(al => Console.WriteLine(string.Format("{0} {1} {2} {3} {4} {5} {6} {7} {8} {9} ",
                "",
                al.Id,
                al.EventId,
                al.ApplicationName,
                al.FeatureName,
                al.Category,
                al.MessageCode,
                //al.Messages,
                al.TraceLevel,
                al.LoginName,
                al.AuditedOn)));
        }

        [TestMethod]
        public void GetByApplicationName()
        {
            var applicationName = "appname";
            List<AuditLog> auditLogs = _auditLogDataService.GetByApplicationName(applicationName).ToList();

            Assert.IsTrue(auditLogs.Count > 0);
            auditLogs.ForEach(al => Assert.AreEqual(al.ApplicationName, applicationName));

            auditLogs.ForEach(al => Console.WriteLine(string.Format("{0} {1} {2} {3} {4} {5} {6} {7} {8} {9} ",
                "",
                al.Id,
                al.EventId,
                al.ApplicationName,
                al.FeatureName,
                al.Category,
                al.MessageCode,
                //al.Messages,
                al.TraceLevel,
                al.LoginName,
                al.AuditedOn)));
        }

        [TestMethod]
        public void GetByCategory()
        {
            var category = "Web unhandled exception";
            List<AuditLog> auditLogs = _auditLogDataService.GetByCategory(category).ToList();

            Assert.IsTrue(auditLogs.Count > 0);
            auditLogs.ForEach(al => Assert.AreEqual(al.Category, category));

            auditLogs.ForEach(al => Console.WriteLine(string.Format("{0} {1} {2} {3} {4} {5} {6} {7} {8} {9} ",
                "",
                al.Id,
                al.EventId,
                al.ApplicationName,
                al.FeatureName,
                al.Category,
                al.MessageCode,
                //al.Messages,
                al.TraceLevel,
                al.LoginName,
                al.AuditedOn)));
        }

        [TestMethod]
        public void GetByFeatureName()
        {
            var featureName = "feature";
            List<AuditLog> auditLogs = _auditLogDataService.GetByFeatureName(featureName).ToList();

            Assert.IsTrue(auditLogs.Count > 0);
            auditLogs.ForEach(al => Assert.AreEqual(al.FeatureName, featureName));

            auditLogs.ForEach(al => Console.WriteLine(string.Format("{0} {1} {2} {3} {4} {5} {6} {7} {8} {9} ",
                "",
                al.Id,
                al.EventId,
                al.ApplicationName,
                al.FeatureName,
                al.Category,
                al.MessageCode,
                //al.Messages,
                al.TraceLevel,
                al.LoginName,
                al.AuditedOn)));
        }

        [TestMethod]
        public void GetByTraceLevel()
        {
            var traceLevel = "traceLevel";

            List<AuditLog> auditLogs = _auditLogDataService.GetByTraceLevel(traceLevel).ToList();

            Assert.IsTrue(auditLogs.Count > 0);
            auditLogs.ForEach(al => Assert.AreEqual(al.TraceLevel, traceLevel));

            auditLogs.ForEach(al => Console.WriteLine(string.Format("{0} {1} {2} {3} {4} {5} {6} {7} {8} {9}",
                al.Id,
                al.EventId,
                al.ApplicationName,
                al.FeatureName,
                al.Category,
                al.MessageCode,
                al.Messages,
                al.TraceLevel,
                al.LoginName,
                al.AuditedOn)));
        }

        [TestMethod]
        public void GetAll()
        {
            List<AuditLog> auditLogs = _auditLogDataService.GetAll().ToList();

            Assert.IsTrue(auditLogs.Count > 5, "auditLogs.Count: " + auditLogs.Count);

            auditLogs.ForEach(al => Console.WriteLine(string.Format("{0} {1} {2} {3} {4} {5} {6} {7} {8} {9} ",
                "",
                al.Id,
                al.EventId,
                al.ApplicationName,
                al.FeatureName,
                al.Category,
                al.MessageCode,
                //al.Messages,
                al.TraceLevel,
                al.LoginName,
                al.AuditedOn)));
        }

        [TestMethod]
        public void GetAll_LimitRowcount()
        {
            int rowcount = 10;
            List<AuditLog> auditLogs = _auditLogDataService.GetAll(rowcount).ToList();

            Assert.IsTrue(auditLogs.Count > 5, "auditLogs.Count: " + auditLogs.Count);
            Assert.AreEqual(rowcount, auditLogs.Count);

            auditLogs.ForEach(al => Console.WriteLine(string.Format("{0} {1} {2} {3} {4} {5} {6} {7} {8} {9} ",
                "",
                al.Id,
                al.EventId,
                al.ApplicationName,
                al.FeatureName,
                al.Category,
                al.MessageCode,
                //al.Messages,
                al.TraceLevel,
                al.LoginName,
                al.AuditedOn)));
        }

        public string AddAuditLogRow()
        {
            AuditLog auditLog = DefaultAuditLog();

            _auditLogDataService.AddAuditLog(auditLog);

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
