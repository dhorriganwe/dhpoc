-- Function: rt.GetAuditLogByApplicationName(character varying)

-- DROP FUNCTION rt.GetAuditLogByApplicationName(character varying);

CREATE OR REPLACE FUNCTION rt.GetAuditLogByApplicationName(IN i_applicationName character varying)
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
	SELECT al.Id
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
	WHERE al.ApplicationName = i_applicationName
	ORDER BY al.id;
END
$BODY$
  LANGUAGE plpgsql VOLATILE
  COST 100;
ALTER FUNCTION rt.GetAuditLogByApplicationName(character varying)
  OWNER TO postgres;

/*  
select rt.GetAuditLogByApplicationName('AgVerdict-AdvancedRec')
*/
