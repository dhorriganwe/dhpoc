function MQ(config, db) {

    var amqp = require('./node_modules/amqp'),
         connection = null,
	     queueName = config.QueueName,
	     queue = null,
	     exchange = null,
	 	 db = db;

    function init() {
        connection = amqp.createConnection({ host: config.Host, port: config.Port });
        connection.on('ready', function () {
            AppConfig.EnableLogging && console.log(AppConfig.Messages.MQConnected + config.Host + ', ' + config.Port);
            exchange = connection.exchange(config.ExchangeName);
            queue = connection.queue(queueName, {}, function (message) {
                queue.subscribe(processor);
            });
        });
    }

    function processor(message) {
        AppConfig.EnableLogging && console.log(AppConfig.Messages.MQProcessorMessage + new Date());
        db.insert(message);
    }

    this.publish = function (message) {
        exchange.publish(queueName, message, null, function (err) {
            console.dir(err);
        });
        AppConfig.EnableLogging && console.log(AppConfig.Messages.MQPush + ' Queue : ' + queueName);
    }

    init();
}

exports.MQ = MQ;
