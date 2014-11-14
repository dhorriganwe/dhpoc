-- Function: rt.AddAuditLog(character varying, character varying, character varying, character varying, character varying, text, character varying, character varying)

-- DROP FUNCTION rt.AddAuditLog(character varying, character varying, character varying, character varying, character varying, text, character varying, character varying);

CREATE OR REPLACE FUNCTION rt.AddAuditLog(i_eventId character varying, i_applicationname character varying, i_featurename character varying, i_category character varying, i_messagecode character varying, i_messages text, i_tracelevel character varying, i_loginname character varying)
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
ALTER FUNCTION rt.AddAuditLog(character varying, character varying, character varying, character varying, character varying, text, character varying, character varying)
  OWNER TO postgres;

/*
select rt.getallauditlogs();

select rt.addauditlog('id', 'i_applicationname', 'i_featurename', 'i_category', 'i_messagecode', 'i_messages', 'i_tracelevel', 'i_loginname');
select rt.addauditlog('id2', 'i_applicationname2', 'i_featurename2', 'i_category2', 'i_messagecode2', 'i_messages2', 'i_tracelevel2', 'i_loginname2');

*/