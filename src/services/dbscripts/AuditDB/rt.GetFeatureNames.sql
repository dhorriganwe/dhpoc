-- Function: rt.getfeaturenames()

-- DROP FUNCTION rt.getfeaturenames();

CREATE OR REPLACE FUNCTION rt.getfeaturenames()
  RETURNS SETOF character varying AS
$BODY$BEGIN
RETURN QUERY

	select distinct featurename as name
	from rt.auditlog;
	
END
$BODY$
  LANGUAGE plpgsql VOLATILE
  COST 100;
ALTER FUNCTION rt.getfeaturenames()
  OWNER TO postgres;

/*  
select rt.getfeaturenames()
*/
