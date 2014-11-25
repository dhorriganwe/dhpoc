-- Function: rt.GetCategories()

-- DROP FUNCTION rt.GetCategories();

CREATE OR REPLACE FUNCTION rt.GetCategories()
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
ALTER FUNCTION rt.GetCategories()
  OWNER TO postgres;

/*  
select rt.GetCategories()
*/
