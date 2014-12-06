
-- Function: rt.GetSummaryByCategory()

-- DROP FUNCTION rt.GetSummaryByCategory(character varying, character varying);

CREATE OR REPLACE FUNCTION rt.GetSummaryByCategory(
                IN i_summaryType character varying, 
                IN i_category character varying)
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
                where category = i_category
                group by al.ApplicationName
                order by al.ApplicationName;
END;
END IF;

IF i_summaryType = 'FeatureName' THEN -- FeatureNames
BEGIN
                RETURN QUERY
                select al.FeatureName, count(*) 
                from rt.auditlog al
                where category = i_category
                group by al.FeatureName
                order by al.FeatureName;
END;
END IF;

RAISE NOTICE 'Invalid Summary Type(%)', i_summaryType;

END;
END
$BODY$
  LANGUAGE plpgsql VOLATILE
  COST 100;
ALTER FUNCTION rt.GetSummaryByCategory(character varying, character varying)
  OWNER TO postgres;

/*  
select rt.GetSummaryByCategory('ApplicationName', 'Authentication')
select rt.GetSummaryByCategory('FeatureName', 'Authentication')
select rt.GetSummaryByCategory('xxx', 'Authentication')

*/
