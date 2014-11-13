SELECT RT.DeleteGivenFunction('getallauditlogs');
-- Function: rt.getallauditlogs()

-- DROP FUNCTION rt.getallauditlogs();

CREATE OR REPLACE FUNCTION rt.getallauditlogs()
  RETURNS TABLE(id integer, message character varying, isdefault boolean) AS
$BODY$BEGIN
RETURN QUERY
	SELECT am.id
		,am.title
		,am.isdefault 
	FROM rt.applicationmethod am 
	WHERE am.isdeleted=false
	ORDER BY am.displayorder;
END
$BODY$
  LANGUAGE plpgsql VOLATILE
  COST 100
  ROWS 1000;
ALTER FUNCTION rt.getallauditlogs()
  OWNER TO postgres;
