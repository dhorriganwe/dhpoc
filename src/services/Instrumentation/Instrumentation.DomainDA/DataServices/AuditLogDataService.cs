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
        private const string GETAUDITLOGBYFILTERS = "GetAuditLogByFilters";
        private const string GETBYILIKEEVENTID = "GetByILikeEventId";
        private const string GETBYILIKEMESSAGE = "GetByILikeMessage";
        private const string GETBYILIKEADDITIONALINFO = "GetByILikeAdditionalInfo";
        private const string GETBYILIKELOGINNAME = "GetByILikeLoginName";
        
        private const string GETAUDITLOGROWCOUNT = "GetAuditLogRowCount";
        private const string GETSUMMARYBYAPPLICATIONNAME = "GetSummaryByApplicationName";
        private const string GETSUMMARYBYFEATURENAME = "GetSummaryByFeatureName";
        private const string GETSUMMARYBYCATEGORY = "GetSummaryByCategory";
        
        private const string GETAPPLICATIONNAMECOUNTS = "GetApplicationNameCounts";
        private const string GETAPPLICATIONNAMES = "GetApplicationNames";
        private const string GETFEATURENAMECOUNTS = "GetFeatureNameCounts";
        private const string GETFEATURENAMES = "GetFeatureNames";
        private const string GETCATEGORYCOUNTS = "GetCategoryCounts";
        private const string GETCATEGORIES = "GetCategories";
        private const string GETTRACELEVELS = "GetTraceLevels";
        private const string ADDAUDITLOG = "addauditlog";
        private const string DBSCHEMA = "rt";
        private readonly string DEFAULTDBKEY = "rtAudit";
        private static string NL = Environment.NewLine;
        private string _dbkey = null;

        public AuditLogDataService(string dbKey)
        {
            DEFAULTDBKEY = Configurations.DbKeyDefault;

            if (string.IsNullOrEmpty(dbKey))
                _dbkey = DEFAULTDBKEY;
            else
                _dbkey = dbKey;
        }

        public AuditLogDataService()
        {
            DEFAULTDBKEY = Configurations.DbKeyDefault;

            _dbkey = DEFAULTDBKEY;
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

            return GetAuditLogList(GETAUDITLOGAll,
                                new Dictionary<string, object>
                                {
                                    {"Rowcount", maxRowCount}
                                });
        }

        public AuditLogSummary GetAuditLogRowCount()
        {
            var procName = GETAUDITLOGROWCOUNT;
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
                            summaryItem = ToSummaryItem(reader);
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
                            summaryItem = ToSummaryItem(reader);
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
                            summaryItem = ToSummaryItem(reader);
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

        public List<string> GetApplicationNames()
        {
            var applicationNames = new List<string>();
            var procName = GETAPPLICATIONNAMES;

            using (var dbContext = new SqlCommand(_dbkey, DBSCHEMA))
            {
                using (var reader = dbContext.ExecuteReader(procName, new Dictionary<string, object> { }))
                {
                    try
                    {
                        while (!reader.IsClosed && reader.Read())
                        {
                            var item = ToStringItemByIndex(reader);
                            applicationNames.Add(item);
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

        public List<string> GetFeatureNames()
        {
            var featureNames = new List<string>();
            var procName = GETFEATURENAMES;

            using (var dbContext = new SqlCommand(_dbkey, DBSCHEMA))
            {
                using (var reader = dbContext.ExecuteReader(procName, new Dictionary<string, object> { }))
                {
                    try
                    {
                        while (!reader.IsClosed && reader.Read())
                        {
                            var item = ToStringItemByIndex(reader);
                            featureNames.Add(item);
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

        public List<string> GetCategories()
        {
            var categories = new List<string>();
            var procName = GETCATEGORIES;

            using (var dbContext = new SqlCommand(_dbkey, DBSCHEMA))
            {
                using (var reader = dbContext.ExecuteReader(procName, new Dictionary<string, object> { }))
                {
                    try
                    {
                        while (!reader.IsClosed && reader.Read())
                        {
                            var item = ToStringItemByIndex(reader);
                            categories.Add(item);
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

        public List<string> GetTraceLevels()
        {
            var traceLevels = new List<string>();
            var procName = GETTRACELEVELS;

            using (var dbContext = new SqlCommand(_dbkey, DBSCHEMA))
            {
                using (var reader = dbContext.ExecuteReader(procName, new Dictionary<string, object> { }))
                {
                    try
                    {
                        while (!reader.IsClosed && reader.Read())
                        {
                            var item = ToStringItemByIndex(reader);
                            traceLevels.Add(item);
                        }
                    }
                    catch (Exception e)
                    {
                        throw new Exception(string.Format("{0}storedProcedureName:{1}{0}reader.Depth:{2}", NL, procName, reader.Depth), e);
                    }
                }
            }

            return traceLevels;
        }

        
        public List<SummaryItem> GetApplicationNameCounts()
        {
            var applicationNameCounts = new List<SummaryItem>();
            var procName = GETAPPLICATIONNAMECOUNTS;
            SummaryItem summaryItem = null;

            using (var dbContext = new SqlCommand(_dbkey, DBSCHEMA))
            {
                using (var reader = dbContext.ExecuteReader(procName, new Dictionary<string, object> { }))
                {
                    try
                    {
                        while (!reader.IsClosed && reader.Read())
                        {
                            summaryItem = ToSummaryItem(reader);
                            applicationNameCounts.Add(summaryItem);
                        }
                    }
                    catch (Exception e)
                    {
                        throw new Exception(string.Format("{0}storedProcedureName:{1}{0}reader.Depth:{2}", NL, procName, reader.Depth), e);
                    }
                }
            }

            return applicationNameCounts;
        }

        public List<SummaryItem> GetFeatureNameCounts()
        {
            var featureNameCounts = new List<SummaryItem>();
            var procName = GETFEATURENAMECOUNTS;
            SummaryItem summaryItem = null;

            using (var dbContext = new SqlCommand(_dbkey, DBSCHEMA))
            {
                using (var reader = dbContext.ExecuteReader(procName, new Dictionary<string, object> { }))
                {
                    try
                    {
                        while (!reader.IsClosed && reader.Read())
                        {
                            summaryItem = ToSummaryItem(reader);
                            featureNameCounts.Add(summaryItem);
                        }
                    }
                    catch (Exception e)
                    {
                        throw new Exception(string.Format("{0}storedProcedureName:{1}{0}reader.Depth:{2}", NL, procName, reader.Depth), e);
                    }
                }
            }

            return featureNameCounts;
        }

        public List<SummaryItem> GetCategoryCounts()
        {
            List<SummaryItem> categoryCounts = new List<SummaryItem>();
            var procName = GETCATEGORYCOUNTS;
            SummaryItem summaryItem = null;

            using (var dbContext = new SqlCommand(_dbkey, DBSCHEMA))
            {
                using (var reader = dbContext.ExecuteReader(procName, new Dictionary<string, object> { }))
                {
                    try
                    {
                        while (!reader.IsClosed && reader.Read())
                        {
                            summaryItem = ToSummaryItem(reader);
                            categoryCounts.Add(summaryItem);
                        }
                    }
                    catch (Exception e)
                    {
                        throw new Exception(string.Format("{0}storedProcedureName:{1}{0}reader.Depth:{2}", NL, procName, reader.Depth), e);
                    }
                }
            }

            return categoryCounts;
        }

        public AuditLog GetAuditLogById(string id)
        {
            IList<AuditLog> auditLogs = GetAuditLogList(GETAUDITLOGBYID, 
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

            IList<AuditLog> auditLogs = GetAuditLogList(GETAUDITLOGBYAPPLICATIONNAME,
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

            IList<AuditLog> auditLogs = GetAuditLogList(GETAUDITLOGBYCATEGORY,
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

            IList<AuditLog> auditLogs = GetAuditLogList(GETAUDITLOGBYEVENTID,
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

            IList<AuditLog> auditLogs = GetAuditLogList(GETAUDITLOGBYFEATURENAME,
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

            return GetAuditLogList(GETAUDITLOGBYTRACELEVEL,
                new Dictionary<string, object>
                {
                    {"TraceLevel", travelLevel},
                    {"Rowcount", maxRowCount},
                });
        }

        public IList<AuditLog> GetAuditLogByFilters(
            int maxRowCount, 
            string startTime, 
            string endTime, 
            string travelLevel,
            string applicationName)
        {
            if (maxRowCount < 0)
                maxRowCount = Configurations.MaxRowCountDefault;

            return GetAuditLogList(GETAUDITLOGBYFILTERS,
                new Dictionary<string, object>
                {
                    {"Rowcount", maxRowCount},
                    {"StartTime", startTime},
                    {"EndTime", endTime},
                    {"TraceLevel", travelLevel},
                    {"ApplicationName", applicationName},
                });
        }

        public IList<AuditLog> GetByILikeEventId(
            int maxRowCount,
            string startTime,
            string endTime,
            string eventIdSearchStr)
        {
            if (maxRowCount < 0)
                maxRowCount = Configurations.MaxRowCountDefault;

            return GetAuditLogList(GETBYILIKEEVENTID,
                new Dictionary<string, object>
                {
                    {"Rowcount", maxRowCount},
                    {"StartTime", startTime},
                    {"EndTime", endTime},
                    {"EventIdSearchStr", eventIdSearchStr},
                });
        }

        public IList<AuditLog> GetByILikeMessage(
            int maxRowCount,
            string startTime,
            string endTime,
            string messageSearchStr)
        {
            if (maxRowCount < 0)
                maxRowCount = Configurations.MaxRowCountDefault;

            return GetAuditLogList(GETBYILIKEMESSAGE,
                new Dictionary<string, object>
                {
                    {"Rowcount", maxRowCount},
                    {"StartTime", startTime},
                    {"EndTime", endTime},
                    {"MessageSearchStr", messageSearchStr},
                });
        }

        public IList<AuditLog> GetByILikeAdditionalInfo(
            int maxRowCount,
            string startTime,
            string endTime,
            string additionalInfoSearchStr)
        {
            if (maxRowCount < 0)
                maxRowCount = Configurations.MaxRowCountDefault;

            return GetAuditLogList(GETBYILIKEADDITIONALINFO,
                new Dictionary<string, object>
                {
                    {"Rowcount", maxRowCount},
                    {"StartTime", startTime},
                    {"EndTime", endTime},
                    {"AdditionalInfoSearchStr", additionalInfoSearchStr},
                });
        }

        public IList<AuditLog> GetByILikeLoginName(
            int maxRowCount,
            string startTime,
            string endTime,
            string loginNameSearchStr)
        {
            if (maxRowCount < 0)
                maxRowCount = Configurations.MaxRowCountDefault;

            return GetAuditLogList(GETBYILIKELOGINNAME,
                new Dictionary<string, object>
                {
                    {"Rowcount", maxRowCount},
                    {"StartTime", startTime},
                    {"EndTime", endTime},
                    {"LoginNameSearchStr", loginNameSearchStr},
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

        private IList<AuditLog> GetAuditLogList(string storedProcedureName, IDictionary<string, object> parameters)
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

        private string ToStringItem(IDataReader reader)
        {
            string item = StringField(reader, "name");

            if (!string.IsNullOrEmpty(item))
                item = item.Trim();

            return item;
        }

        private string ToStringItemByIndex(IDataReader reader)
        {
            string item = StringField(reader, 0);

            if (!string.IsNullOrEmpty(item))
                item = item.Trim();

            return item;
        }

        private SummaryItem ToSummaryItem(IDataReader reader)
        {
            var summaryItem = new SummaryItem()
            {
                Name = StringField(reader, "name"),
                Count = LongField(reader, "count")
            };

            if (!string.IsNullOrEmpty(summaryItem.Name))
                summaryItem.Name = summaryItem.Name.Trim();

            return summaryItem;
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

        private string StringField(IDataReader reader, int index)
        {
            string val = null;
            try
            {
                val = reader[index].ReturnDefaultOrValue<string>();
            }
            catch (Exception e)
            {
                throw new Exception(string.Format("index: {0}", index), e);
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
