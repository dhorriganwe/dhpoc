DO 
$$  
BEGIN
	IF NOT EXISTS (SELECT 1 FROM information_schema.tables 
			WHERE table_schema iLike 'rt' AND Table_Name iLike 'auditlog')
	THEN
		CREATE TABLE rt.auditlog
		(
			eventid character varying(50),
			applicationname character varying(500),
			featurename character varying(50),
			category character varying(100),
			messagecode character varying,
			messages text,
			tracelevel character varying(20),
			loginname character varying(100),
			auditedon timestamp without time zone
		)
		WITH (
		  OIDS=FALSE
		);
		
		ALTER TABLE rt.auditlog
			OWNER TO postgres;
	END IF;
END;
$$;


--add coloumn 'additionalinfo' to 'auditlog'  table 
DO
$$
BEGIN   
        IF NOT EXISTS (SELECT COLUMN_NAME FROM information_schema.columns 
			WHERE  table_schema iLike 'rt'  AND table_name iLike 'auditlog' AND column_name iLike 'additionalinfo') 
	THEN
		ALTER TABLE rt.auditlog ADD COLUMN additionalinfo text;
		COMMENT ON COLUMN rt.auditlog.additionalinfo IS 'Add comment';
        END IF;
END
$$;
