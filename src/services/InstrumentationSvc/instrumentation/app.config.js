exports.Config = {
    DB: {
        User: "postgres",
        Database: "Audit",
        Password: "Aditi01*",
        Host: "localhost",
        Port: "5432",
        Queries: {
            InsertLog: "INSERT INTO rt.auditlog " +
                "(eventid, applicationname, featurename,category,messages,tracelevel,additionalinfo,auditedon,messagecode,loginname) values($1, $2, $3, $4, $5, $6, $7, $8, $9, $10)"
        }
    },
    Queue:
	{
	    ExchangeName: "",
	    QueueName: "RisingTide.Instrumentation.Queue",
	    Host: "localhost",
	    Port: "5672"
	},
    Service:
	{
	    Host: "localhost",
	    Port: "9000",
	    LogMethodRoute: "/api/logs",
	    Name: "RisingTide Instrumentation Service",
	    Version: '1.0.0'
	},
    EnableLogging: true,
    Messages: {
        LogPushSuccess: 'Log entry successfully queued for processing.',
        InvalidSchema: 'Log entry JSON schmema invalid, queue push failed.',
        HTTPServerRunning: 'HTTP started listening  on  host, port : ',
        DBConnectionError: 'Error connecting database.',
        DBConnected: 'Database connection established on host, port : ',
        DBQueryExecutionError: 'Error executing query.',
        MQConnected: 'Message queue connection established on host, port : ',
        MQProcessorMessage: 'A new log message was picked up by the queue processor for processing.',
        MQPush: 'Log entry was pushed to the MQ server.'
    },
    Schema: {
        type: 'object',
        properties: {
            EventId: { type: String, required: true },
            ApplicationName: { type: String, required: true },
            FeatureName: { type: String, required: true },
            Category: { type: String, required: true },
            Messages: { type: [], required: true },
            TraceLevel: { type: String, enum: ['off', 'error', 'warning', 'info', 'verbose','Warning','Error','Exception','exception','Debug', 'Information', 'information'] },
            AdditionalInfo: { type: [], required: true },
            MessageCode: { type: String, required: false },
            LoginName:  { type: String, required: false }
        }
    }
};

