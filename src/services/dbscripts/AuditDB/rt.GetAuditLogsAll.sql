--SELECT RT.DeleteGivenFunction('getauditlogsall');
-- Function: rt.getauditlogsall()

-- DROP FUNCTION rt.getauditlogsall();

CREATE OR REPLACE FUNCTION rt.getauditlogsall()
  RETURNS TABLE(	id bigint, 
			eventId character varying, 
			applicationname character varying, 
			featurename character varying, 
			category character varying, 
			messagecode character varying, 
			messages text, 
			additionalinfo text, 
			tracelevel character varying, 
			loginname character varying, 
			auditedOn timestamp without time zone) AS
$BODY$BEGIN
RETURN QUERY
	SELECT	al.Id
		,al.EventId
		,al.ApplicationName
		,al.FeatureName
		,al.Category
		,al.MessageCode
		,al.Messages
		,al.AdditionalInfo
		,al.TraceLevel 
		,al.LoginName 
		,al.AuditedOn 
	FROM rt.AuditLog al
	ORDER BY al.id DESC
	LIMIT 20;
END
$BODY$
  LANGUAGE plpgsql VOLATILE
  COST 100;
ALTER FUNCTION rt.getauditlogsall()
  OWNER TO postgres;

-- select rt.getauditlogsall()
