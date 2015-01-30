-- Function: rt.getbyilikemessage(integer, timestamp without time zone, timestamp without time zone, character varying)

-- DROP FUNCTION rt.getbyilikemessage(integer, timestamp without time zone, timestamp without time zone, character varying);

CREATE OR REPLACE FUNCTION rt.getbyilikemessage(IN i_rowcount integer, IN i_starttime timestamp without time zone, IN i_endtime timestamp without time zone, IN i_messagesearchstr character varying)
  RETURNS TABLE(id bigint, eventid character varying, applicationname character varying, featurename character varying, category character varying, messagecode character varying, messages text, tracelevel character varying, loginname character varying, auditedon timestamp without time zone, additionalinfo text) AS
$BODY$
BEGIN

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
			WHERE al.AuditedOn > i_starttime
			and al.AuditedOn < i_endtime
			and al.Messages ilike concat('%', i_messagesearchstr, '%')
			ORDER BY al.id DESC
			LIMIT i_rowcount;

END;
$BODY$
  LANGUAGE plpgsql VOLATILE
  COST 100
  ROWS 1000;
ALTER FUNCTION rt.getbyilikemessage(integer, timestamp without time zone, timestamp without time zone, character varying)
  OWNER TO postgres;
/*  
select rt.getbyilikemessage(100, '1/1/2015', '2/1/2015', 'adv')
select id, AuditedOn, TraceLevel, Messages, EventId  from rt.getbyilikemessage(100, '1/1/2015', '2/1/2015', 'advanced')
*/
