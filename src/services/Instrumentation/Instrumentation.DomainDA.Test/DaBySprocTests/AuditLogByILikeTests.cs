using System;
using System.Collections.Generic;
using System.Linq;
using Instrumentation.DomainDA.DataServices;
using Instrumentation.DomainDA.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Instrumentation.DomainDA.Test.DaBySprocTests
{
    [TestClass]
    public class AuditLogByILikeTests
    {
        IAuditLogDataService _auditLogDataService = new AuditLogDataService();

        [TestMethod]
        public void GetByILikeEventId()
        {
            var eventIdSearchStr = "73a9";
            var maxRowCount = 100;
            var startDate = "1/1/2015";
            var endDate = "2/1/2015";

            IList<AuditLog> auditLogs = _auditLogDataService.GetByILikeEventId(maxRowCount, startDate, endDate, eventIdSearchStr);

            Assert.IsTrue(auditLogs.Count > 0);

            Assert.IsTrue(auditLogs[0].EventId.Contains(eventIdSearchStr));

            Assert.IsTrue(auditLogs[0].EventId.IndexOf(eventIdSearchStr) > 0);
        }

        [TestMethod]
        public void GetByILikeMessage()
        {
            var messageSearchStr = "xx";
            var maxRowCount = 100;
            var startDate = "1/1/2015";
            var endDate = "2/1/2015";

            IList<AuditLog> auditLogs = _auditLogDataService.GetByILikeMessage(maxRowCount, startDate, endDate, messageSearchStr);

            Assert.IsTrue(auditLogs.Count > 0);
            Assert.IsTrue(auditLogs[0].Messages.ToLower().Contains(messageSearchStr.ToLower()));
            Assert.IsTrue(auditLogs[0].Messages.ToLower().IndexOf(messageSearchStr.ToLower()) > 0);
        }

        [TestMethod]
        public void GetByILikeAdditionalInfo()
        {
            var additionalInfoSearchStr = "Lannate";
            var maxRowCount = 100;
            var startDate = "1/1/2015";
            var endDate = "2/1/2015";

            IList<AuditLog> auditLogs = _auditLogDataService.GetByILikeAdditionalInfo(maxRowCount, startDate, endDate, additionalInfoSearchStr);

            Assert.IsTrue(auditLogs.Count > 0);
            Assert.IsTrue(auditLogs[0].AdditionalInfo.ToLower().Contains(additionalInfoSearchStr.ToLower()));
            Assert.IsTrue(auditLogs[0].AdditionalInfo.ToLower().IndexOf(additionalInfoSearchStr.ToLower()) > 0);
        }

        [TestMethod]
        public void GetByILikeLoginName()
        {
            var loginNameSearchStr = "276";
            var maxRowCount = 100;
            var startDate = "1/1/2015";
            var endDate = "2/1/2015";

            IList<AuditLog> auditLogs = _auditLogDataService.GetByILikeLoginName(maxRowCount, startDate, endDate, loginNameSearchStr);

            Assert.IsTrue(auditLogs.Count > 0);
            Assert.IsTrue(auditLogs[0].LoginName.ToLower().Contains(loginNameSearchStr.ToLower()));
            Assert.IsTrue(auditLogs[0].LoginName.ToLower().IndexOf(loginNameSearchStr.ToLower()) > 0);
        }

    }
}
