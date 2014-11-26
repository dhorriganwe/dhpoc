-- Function: rt.getauditlogbyeventid(character varying, int)

-- DROP FUNCTION rt.getauditlogbyeventid(character varying, int);

CREATE OR REPLACE FUNCTION rt.getauditlogbyeventid(IN i_eventId character varying, IN i_rowcount int)
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
	WHERE al.EventId = i_eventId
	ORDER BY al.id DESC
	LIMIT i_rowcount;
END
$BODY$
  LANGUAGE plpgsql VOLATILE
  COST 100;
ALTER FUNCTION rt.getauditlogbyeventid(character varying, int)
  OWNER TO postgres;

/*  
select rt.getauditlogbyeventid('7b3d299e-7ed0-4d1d-ae14-51bb7b345df8', 100)
*/
