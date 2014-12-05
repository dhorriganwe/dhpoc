using System;
using System.Collections.Generic;
using System.Linq;
using Instrumentation.DomainDA.DataServices;
using Instrumentation.DomainDA.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Instrumentation.DomainDA.Test.DaBySprocTests
{
    [TestClass]
    public class AuditLogTests
    {
        private static string NL = Environment.NewLine;

        [TestMethod]
        public void GetAuditLogSummary()
        {
            IAuditLogDataService auditLogDataService = new AuditLogDataService();

            AuditLogSummary auditLogSummary = auditLogDataService.GetAuditLogSummary();
            
            Console.WriteLine(string.Format("{0} {1} ",
                NL,
                auditLogSummary.TotalRowCount));

            Assert.IsTrue(auditLogSummary.TotalRowCount > 0);
        }

        [TestMethod]
        public void GetApplicationNames()
        {
            IAuditLogDataService auditLogDataService = new AuditLogDataService();

            List<ApplicationName> applicationNames = auditLogDataService.GetApplicationNames();

            Assert.IsTrue(applicationNames.Count > 0);

            foreach (var applicationName in applicationNames)
            {
                Console.WriteLine(string.Format("{0} {1}",
                    applicationName.Name,
                    applicationName.Count));
            }
        }

        [TestMethod]
        public void GetCategories()
        {
            IAuditLogDataService auditLogDataService = new AuditLogDataService();

            List<Category> categories = auditLogDataService.GetCategories();

            Assert.IsTrue(categories.Count > 0);

            foreach (var category in categories)
            {
                Console.WriteLine(string.Format("{0} {1}",
                    category.Name,
                    category.Count));
            }
        }

        [TestMethod]
        public void GetFeatureNames()
        {
            IAuditLogDataService auditLogDataService = new AuditLogDataService();

            List<FeatureName> featureNames = auditLogDataService.GetFeatureNames();

            Assert.IsTrue(featureNames.Count > 0);

            foreach (var featureName in featureNames)
            {
                Console.WriteLine(string.Format("{0} {1}",
                    featureName.Name,
                    featureName.Count));
            }
        }

        [TestMethod]
        public void GetFeatureNames_Filtered()
        {
            IAuditLogDataService auditLogDataService = new AuditLogDataService();
            
            var applicationName = "";
            var featureName = "";
            var category = "";

            List<SummaryItem> summaryItems = auditLogDataService.GetSummaryItems(applicationName, featureName, category);

            Assert.IsTrue(summaryItems.Count > 0);

            foreach (var summaryItem in summaryItems)
            {
                Console.WriteLine(string.Format("{0} {1}",
                    summaryItem.ApplicationName,
                    summaryItem.Count));
            }
        }

        [TestMethod]
        public void GetAuditLogById()
        {
            IAuditLogDataService auditLogDataService = new AuditLogDataService();

            var id = AddAuditLogRow();
            AuditLog auditLog = auditLogDataService.GetAuditLogById(id);

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
        public void GetAuditLogByEventId()
        {
            IAuditLogDataService auditLogDataService = new AuditLogDataService();

            var eventId = "eventid";
            List<AuditLog> auditLogs = auditLogDataService.GetAuditLogByEventId(eventId).ToList();

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
        public void GetAuditLogByApplicationName()
        {
            IAuditLogDataService auditLogDataService = new AuditLogDataService();

            var applicationName = "appname";
            List<AuditLog> auditLogs = auditLogDataService.GetAuditLogByApplicationName(applicationName).ToList();

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
        public void GetAuditLogByCategory()
        {
            IAuditLogDataService auditLogDataService = new AuditLogDataService();

            var category = "Web unhandled exception";
            List<AuditLog> auditLogs = auditLogDataService.GetAuditLogByCategory(category).ToList();

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
        public void GetAuditLogByFeatureName()
        {
            IAuditLogDataService auditLogDataService = new AuditLogDataService();

            var featureName = "feature";
            List<AuditLog> auditLogs = auditLogDataService.GetAuditLogByFeatureName(featureName).ToList();

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
        public void GetAuditLogsByTraceLevel()
        {
            IAuditLogDataService auditLogDataService = new AuditLogDataService();

            var traceLevel = "traceLevel";

            List<AuditLog> auditLogs = auditLogDataService.GetAuditLogByTraceLevel(traceLevel).ToList();

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
        public void GetAuditLogsAll()
        {
            IAuditLogDataService auditLogDataService = new AuditLogDataService();

            List<AuditLog> auditLogs = auditLogDataService.GetAuditLogAll().ToList();

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
        public void GetAuditLogsAll_LimitRowcount()
        {
            IAuditLogDataService auditLogDataService = new AuditLogDataService();
            int rowcount = 10;
            List<AuditLog> auditLogs = auditLogDataService.GetAuditLogAll(rowcount).ToList();

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

        [TestMethod]
        public void AddAuditLog()
        {
            AddAuditLogRow();
        }

        public string AddAuditLogRow()
        {
            var auditLogDataService = new AuditLogDataService();

            AuditLog auditLog = new AuditLog();
            auditLog.EventId = "eventid";
            auditLog.ApplicationName = "appname";
            auditLog.FeatureName = "feature";
            auditLog.Category = "category1";
            auditLog.MessageCode = "code1";
            auditLog.Messages = "real message";
            auditLog.TraceLevel = "traceLevel";
            auditLog.LoginName = "login1";

            auditLogDataService.AddAuditLog(auditLog);

            return auditLog.Id;
        }
    }
}
