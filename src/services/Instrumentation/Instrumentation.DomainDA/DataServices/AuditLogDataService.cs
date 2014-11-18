using System;
using System.Collections.Generic;
using System.Data;
using Instrumentation.DomainDA.DbFramework;
using Instrumentation.DomainDA.Helpers;
using Instrumentation.DomainDA.Models;

namespace Instrumentation.DomainDA.DataServices
{
    public interface IAuditLogDataService
    {
        IList<AuditLog> GetAuditLogsByEventId(string eventid);
        IList<AuditLog> GetAuditLogsByTraceLevel(string travelLevel);
        IList<AuditLog> GetAuditLogsAll();
        AuditLog GetAuditLogById(string id);
        void AddAuditLog(AuditLog auditLog);
    }

    public class AuditLogDataService : IAuditLogDataService
    {
        #region AuditLog

        private const string GETAUDITLOGSBYTRACELEVEL = "GetAuditLogsByTraceLevel";
        private const string GETAUDITLOGSAll = "getauditlogsall";
        private const string GETAUDITLOGBYID = "getauditlogbyid";
        private const string GETAUDITLOGSBYEVENTID = "getauditlogsbyeventid";
        private const string ADDAUDITLOG = "addauditlog";
        private const string DBKEY = "rtAudit";
        private const string DBSCHEMA = "rt";

        private static string NL = Environment.NewLine;

        public void AddAuditLog(AuditLog auditLog)
        {
            long id = AddAuditLog(ADDAUDITLOG,
                new Dictionary<string, object>
                {
                    {"EventId", auditLog.EventId},
                    {"ApplicationName", auditLog.ApplicationName},
                    {"FeatureName", auditLog.FeatureName},
                    {"Category", auditLog.Category},
                    {"MessageCode", auditLog.MessageCode},
                    {"Messages", auditLog.Messages},
                    {"TraceLevel", auditLog.TraceLevel},
                    {"LoginName", auditLog.LoginName},
                });

            auditLog.Id = id.ToString();
        }

        public IList<AuditLog> GetAuditLogsAll()
        {
            return GetAuditLogs(GETAUDITLOGSAll, new Dictionary<string, object>());
        }

        public AuditLog GetAuditLogById(string id)
        {
            IList<AuditLog> auditLogs = GetAuditLogs(GETAUDITLOGBYID, 
                new Dictionary<string, object>
                {
                    {"Id", id},
                });

            if(auditLogs.Count > 0)
                return auditLogs[0];
            else
                return new AuditLog();
        }

        public IList<AuditLog> GetAuditLogsByEventId(string eventId)
        {
            IList<AuditLog> auditLogs = GetAuditLogs(GETAUDITLOGSBYEVENTID,
                new Dictionary<string, object>
                {
                    {"EventId", eventId},
                });

            return auditLogs;
        }

        public IList<AuditLog> GetAuditLogsByTraceLevel(string travelLevel)
        {
            return GetAuditLogs(GETAUDITLOGSBYTRACELEVEL,
                new Dictionary<string, object>
                {
                    {"TraceLevel", travelLevel}
                });
        }

        private static long AddAuditLog(string storedProcedureName, IDictionary<string, object> parameters)
        {
            long id = -1;
            using (var dbContext = new SqlCommand(DBKEY, DBSCHEMA))
            {
                using (var reader = dbContext.ExecuteReader(storedProcedureName, parameters))
                {
                    while (!reader.IsClosed && reader.Read())
                    {
                        id = reader["addauditlog"].ReturnDefaultOrValue<long>();
                    }
                }
            }
            return id;
        }

        private static IList<AuditLog> GetAuditLogs(string storedProcedureName, IDictionary<string, object> parameters)
        {
            var auditLogs = new List<AuditLog>();

            using (var dbContext = new SqlCommand(DBKEY, DBSCHEMA))
            {
                using (var reader = dbContext.ExecuteReader(storedProcedureName, parameters))
                {
                    try
                    {
                        while (!reader.IsClosed && reader.Read())
                        {
                            var location = ToAuditLog(reader);
                            auditLogs.Add(location);
                        }
                    }
                    catch (Exception e)
                    {
                        throw new Exception(string.Format("{0}storedProcedureName:{1}{0}reader.Depth:{2}", NL, storedProcedureName, reader.Depth), e);
                    }
                }
            }
            return auditLogs;
        }

        private static AuditLog ToAuditLog(IDataReader reader)
        {
            return new AuditLog()
            {
                Id = reader["Id"].ReturnDefaultOrValue<string>(),
                EventId = reader["EventId"].ReturnDefaultOrValue<string>(),
                ApplicationName = reader["ApplicationName"].ReturnDefaultOrValue<string>(),
                FeatureName = reader["FeatureName"].ReturnDefaultOrValue<string>(),
                Category = reader["Category"].ReturnDefaultOrValue<string>(),
                MessageCode = reader["MessageCode"].ReturnDefaultOrValue<string>(),
                Messages = reader["messages"].ReturnDefaultOrValue<string>(),
                TraceLevel = reader["TraceLevel"].ReturnDefaultOrValue<string>(),
                LoginName = reader["LoginName"].ReturnDefaultOrValue<string>(),
                AuditedOn = reader["AuditedOn"].ReturnDefaultOrValue<string>(),
                AdditionalInfo = reader["AdditionalInfo"].ReturnDefaultOrValue<string>(),
            };
        }

        #endregion
    }
}
