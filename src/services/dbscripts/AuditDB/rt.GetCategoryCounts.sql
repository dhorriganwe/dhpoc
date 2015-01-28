-- Function: rt.GetCategoryCounts()

-- DROP FUNCTION rt.GetCategoryCounts();

CREATE OR REPLACE FUNCTION rt.GetCategoryCounts()
  RETURNS TABLE(name character varying, 
		count bigint) AS
$BODY$BEGIN
RETURN QUERY

	select al.Category, count(*) 
	from rt.auditlog al
	group by al.Category
	order by al.Category;
	
END
$BODY$
  LANGUAGE plpgsql VOLATILE
  COST 100;
ALTER FUNCTION rt.GetCategoryCounts()
  OWNER TO postgres;

/*  
select rt.GetCategoryCounts()
*/
