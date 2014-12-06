-- Function: rt.getsummary()

-- DROP FUNCTION rt.getsummary();

CREATE OR REPLACE FUNCTION rt.getsummary()
  RETURNS TABLE(name character varying, count bigint) AS
$BODY$BEGIN
RETURN QUERY

	select al.Category, count(*) 
	from rt.auditlog al
	group by al.Category
	order by al.Category;
	
END
$BODY$
  LANGUAGE plpgsql VOLATILE
  COST 100
  ROWS 1000;
ALTER FUNCTION rt.getsummary()
  OWNER TO postgres;


/*  
select rt.getsummary()
*/
