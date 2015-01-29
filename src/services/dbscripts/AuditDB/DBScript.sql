\encoding UTF8
\connect Audit
\encoding UTF8

\ir SchemaChanges.sql

\ir ./rt.AddAuditLog.sql
\ir ./rt.GetApplicationNames.sql
\ir ./rt.GetAuditLogAll.sql
\ir ./rt.GetAuditLogByApplicationName.sql
\ir ./rt.GetAuditLogByCategory.sql

\ir ./rt.GetAuditLogByEventId.sql
\ir ./rt.GetAuditLogByFeatureName.sql
\ir ./rt.GetAuditLogByFilters.sql
\ir ./rt.GetAuditLogById.sql
\ir ./rt.GetAuditLogByTraceLevel.sql
\ir ./rt.GetAuditLogRowCount.sql

\ir ./rt.GetCategories.sql
\ir ./rt.GetFeatureNames.sql
\ir ./rt.GetSummaryByApplicationName.sql
\ir ./rt.GetSummaryByCategory.sql
\ir ./rt.GetSummaryByFeatureName.sql
