-- Function: rt.getauditlogbyid(bigint)

-- DROP FUNCTION rt.getauditlogbyid(bigint);

CREATE OR REPLACE FUNCTION rt.getauditlogbyid(IN i_id bigint)
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
	WHERE al.Id = i_Id
	ORDER BY al.id;
END
$BODY$
  LANGUAGE plpgsql VOLATILE
  COST 100;
ALTER FUNCTION rt.getauditlogbyid(bigint)
  OWNER TO postgres;

/*  
select rt.getAuditLogById(208000)
*/
