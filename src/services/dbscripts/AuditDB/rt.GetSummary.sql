-- Function: rt.getsummary()

-- DROP FUNCTION rt.rt.getsummary();

CREATE OR REPLACE FUNCTION rt.GetSummary(IN i_applicationName character varying, IN i_featureName character varying, IN i_category character varying)
  RETURNS TABLE(
		applicationName character varying, 
		featureName character varying, 
		category character varying) AS
$BODY$BEGIN
RETURN QUERY

	select al.applicationname, al.featurename, al.category, count(*) 
	from rt.auditlog al
	group by al.applicationname
	order by al.applicationname;
	
END
$BODY$
  LANGUAGE plpgsql VOLATILE
  COST 100;
ALTER FUNCTION rt.getsummary()
  OWNER TO postgres;

/*  
select rt.getsummary()
*/
