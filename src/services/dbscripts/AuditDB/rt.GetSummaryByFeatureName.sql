
-- Function: rt.GetSummaryByFeatureName()

-- DROP FUNCTION rt.GetSummaryByFeatureName(character varying, character varying);

CREATE OR REPLACE FUNCTION rt.GetSummaryByFeatureName(
                IN i_summaryType character varying, 
                IN i_featureName character varying)
  RETURNS TABLE(name character varying, 
                                count bigint) AS
$BODY$
BEGIN


BEGIN

IF i_summaryType = 'ApplicationName' THEN -- ApplicationNames
BEGIN

                RETURN QUERY
                select al.ApplicationName, count(*) 
                from rt.auditlog al
                where featurename = i_featureName
                group by al.ApplicationName
                order by al.ApplicationName;
END;
END IF;

IF i_summaryType = 'Category' THEN -- Category
BEGIN
                RETURN QUERY
                select al.Category, count(*) 
                from rt.auditlog al
                where featurename = i_featureName
                group by al.Category
                order by al.Category;
END;
END IF;

RAISE NOTICE 'Invalid Summary Type(%)', i_summaryType;

END;
END
$BODY$
  LANGUAGE plpgsql VOLATILE
  COST 100;
ALTER FUNCTION rt.GetSummaryByFeatureName(character varying, character varying)
  OWNER TO postgres;

/*  
select rt.GetSummaryByFeatureName('ApplicationName', 'AccessPermission')
select rt.GetSummaryByFeatureName('Category', 'AccessPermission')
select rt.GetSummaryByFeatureName('xxx', 'AccessPermission')

*/
