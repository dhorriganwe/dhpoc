\encoding UTF8
\connect Audit
\encoding UTF8

\ir SchemaChanges.sql

\ir ./rt.AddAuditLog.sql
\ir ./rt.GetAuditLogById.sql
\ir ./rt.GetAuditLogsAll.sql
\ir ./rt.GetAuditLogsByEventId.sql
\ir ./rt.GetAuditLogsByTraceLevel.sql
