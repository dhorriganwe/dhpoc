-- Function: rt.gettracelevels()

-- DROP FUNCTION rt.gettracelevels();

CREATE OR REPLACE FUNCTION rt.gettracelevels()
  RETURNS SETOF character varying AS
$BODY$BEGIN
RETURN QUERY

	select distinct tracelevel as tracecode
	from rt.auditlog;
	
END
$BODY$
  LANGUAGE plpgsql VOLATILE
  COST 100
  ROWS 1000;
ALTER FUNCTION rt.gettracelevels()
  OWNER TO postgres;

/*  
select tracecode from rt.GetTraceLevels()
*/
