-- Function: rt.getauditlogsbytracelevel(character varying)

-- DROP FUNCTION rt.getauditlogsbytracelevel(character varying);

CREATE OR REPLACE FUNCTION rt.getauditlogsbytracelevel(IN i_tracelevel character varying)
  RETURNS TABLE(	id bigint, 
					eventid character varying, 
					applicationname character varying, 
					featurename character varying, 
					category character varying, 
					messagecode character varying, 
					messages text, 
					tracelevel character varying, 
					loginname character varying, 
					auditedon timestamp without time zone,
					AdditionalInfo text) AS

$BODY$BEGIN
RETURN QUERY
	SELECT al.id
		,al.EventId
		,al.ApplicationName
		,al.FeatureName
		,al.Category
		,al.MessageCode
		,al.Messages
		,al.TraceLevel 
		,al.LoginName 
		,al.AuditedOn 
		,al.AdditionalInfo
	FROM rt.AuditLog al 
	WHERE al.TraceLevel = i_traceLevel
	ORDER BY al.id;
END
$BODY$
  LANGUAGE plpgsql VOLATILE
  COST 100;
ALTER FUNCTION rt.getauditlogsbytracelevel(character varying)
  OWNER TO postgres;

/*  
select rt.getAuditLogsByTraceLevel('error')
*/
