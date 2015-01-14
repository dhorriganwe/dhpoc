function DB(config) {
    //private members
    var pg = require('./node_modules/pg'),
        config = config;

    //private methods
    function init() {
        pgClient = new pg.Client(
			{ 	user: config.User, 
				password: config.Password, 
				database: config.Database, 
				host: config.Host, 
				port: config.Port 
			});
        pgClient.connect(function (err) {
            err &&  console.log(AppConfig.Messages.DBConnectionError + '\n' + err);
            AppConfig.EnableLogging && console.log(AppConfig.Messages.DBConnected + config.Host + ', ' + config.Port);
        });
    }

    this.insert = function (message) {

	var query = pgClient.query(config.Queries.InsertLog, [message.EventId, message.ApplicationName, message.FeatureName, message.Category, message.Messages, message.TraceLevel,message.AdditionalInfo, message.AuditedOn], function (err, result) {
		try {
			err && console.log(AppConfig.Messages.DBQueryExecutionError + '\n' + config.Queries.InsertLog  + '\n' + err);
			if(result.command)
				AppConfig.EnableLogging && console.log(result.command + ' at : ' + new Date());
			else {
				console.log('Result: *****************************************' + JSON.stringify(result));
			}
		} catch (e) {
			console.log('Exception: *****************************************' + e + new Date());
		}
	});

	query.on('error', function (error) {
		console.log(AppConfig.Messages.DBQueryExecutionError + '\n' + config.Queries.InsertLog + '\n' + err);
	});

    }

    init();
}

exports.DB = DB;
