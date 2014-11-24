--------------- Add Id identity column to rt.AuditLog  ---------------------------------------------------------------------------------

DO
$BODY$
BEGIN
IF NOT EXISTS (SELECT 1 FROM Information_Schema.Columns
	WHERE Table_Schema ILIKE 'RT' AND Table_Name ILIKE 'AuditLog' AND Column_Name ILIKE 'Id')
THEN
	ALTER TABLE RT.AuditLog ADD COLUMN Id bigserial NOT NULL;
END IF;
END;
$BODY$;

--------------- Make Id column PrimaryKey of rt.AuditLog  ---------------------------------------------------------------------------------

DO
$BODY$
BEGIN
IF NOT EXISTS (
SELECT 1 FROM Information_Schema.Table_Constraints WHERE Table_Schema ILIKE 'RT' AND  Constraint_Name ILIKE 'pk_AuditLog'
)
THEN
	ALTER TABLE rt.AuditLog ADD CONSTRAINT pk_AuditLog PRIMARY KEY (Id);
END IF;
END;
$BODY$;

-----------------------------------------------------------------------------------------------------------------------------------------------------

--------------- Add AdditionalInfo text column to rt.AuditLog  ---------------------------------------------------------------------------------

DO
$BODY$
BEGIN
IF NOT EXISTS (SELECT 1 FROM Information_Schema.Columns
	WHERE Table_Schema ILIKE 'RT' AND Table_Name ILIKE 'AuditLog' AND Column_Name ILIKE 'AdditionalInfo')
THEN
	ALTER TABLE RT.AuditLog ADD COLUMN AdditionalInfo text;
END IF;
END;
$BODY$;

