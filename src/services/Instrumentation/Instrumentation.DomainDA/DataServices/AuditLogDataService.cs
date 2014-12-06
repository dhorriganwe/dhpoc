using System;
using System.Collections.Generic;
using System.Data;
using Instrumentation.DomainDA.DbFramework;
using Instrumentation.DomainDA.Helpers;
using Instrumentation.DomainDA.Models;

namespace Instrumentation.DomainDA.DataServices
{
    public class AuditLogDataService : IAuditLogDataService
    {
        #region AuditLog

        private const string GETAUDITLOGAll = "GetAuditLogAll";
        private const string GETAUDITLOGBYID = "GetAuditLogById";
        private const string GETAUDITLOGBYEVENTID = "GetAuditLogByEventId";
        private const string GETAUDITLOGBYAPPLICATIONNAME = "GetAuditLogByApplicationName";
        private const string GETAUDITLOGBYFEATURENAME = "GetAuditLogByFeatureName";
        private const string GETAUDITLOGBYCATEGORY = "GetAuditLogByCategory";
        private const string GETAUDITLOGBYTRACELEVEL = "GetAuditLogByTraceLevel";
        private const string GETAUDITLOGSUMMARY = "GetAuditLogSummary";
        private const string GETSUMMARYBYAPPLICATIONNAME = "GetSummaryByApplicationName";
        private const string GETSUMMARYBYFEATURENAME = "GetSummaryByFeatureName";
        private const string GETSUMMARYBYCATEGORY = "GetSummaryByCategory";
        
        private const string GETAPPLICATIONNAMES = "GetApplicationNames";
        private const string GETFEATURENAMES = "GetFeatureNames";
        private const string GETCATEGORIES = "GetCategories";
        private const string ADDAUDITLOG = "addauditlog";
        private const string DBSCHEMA = "rt";
        private readonly string DEFAULTDBKEY = "rtAudit";
        private static string NL = Environment.NewLine;
        private string _dbkey = null;

        public AuditLogDataService(string dbKey = "rtAudit")
        {
            DEFAULTDBKEY = Configurations.DbKeyDefault;

            if (string.IsNullOrEmpty(dbKey))
                _dbkey = DEFAULTDBKEY;
            else
                _dbkey = dbKey;
        }

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

        public IList<AuditLog> GetAuditLogAll(int maxRowCount = -1)
        {
            if (maxRowCount < 0)
                maxRowCount = Configurations.MaxRowCountDefault;

            return GetAuditLog(GETAUDITLOGAll,
                                new Dictionary<string, object>
                                {
                                    {"Rowcount", maxRowCount}
                                });
        }

