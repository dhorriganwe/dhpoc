﻿-- Function: rt.getauditlogbyfilters(integer, timestamp without time zone, timestamp without time zone, character varying, character varying)

-- DROP FUNCTION rt.getauditlogbyfilters(integer, timestamp without time zone, timestamp without time zone, character varying, character varying);

CREATE OR REPLACE FUNCTION rt.getauditlogbyfilters(IN i_rowcount int, IN i_starttime timestamp without time zone, IN i_endtime timestamp without time zone, IN i_tracelevel character varying, IN i_appName character varying)
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

$BODY$
BEGIN

	IF i_appName = '' THEN
	
		IF i_tracelevel = '' THEN

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
			ORDER BY al.id DESC
			LIMIT i_rowcount;

		ELSE

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
			WHERE al.TraceLevel = i_traceLevel
			and al.AuditedOn > i_starttime
			and al.AuditedOn < i_endtime
			ORDER BY al.id DESC
			LIMIT i_rowcount;

		END IF;  

	ELSE
	
		IF i_tracelevel = '' THEN

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
			and al.ApplicationName = i_appName
			ORDER BY al.id DESC
			LIMIT i_rowcount;

		ELSE

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
			WHERE al.TraceLevel = i_traceLevel
			and al.AuditedOn > i_starttime
			and al.AuditedOn < i_endtime
			and al.ApplicationName = i_appName
			ORDER BY al.id DESC
			LIMIT i_rowcount;

		END IF;  
	END IF;  

END;
$BODY$
  LANGUAGE plpgsql VOLATILE
  COST 100
  ROWS 1000;
ALTER FUNCTION rt.getauditlogbyfilters(int, timestamp without time zone, timestamp without time zone, character varying, character varying)
  OWNER TO postgres;

/*  
select rt.getauditlogbyfilters(10, '1/1/2015', '2/1/2015', 'error', '')
select id, ApplicationName, AuditedOn, TraceLevel, Messages  from rt.getauditlogbyfilters(1000, '1/1/2014', '2/20/2015', '', '')
select id, ApplicationName, AuditedOn, TraceLevel, Messages  from rt.getauditlogbyfilters(1000, '1/1/2014', '2/20/2015', '', 'RisingTide')
select id, ApplicationName, AuditedOn, TraceLevel, Messages  from rt.getauditlogbyfilters(1000, '1/1/2014', '2/20/2015', 'error', 'RisingTide')
select id, ApplicationName, AuditedOn, TraceLevel, Messages  from rt.getauditlogbyfilters(1000, '1/1/2014', '2/20/2015', 'info', '')
*/
