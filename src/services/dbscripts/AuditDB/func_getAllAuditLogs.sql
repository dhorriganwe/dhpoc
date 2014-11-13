-- Function: rt.getauditlogsall()

-- DROP FUNCTION rt.getauditlogsall();

CREATE OR REPLACE FUNCTION rt.getauditlogsall()
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
	--WHERE am.isdeleted=false
	ORDER BY am.id;
END
$BODY$
  LANGUAGE plpgsql VOLATILE
  COST 100
  ROWS 1000;
ALTER FUNCTION rt.getauditlogsall()
  OWNER TO postgres;

/*  
select rt.getauditlogsall()
*/
