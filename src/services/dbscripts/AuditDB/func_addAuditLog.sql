-- Function: rt.addauditlog(character varying, character varying, character varying, character varying, character varying, text, character varying, character varying)

-- DROP FUNCTION rt.addauditlog(character varying, character varying, character varying, character varying, character varying, text, character varying, character varying);

CREATE OR REPLACE FUNCTION rt.addauditlog(i_eventid character varying, i_applicationname character varying, i_featurename character varying, i_category character varying, i_messagecode character varying, i_messages text, i_tracelevel character varying, i_loginname character varying)
  RETURNS bigint AS
$BODY$
BEGIN
	INSERT INTO rt.AuditLog
	(
		EventId,
		ApplicationName,
		FeatureName,
		Category,
		MessageCode,
		Messages,
		TraceLevel,
		LoginName,
		AuditedOn
	)
	SELECT i_eventId,
		i_applicationname,
		i_featurename,
		i_category,
		i_messagecode,
		i_messages,
		i_tracelevel,
		i_loginname,
		Now();

	RETURN lastval();
END;
$BODY$
  LANGUAGE plpgsql VOLATILE
  COST 100;
ALTER FUNCTION rt.addauditlog(character varying, character varying, character varying, character varying, character varying, text, character varying, character varying)
  OWNER TO postgres;

/*  
select * from rt.auditlog

select rt.addauditlog('event2','appName2','feature2', 'cat2', 'messagecode2', 'some message2', 'tracevel2', 'login2')

select * from rt.auditlog
*/