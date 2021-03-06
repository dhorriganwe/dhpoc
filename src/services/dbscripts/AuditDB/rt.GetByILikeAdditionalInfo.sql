﻿-- Function: rt.getbyilikeadditionalinfo(integer, timestamp without time zone, timestamp without time zone, character varying)

-- DROP FUNCTION rt.getbyilikeadditionalinfo(integer, timestamp without time zone, timestamp without time zone, character varying);

CREATE OR REPLACE FUNCTION rt.getbyilikeadditionalinfo(IN i_rowcount integer, IN i_starttime timestamp without time zone, IN i_endtime timestamp without time zone, IN i_additionalinfosearchstr character varying)
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
			and al.AdditionalInfo ilike concat('%', i_additionalinfosearchstr, '%')
			ORDER BY al.id DESC
			LIMIT i_rowcount;

END;
$BODY$
  LANGUAGE plpgsql VOLATILE
  COST 100
  ROWS 1000;
ALTER FUNCTION rt.getbyilikeadditionalinfo(integer, timestamp without time zone, timestamp without time zone, character varying)
  OWNER TO postgres;
/*  
select rt.getbyilikeadditionalinfo(100, '1/1/2015', '2/1/2015', 'LANNATE SP')
select id, AuditedOn, TraceLevel, Messages, EventId, AdditionalInfo from rt.getbyilikeadditionalinfo(100, '1/1/2015', '2/1/2015', 'LANNATE SP')
*/
