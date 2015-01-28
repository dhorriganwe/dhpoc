-- Function: rt.getapplicationnamecounts()

-- DROP FUNCTION rt.getapplicationnamecounts();

CREATE OR REPLACE FUNCTION rt.getapplicationnamecounts()
  RETURNS TABLE(name character varying, 
		count bigint) AS
$BODY$BEGIN
RETURN QUERY

	select al.applicationname, count(*) 
	from rt.auditlog al
	group by al.applicationname
	order by al.applicationname;
	
END
$BODY$
  LANGUAGE plpgsql VOLATILE
  COST 100;
ALTER FUNCTION rt.getapplicationnamecounts()
  OWNER TO postgres;

/*  
select rt.getapplicationnamecounts()
*/
