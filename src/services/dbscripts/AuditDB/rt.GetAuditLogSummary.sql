-- Function: rt.getauditlogsummary()

-- DROP FUNCTION rt.getauditlogsummary();

CREATE OR REPLACE FUNCTION rt.getauditlogsummary()
  RETURNS TABLE(totalrowcount bigint) AS
$BODY$BEGIN
RETURN QUERY

	SELECT count(*) as totalrowcount
	FROM rt.AuditLog;

	END
$BODY$
  LANGUAGE plpgsql VOLATILE
  COST 100;
ALTER FUNCTION rt.getauditlogsummary()
  OWNER TO postgres;

/*  
select rt.getauditlogsummary()
*/
