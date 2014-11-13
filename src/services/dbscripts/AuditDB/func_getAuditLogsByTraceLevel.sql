
CREATE OR REPLACE FUNCTION rt.getAuditLogsByTraceLevel(i_traceLevel character varying)
  RETURNS TABLE(id bigint, eventid character varying, applicationname character varying, featurename character varying, category character varying, messagecode character varying, messages text, tracelevel character varying, loginname character varying, auditedon timestamp without time zone) AS
$BODY$BEGIN
RETURN QUERY
	SELECT am.id
		,am.EventId
		,am.ApplicationName
		,am.FeatureName
		,am.Category
		,am.MessageCode
		,am.Messages
		,am.TraceLevel 
		,am.LoginName 
		,am.AuditedOn 
	FROM rt.AuditLog am 
	WHERE am.TraceLevel = i_traceLevel
	ORDER BY am.id;
END
$BODY$
  LANGUAGE plpgsql VOLATILE
  COST 100
  ROWS 1000;
ALTER FUNCTION rt.getAuditLogsByTraceLevel(character varying)
  OWNER TO postgres;

/*  
select rt.getAuditLogsByTraceLevel('error')
*/
