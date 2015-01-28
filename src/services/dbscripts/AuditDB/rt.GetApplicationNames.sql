-- Function: rt.getapplicationnames()

-- DROP FUNCTION rt.getapplicationnames();

CREATE OR REPLACE FUNCTION rt.getapplicationnames()
  RETURNS SETOF character varying AS
$BODY$BEGIN
RETURN QUERY

	select distinct applicationname as name
	from rt.auditlog;

END
$BODY$
  LANGUAGE plpgsql VOLATILE
  COST 100
  ROWS 1000;
ALTER FUNCTION rt.getapplicationnames()
  OWNER TO postgres;

/*  
select rt.getapplicationnames()
*/
