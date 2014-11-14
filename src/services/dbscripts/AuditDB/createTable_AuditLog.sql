-- Table: rt.auditlog

-- DROP TABLE rt.auditlog;

CREATE TABLE rt.auditlog
(
  id bigserial NOT NULL,
  eventid character varying(50),
  applicationname character varying(500),
  featurename character varying(50),
  category character varying(100),
  messagecode character varying,
  messages text,
  tracelevel character varying(20),
  loginname character varying(100),
  auditedon timestamp without time zone,
  additionalinfo text, -- Add comment
  CONSTRAINT pk_auditlog PRIMARY KEY (id)
)
WITH (
  OIDS=FALSE
);
ALTER TABLE rt.auditlog
  OWNER TO postgres;
COMMENT ON COLUMN rt.auditlog.additionalinfo IS 'Add comment';

/*
INSERT INTO rt.auditlog(eventid, applicationname, featurename, category, messagecode, messages, tracelevel, loginname, auditedon, additionalinfo) 
SELECT 'event1','appName1','feature1', 'cat1', 'messagecode1', 'some message', 'tracevel1', 'login1', now(), 'some comment'

select * from rt.auditlog
*/