        public AuditLogSummary GetAuditLogSummary()
        {
            var procName = GETAUDITLOGSUMMARY;
            AuditLogSummary summary = null;
            using (var dbContext = new SqlCommand(_dbkey, DBSCHEMA))
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

        public List<SummaryItem> GetSummaryItemsByApplicationName(string summaryType, string applicationName)
        {
            var procName = GETSUMMARYBYAPPLICATIONNAME;

            List<SummaryItem> summaryItems = new List<SummaryItem>();
            SummaryItem summaryItem = null;
            
            using (var dbContext = new SqlCommand(_dbkey, DBSCHEMA))
            {
                using (var reader = dbContext.ExecuteReader(procName, new Dictionary<string, object>
                {
                    {"SummaryType", summaryType},
                    {"ApplicationName", applicationName}
                }))
                {
                    try
                    {
                        while (!reader.IsClosed && reader.Read())
                        {
                            summaryItem = ToAuditLogSummaryItem(reader);
                            summaryItems.Add(summaryItem);
                        }
                    }
                    catch (Exception e)
                    {
                        throw new Exception(string.Format("{0}storedProcedureName:{1}{0}reader.Depth:{2}", NL, procName, reader.Depth), e);
                    }
                }
            }

            return summaryItems;
        }

        public List<SummaryItem> GetSummaryItemsByFeatureName(string summaryType, string featureName)
        {
            var procName = GETSUMMARYBYFEATURENAME;

            List<SummaryItem> summaryItems = new List<SummaryItem>();
            SummaryItem summaryItem = null;

            using (var dbContext = new SqlCommand(_dbkey, DBSCHEMA))
            {
                using (var reader = dbContext.ExecuteReader(procName, new Dictionary<string, object>
                {
                    {"SummaryType", summaryType},
                    {"FeatureName", featureName}
                }))
                {
                    try
                    {
                        while (!reader.IsClosed && reader.Read())
                        {
                            summaryItem = ToAuditLogSummaryItem(reader);
                            summaryItems.Add(summaryItem);
                        }
                    }
                    catch (Exception e)
                    {
                        throw new Exception(string.Format("{0}storedProcedureName:{1}{0}reader.Depth:{2}", NL, procName, reader.Depth), e);
                    }
                }
            }

            return summaryItems;
        }

        public List<SummaryItem> GetSummaryItemsByCategory(string summaryType, string category)
        {
            var procName = GETSUMMARYBYCATEGORY;

            List<SummaryItem> summaryItems = new List<SummaryItem>();
            SummaryItem summaryItem = null;

            using (var dbContext = new SqlCommand(_dbkey, DBSCHEMA))
            {
                using (var reader = dbContext.ExecuteReader(procName, new Dictionary<string, object>
                {
                    {"SummaryType", summaryType},
                    {"Category", category}
                }))
                {
                    try
                    {
                        while (!reader.IsClosed && reader.Read())
                        {
                            summaryItem = ToAuditLogSummaryItem(reader);
                            summaryItems.Add(summaryItem);
                        }
                    }
                    catch (Exception e)
                    {
                        throw new Exception(string.Format("{0}storedProcedureName:{1}{0}reader.Depth:{2}", NL, procName, reader.Depth), e);
                    }
                }
            }

            return summaryItems;
        }

        public List<SummaryItem> GetApplicationNames()
        {
            List<SummaryItem> applicationNames = new List<SummaryItem>();
            var procName = GETAPPLICATIONNAMES;
            SummaryItem appName = null;

            using (var dbContext = new SqlCommand(_dbkey, DBSCHEMA))
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

        public List<SummaryItem> GetFeatureNames()
        {
            var featureNames = new List<SummaryItem>();
            var procName = GETFEATURENAMES;
            SummaryItem featureName = null;

            using (var dbContext = new SqlCommand(_dbkey, DBSCHEMA))
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

        public List<SummaryItem> GetCategories()
        {
            List<SummaryItem> categories = new List<SummaryItem>();
            var procName = GETCATEGORIES;
            SummaryItem category = null;

            using (var dbContext = new SqlCommand(_dbkey, DBSCHEMA))
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

        public IList<AuditLog> GetAuditLogByApplicationName(string applicationName, int maxRowCount = -1)
        {
            if (maxRowCount < 0)
                maxRowCount = Configurations.MaxRowCountDefault;

            IList<AuditLog> auditLogs = GetAuditLog(GETAUDITLOGBYAPPLICATIONNAME,
                new Dictionary<string, object>
                {
                    {"ApplicationName", applicationName},
                    {"Rowcount", maxRowCount},
                });

            return auditLogs;
        }

        public IList<AuditLog> GetAuditLogByCategory(string category, int maxRowCount = -1)
        {
            if (maxRowCount < 0)
                maxRowCount = Configurations.MaxRowCountDefault;

            IList<AuditLog> auditLogs = GetAuditLog(GETAUDITLOGBYCATEGORY,
                new Dictionary<string, object>
                {
                    {"Category", category},
                    {"Rowcount", maxRowCount},
                });

            return auditLogs;
        }

        public IList<AuditLog> GetAuditLogByEventId(string eventId, int maxRowCount = -1)
        {
            if (maxRowCount < 0)
                maxRowCount = Configurations.MaxRowCountDefault;

            IList<AuditLog> auditLogs = GetAuditLog(GETAUDITLOGBYEVENTID,
                new Dictionary<string, object>
                {
                    {"EventId", eventId},
                    {"Rowcount", maxRowCount},
                });

            return auditLogs;
        }

        public IList<AuditLog> GetAuditLogByFeatureName(string featureName, int maxRowCount = -1)
        {
            if (maxRowCount < 0)
                maxRowCount = Configurations.MaxRowCountDefault;

            IList<AuditLog> auditLogs = GetAuditLog(GETAUDITLOGBYFEATURENAME,
                new Dictionary<string, object>
                {
                    {"FeatureName", featureName},
                    {"Rowcount", maxRowCount},
                });

            return auditLogs;
        }

        public IList<AuditLog> GetAuditLogByTraceLevel(string travelLevel, int maxRowCount = -1)
        {
            if (maxRowCount < 0)
                maxRowCount = Configurations.MaxRowCountDefault;

            return GetAuditLog(GETAUDITLOGBYTRACELEVEL,
                new Dictionary<string, object>
                {
                    {"TraceLevel", travelLevel},
                    {"Rowcount", maxRowCount},
                });
        }

        private long AddAuditLog(string storedProcedureName, IDictionary<string, object> parameters)
        {
            long id = -1;
            using (var dbContext = new SqlCommand(_dbkey, DBSCHEMA))
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

        private IList<AuditLog> GetAuditLog(string storedProcedureName, IDictionary<string, object> parameters)
        {
            var auditLogs = new List<AuditLog>();

            using (var dbContext = new SqlCommand(_dbkey, DBSCHEMA))
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

        private AuditLog ToAuditLog(IDataReader reader)
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

        private AuditLogSummary ToAuditLogSummary(IDataReader reader)
        {
            return new AuditLogSummary()
            {
                TotalRowCount = reader["totalrowcount"].ReturnDefaultOrValue<long>(),
            };
        }

        private SummaryItem ToAuditLogSummaryItem(IDataReader reader)
        {
            SummaryItem summaryItem = new SummaryItem()
            {
                Name = StringField(reader, "name"),
                Count = LongField(reader, "count")
            };

            if (!string.IsNullOrEmpty(summaryItem.Name))
                summaryItem.Name = summaryItem.Name.Trim();

            return summaryItem;
        }
        private SummaryItem ToApplicationName(IDataReader reader)
        {
            var applicationName = new SummaryItem()
            {
                Name = StringField(reader, "name"),
                Count = LongField(reader, "count") 
            };

            if (!string.IsNullOrEmpty(applicationName.Name))
                applicationName.Name = applicationName.Name.Trim();

            return applicationName;
        }

        private SummaryItem ToFeatureName(IDataReader reader)
        {
            var featureName = new SummaryItem()
            {
                Name = StringField(reader, "name"),
                Count = LongField(reader, "count")
            };

            if (!string.IsNullOrEmpty(featureName.Name))
                featureName.Name = featureName.Name.Trim();

            return featureName;
        }

        private SummaryItem ToCategory(IDataReader reader)
        {
            SummaryItem category = new SummaryItem()
            {
                Name = StringField(reader, "name"),
                Count = LongField(reader, "count")
            };

            if (!string.IsNullOrEmpty(category.Name))
                category.Name = category.Name.Trim();

            return category;
        }

        private string StringField(IDataReader reader, string fieldName)
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

        private long LongField(IDataReader reader, string fieldName)
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
