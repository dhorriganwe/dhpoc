-- Function: rt.GetAuditLogRowCount()

-- DROP FUNCTION rt.GetAuditLogRowCount();

CREATE OR REPLACE FUNCTION rt.GetAuditLogRowCount()
  RETURNS TABLE(totalrowcount bigint) AS
$BODY$BEGIN
RETURN QUERY

	SELECT count(*) as totalrowcount
	FROM rt.AuditLog;

	END
$BODY$
  LANGUAGE plpgsql VOLATILE
  COST 100;
ALTER FUNCTION rt.GetAuditLogRowCount()
  OWNER TO postgres;

/*  
select rt.GetAuditLogRowCount()
*/
