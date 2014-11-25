-- Function: rt.GetAuditLogByCategory(character varying)

-- DROP FUNCTION rt.GetAuditLogByCategory(character varying);

CREATE OR REPLACE FUNCTION rt.GetAuditLogByCategory(IN i_category character varying)
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
	WHERE al.Category = i_category
	ORDER BY al.id;
END
$BODY$
  LANGUAGE plpgsql VOLATILE
  COST 100;
ALTER FUNCTION rt.GetAuditLogByCategory(character varying)
  OWNER TO postgres;

/*  
select rt.GetAuditLogByCategory('Web unhandled exception')
*/
