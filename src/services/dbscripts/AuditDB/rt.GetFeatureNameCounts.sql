-- Function: rt.getfeaturenamecounts()

-- DROP FUNCTION rt.getfeaturenamecounts();

CREATE OR REPLACE FUNCTION rt.getfeaturenamecounts()
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
ALTER FUNCTION rt.getfeaturenamecounts()
  OWNER TO postgres;

/*  
select rt.getfeaturenamecounts()
*/
