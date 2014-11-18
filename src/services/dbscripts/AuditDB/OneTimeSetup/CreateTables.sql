------------------------------------------------------------------------------------------------------------------------
DO
$BODY$
BEGIN
	IF NOT EXISTS (SELECT 1 FROM information_schema.tables WHERE table_schema ilike 'rt' AND Table_Name ilike 'auditlog')
	THEN
	-- Table: rt.auditlog

	-- DROP TABLE rt.auditlog;

		CREATE TABLE rt.auditlog
		(
		  Id bigserial NOT NULL,
		  EventId character varying(50),
		  ApplicationName character varying(500),
		  FeatureName character varying(50),
		  Category character varying(100),
		  MessageCode character varying,
		  Messages text,
		  AdditionalInfo text,
		  TraceLevel character varying(20),
		  LoginName character varying(100),
		  AuditedOn timestamp without time zone,
		  CONSTRAINT pk_auditlog PRIMARY KEY (id)
		)
		WITH (
		  OIDS=FALSE
		);
		ALTER TABLE rt.auditlog
		  OWNER TO postgres;

	END IF;
END;
$BODY$;

ALTER TABLE rt.auditlog
	OWNER TO postgres;

------------------------------------------------------------------------------------------------------------------------
