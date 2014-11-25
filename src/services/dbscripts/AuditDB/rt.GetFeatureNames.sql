-- Function: rt.getfeaturenames()

-- DROP FUNCTION rt.getfeaturenames();

CREATE OR REPLACE FUNCTION rt.getfeaturenames()
  RETURNS TABLE(name character varying, 
		count bigint) AS
$BODY$BEGIN
RETURN QUERY

	select al.featurename, count(*) 
	from rt.auditlog al
	group by al.featurename
	order by al.featurename;
	
END
$BODY$
  LANGUAGE plpgsql VOLATILE
  COST 100;
ALTER FUNCTION rt.getfeaturenames()
  OWNER TO postgres;

/*  
select rt.getfeaturenames()
*/
