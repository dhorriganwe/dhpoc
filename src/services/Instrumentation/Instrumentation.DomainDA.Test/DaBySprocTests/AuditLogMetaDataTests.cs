using System;
using System.Collections.Generic;
using Instrumentation.DomainDA.DataServices;
using Instrumentation.DomainDA.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Instrumentation.DomainDA.Test.DaBySprocTests
{
    [TestClass]
    public class AuditLogMetaDataTests
    {
        private static string NL = Environment.NewLine;
        IAuditLogDataService _auditLogDataService = new AuditLogDataService();

        [TestMethod]
        public void GetTotalRowCount()
        {
            long count = _auditLogDataService.GetTotalRowCount();
            
            Console.WriteLine(string.Format("{0} {1} ",
                NL,
                count));

            Assert.IsTrue(count > 0);
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
    }
}
