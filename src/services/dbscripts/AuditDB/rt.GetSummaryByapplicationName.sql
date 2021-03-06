
-- Function: rt.GetSummaryByApplicationName()

-- DROP FUNCTION rt.GetSummaryByApplicationName(character varying, character varying);

CREATE OR REPLACE FUNCTION rt.GetSummaryByApplicationName(
                IN i_summaryType character varying, 
                IN i_applicationName character varying)
  RETURNS TABLE(name character varying, 
                                count bigint) AS
$BODY$
BEGIN


BEGIN

IF i_summaryType = 'FeatureName' THEN -- FeatureNames
BEGIN

                RETURN QUERY
                select al.FeatureName, count(*) 
                from rt.auditlog al
                where applicationname = i_applicationName
                group by al.FeatureName
                order by al.FeatureName;
END;
END IF;

IF i_summaryType = 'Category' THEN -- Category
BEGIN
                RETURN QUERY
                select al.Category, count(*) 
                from rt.auditlog al
                where applicationname = i_applicationName
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
ALTER FUNCTION rt.GetSummaryByApplicationName(character varying, character varying)
  OWNER TO postgres;

/*  
select rt.GetSummaryByApplicationName('FeatureName', 'RisingTide')
select rt.GetSummaryByApplicationName('FeatureName', '')
select rt.GetSummaryByApplicationName('Category', 'RisingTide')
select rt.GetSummaryByApplicationName('xxx', 'RisingTide')

*/
