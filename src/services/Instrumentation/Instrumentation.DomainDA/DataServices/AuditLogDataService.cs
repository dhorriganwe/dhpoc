using System;
using System.Collections.Generic;
using System.Data;
using Instrumentation.DomainDA.DbFramework;
using Instrumentation.DomainDA.Models;

namespace Instrumentation.DomainDA.DataServices
{
    public interface IAuditLogDataService
    {
        IList<AuditLog> GetAuditLogByEventId(string eventid);
        IList<AuditLog> GetAuditLogByTraceLevel(string travelLevel);
        IList<AuditLog> GetAuditLogAll();
        AuditLog GetAuditLogById(string id);
        void AddAuditLog(AuditLog auditLog);
        AuditLogSummary GetAuditLogSummary();
        List<ApplicationName> GetApplicationNames();
        List<FeatureName> GetFeatureNames();
        List<Category> GetCategories();
    }

    public class AuditLogDataService : IAuditLogDataService
    {
        #region AuditLog

        private const string GETAUDITLOGAll = "GetAuditLogAll";
        private const string GETAUDITLOGBYID = "GetAuditLogById";
        private const string GETAUDITLOGBYEVENTID = "GetAuditLogByEventId";
        private const string GETAUDITLOGBYTRACELEVEL = "GetAuditLogByTraceLevel";
        private const string GETAUDITLOGSUMMARY = "GetAuditLogSummary";
        private const string GETAPPLICATIONNAMES = "GetApplicationNames";
        private const string GETFEATURENAMES = "GetFeatureNames";
        private const string GETCATEGORIES = "GetCategories";
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

        public IList<AuditLog> GetAuditLogAll()
        {
            return GetAuditLog(GETAUDITLOGAll, new Dictionary<string, object>());
        }

        public AuditLogSummary GetAuditLogSummary()
        {
            var procName = GETAUDITLOGSUMMARY;
            AuditLogSummary summary = null;
            using (var dbContext = new SqlCommand(DBKEY, DBSCHEMA))
            {
                using (var reader = dbContext.ExecuteReader(procName, new Dictionary<string, object> { }))
                {
                    try
                    {
                        while (!reader.IsClosed && reader.Read())
                        {
                            summary = ToAuditLogSummary(reader);
                        }
                    }
                    catch (Exception e)
                    {
                        throw new Exception(string.Format("{0}storedProcedureName:{1}{0}reader.Depth:{2}", NL, procName, reader.Depth), e);
                    }
                }
            }

            return summary;
        }

        public List<ApplicationName> GetApplicationNames()
        {
            List<ApplicationName> applicationNames = new List<ApplicationName>();
            var procName = GETAPPLICATIONNAMES;
            ApplicationName appName = null;

            using (var dbContext = new SqlCommand(DBKEY, DBSCHEMA))
            {
                using (var reader = dbContext.ExecuteReader(procName, new Dictionary<string, object> { }))
                {
                    try
                    {
                        while (!reader.IsClosed && reader.Read())
                        {
                            appName = ToApplicationName(reader);
                            applicationNames.Add(appName);
                        }
                    }
                    catch (Exception e)
                    {
                        throw new Exception(string.Format("{0}storedProcedureName:{1}{0}reader.Depth:{2}", NL, procName, reader.Depth), e);
                    }
                }
            }

            return applicationNames;
        }

        public List<FeatureName> GetFeatureNames()
        {
            List<FeatureName> featureNames = new List<FeatureName>();
            var procName = GETFEATURENAMES;
            FeatureName featureName = null;

            using (var dbContext = new SqlCommand(DBKEY, DBSCHEMA))
            {
                using (var reader = dbContext.ExecuteReader(procName, new Dictionary<string, object> { }))
                {
                    try
                    {
                        while (!reader.IsClosed && reader.Read())
                        {
                            featureName = ToFeatureName(reader);
                            featureNames.Add(featureName);
                        }
                    }
                    catch (Exception e)
                    {
                        throw new Exception(string.Format("{0}storedProcedureName:{1}{0}reader.Depth:{2}", NL, procName, reader.Depth), e);
                    }
                }
            }

            return featureNames;
        }

        public List<Category> GetCategories()
        {
            List<Category> categories = new List<Category>();
            var procName = GETCATEGORIES;
            Category category = null;

            using (var dbContext = new SqlCommand(DBKEY, DBSCHEMA))
            {
                using (var reader = dbContext.ExecuteReader(procName, new Dictionary<string, object> { }))
                {
                    try
                    {
                        while (!reader.IsClosed && reader.Read())
                        {
                            category = ToCategory(reader);
                            categories.Add(category);
                        }
                    }
                    catch (Exception e)
                    {
                        throw new Exception(string.Format("{0}storedProcedureName:{1}{0}reader.Depth:{2}", NL, procName, reader.Depth), e);
                    }
                }
            }

            return categories;
        }

        public AuditLog GetAuditLogById(string id)
        {
            IList<AuditLog> auditLogs = GetAuditLog(GETAUDITLOGBYID, 
                new Dictionary<string, object>
                {
                    {"Id", id},
                });

            if(auditLogs.Count > 0)
                return auditLogs[0];
            else
                return new AuditLog();
        }

        public IList<AuditLog> GetAuditLogByEventId(string eventId)
        {
            IList<AuditLog> auditLogs = GetAuditLog(GETAUDITLOGBYEVENTID,
                new Dictionary<string, object>
                {
                    {"EventId", eventId},
                });

            return auditLogs;
        }

        public IList<AuditLog> GetAuditLogByTraceLevel(string travelLevel)
        {
            return GetAuditLog(GETAUDITLOGBYTRACELEVEL,
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

        private static IList<AuditLog> GetAuditLog(string storedProcedureName, IDictionary<string, object> parameters)
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

        private static AuditLogSummary ToAuditLogSummary(IDataReader reader)
        {
            return new AuditLogSummary()
            {
                TotalRowCount = reader["totalrowcount"].ReturnDefaultOrValue<long>(),
            };
        }

        private static ApplicationName ToApplicationName(IDataReader reader)
        {
            return new ApplicationName()
            {
                Name = StringField(reader, "name"),
                Count = LongField(reader, "count") 
            };
        }

        private static FeatureName ToFeatureName(IDataReader reader)
        {
            return new FeatureName()
            {
                Name = StringField(reader, "name"),
                Count = LongField(reader, "count")
            };
        }

        private static Category ToCategory(IDataReader reader)
        {
            return new Category()
            {
                Name = StringField(reader, "name"),
                Count = LongField(reader, "count")
            };
        }

        private static string StringField(IDataReader reader, string fieldName)
        {
            string val = null;
            try
            {
                val = reader[fieldName].ReturnDefaultOrValue<string>();
            }
            catch (Exception e)
            {
                throw new Exception(string.Format("fieldName: {0}", fieldName), e);
            }
            return val;
        }

        private static long LongField(IDataReader reader, string fieldName)
        {
            long val = 0;
            try
            {
                val = reader[fieldName].ReturnDefaultOrValue<long>();
            }
            catch (Exception e)
            {
                throw new Exception(string.Format("fieldName: {0}", fieldName), e);
            }
            return val;
        }
        #endregion
    }
}
