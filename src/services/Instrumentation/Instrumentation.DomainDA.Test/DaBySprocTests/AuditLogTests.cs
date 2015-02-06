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
        IAuditLogDataService _auditLogDataService = new AuditLogDataService();

        [TestMethod]
        public void GetAuditLogSummary()
        {
            AuditLogSummary auditLogSummary = _auditLogDataService.GetAuditLogRowCount();
            
            Console.WriteLine(string.Format("{0} {1} ",
                NL,
                auditLogSummary.TotalRowCount));

            Assert.IsTrue(auditLogSummary.TotalRowCount > 0);
        }

        [TestMethod]
        public void GetApplicationNames()
        {
            List<string> applicationNames = _auditLogDataService.GetApplicationNames();

            Assert.IsTrue(applicationNames.Count > 0);

            foreach (var applicationName in applicationNames)
            {
                Console.WriteLine(string.Format("{0}", applicationName));
            }
        }

        [TestMethod]
        public void GetFeatureNames()
        {
            List<string> featureNames = _auditLogDataService.GetFeatureNames();

            Assert.IsTrue(featureNames.Count > 0);

            foreach (var featureName in featureNames)
            {
                Console.WriteLine(string.Format("{0}", featureName));
            }
        }

        [TestMethod]
        public void GetCategories()
        {
            List<string> categories = _auditLogDataService.GetCategories();

            Assert.IsTrue(categories.Count > 0);

            foreach (var category in categories)
            {
                Console.WriteLine(string.Format("{0}", category));
            }
        }

        [TestMethod]
        public void GetTraceLevels()
        {
            List<string> traceLevels = _auditLogDataService.GetTraceLevels();

            Assert.IsTrue(traceLevels.Count > 0);

            foreach (var traceLevel in traceLevels)
            {
                Console.WriteLine(string.Format("{0}", traceLevel));
            }
        }

        [TestMethod]
        public void GetApplicationNameCounts()
        {
            List<SummaryItem> applicationNameCounts = _auditLogDataService.GetApplicationNameCounts();

            Assert.IsTrue(applicationNameCounts.Count > 0);

            foreach (var applicationNameCount in applicationNameCounts)
            {
                Console.WriteLine(string.Format("{0} {1}",
                    applicationNameCount.Name,
                    applicationNameCount.Count));
            }
        }

        [TestMethod]
        public void GetFeatureNameCounts()
        {
            List<SummaryItem> featureNameCounts = _auditLogDataService.GetFeatureNameCounts();

            Assert.IsTrue(featureNameCounts.Count > 0);

            foreach (var featureNameCount in featureNameCounts)
            {
                Console.WriteLine(string.Format("{0} {1}",
                    featureNameCount.Name,
                    featureNameCount.Count));
            }
        }

        [TestMethod]
        public void GetCategoryCounts()
        {
            List<SummaryItem> categories = _auditLogDataService.GetCategoryCounts();

            Assert.IsTrue(categories.Count > 0);

            foreach (var category in categories)
            {
                Console.WriteLine(string.Format("{0} {1}",
                    category.Name,
                    category.Count));
            }
        }

        [TestMethod]
        public void GetFeatureNamesByApplicationName()
        {
            var applicationName = "RisingTide";
            List<SummaryItem> summaryItems = _auditLogDataService.GetSummaryItemsByApplicationName("FeatureName", applicationName);

            Assert.IsTrue(summaryItems.Count > 0);

            foreach (var summaryItem in summaryItems)
            {
                Console.WriteLine(string.Format("{0} {1}",
                    summaryItem.Name,
                    summaryItem.Count));
            }

            Console.WriteLine();
            List<SummaryItem> featureNames = _auditLogDataService.GetFeatureNameCounts();

            foreach (var featureName in featureNames)
            {
                Console.WriteLine(string.Format("{0} {1}",
                    featureName.Name,
                    featureName.Count));
            }

            summaryItems.ForEach(si =>
            {
                Assert.IsTrue(featureNames.Exists(fn => fn.Name == si.Name), "expected SummaryItem in FeatureNames: " + si.Name);
            });
        }

        [TestMethod]
        public void GetCategoriesByApplicationName()
        {
            var applicationName = "RisingTide";
            List<SummaryItem> summaryItems = _auditLogDataService.GetSummaryItemsByApplicationName("Category", applicationName);

            Assert.IsTrue(summaryItems.Count > 0);

            foreach (var summaryItem in summaryItems)
            {
                Console.WriteLine(string.Format("{0} {1}",
                    summaryItem.Name,
                    summaryItem.Count));
            }

            Console.WriteLine();
            List<SummaryItem> categories = _auditLogDataService.GetCategoryCounts();

            foreach (var category in categories)
            {
                Console.WriteLine(string.Format("{0} {1}",
                    category.Name,
                    category.Count));
            }

            summaryItems.ForEach(si =>
            {
                Assert.IsTrue(categories.Exists(fn => fn.Name == si.Name), "expected SummaryItem in FeatureNames: " + si.Name);
            });
        }

        [TestMethod]
        public void ShouldDetectErrorRaisedInSproc()
        {
            var featureName = "AccessPermission";
            List<SummaryItem> summaryItems = _auditLogDataService.GetSummaryItemsByFeatureName("xyz", featureName);

            Assert.IsTrue(summaryItems.Count > 0);

        }

        [TestMethod]
        public void GetApplicationNamesByFeatureName()
        {
            var featureName = "AccessPermission";
            List<SummaryItem> summaryItems = _auditLogDataService.GetSummaryItemsByFeatureName("ApplicationName", featureName);

            Assert.IsTrue(summaryItems.Count > 0);

            foreach (var summaryItem in summaryItems)
            {
                Console.WriteLine(string.Format("{0} {1}",
                    summaryItem.Name,
                    summaryItem.Count));
            }

            Console.WriteLine();
            List<SummaryItem> applicationNames = _auditLogDataService.GetApplicationNameCounts();

            foreach (var applicationName in applicationNames)
            {
                Console.WriteLine(string.Format("{0} {1}",
                    applicationName.Name,
                    applicationName.Count));
            }

            summaryItems.ForEach(
                si => Assert.IsTrue(
                    applicationNames.Exists(
                        fn => fn.Name == si.Name), "expected SummaryItem in ApplicationNames: " + si.Name));
        }

        [TestMethod]
        public void GetApplicationNamesByCategoryName()
        {
            var category = "Security";
            List<SummaryItem> summaryItems = _auditLogDataService.GetSummaryItemsByCategory("ApplicationName", category);

            Assert.IsTrue(summaryItems.Count > 0);

            foreach (var summaryItem in summaryItems)
            {
                Console.WriteLine(string.Format("{0} {1}",
                    summaryItem.Name,
                    summaryItem.Count));
            }

            Console.WriteLine();
            List<SummaryItem> applicationNames = _auditLogDataService.GetApplicationNameCounts();

            foreach (var applicationName in applicationNames)
            {
                Console.WriteLine(string.Format("{0} {1}",
                    applicationName.Name,
                    applicationName.Count));
            }

            summaryItems.ForEach(
                si => Assert.IsTrue(
                    applicationNames.Exists(
                        fn => fn.Name == si.Name), "expected SummaryItem in ApplicationNames: " + si.Name));
        }

        [TestMethod]
        public void GetAuditLogById()
        {
            var id = AddAuditLogRow();
            AuditLog auditLog = _auditLogDataService.GetAuditLogById(id);

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
            var eventId = "eventid";
            List<AuditLog> auditLogs = _auditLogDataService.GetAuditLogByEventId(eventId).ToList();

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
            var applicationName = "appname";
            List<AuditLog> auditLogs = _auditLogDataService.GetAuditLogByApplicationName(applicationName).ToList();

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
            var category = "Web unhandled exception";
            List<AuditLog> auditLogs = _auditLogDataService.GetAuditLogByCategory(category).ToList();

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
            var featureName = "feature";
            List<AuditLog> auditLogs = _auditLogDataService.GetAuditLogByFeatureName(featureName).ToList();

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
            var traceLevel = "traceLevel";

            List<AuditLog> auditLogs = _auditLogDataService.GetAuditLogByTraceLevel(traceLevel).ToList();

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
            List<AuditLog> auditLogs = _auditLogDataService.GetAuditLogAll().ToList();

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
            int rowcount = 10;
            List<AuditLog> auditLogs = _auditLogDataService.GetAuditLogAll(rowcount).ToList();

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
