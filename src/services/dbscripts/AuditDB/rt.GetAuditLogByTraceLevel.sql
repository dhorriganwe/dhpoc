-- Function: rt.getauditlogbytracelevel(character varying, int)

-- DROP FUNCTION rt.getauditlogbytracelevel(character varying, int);

CREATE OR REPLACE FUNCTION rt.getauditlogbytracelevel(IN i_tracelevel character varying, IN i_rowcount int)
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
	ORDER BY al.id DESC
	LIMIT i_rowcount;
END
$BODY$
  LANGUAGE plpgsql VOLATILE
  COST 100;
ALTER FUNCTION rt.getauditlogbytracelevel(character varying, int)
  OWNER TO postgres;

/*  
select rt.getauditlogbytracelevel('error', 100)
*/
