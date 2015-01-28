-- Function: rt.GetCategories()

-- DROP FUNCTION rt.GetCategories();

CREATE OR REPLACE FUNCTION rt.GetCategories()
  RETURNS SETOF character varying AS
$BODY$BEGIN
RETURN QUERY

	select distinct Category as name
	from rt.auditlog;

END
$BODY$
  LANGUAGE plpgsql VOLATILE
  COST 100;
ALTER FUNCTION rt.GetCategories()
  OWNER TO postgres;

/*  
select rt.GetCategories()
*/
