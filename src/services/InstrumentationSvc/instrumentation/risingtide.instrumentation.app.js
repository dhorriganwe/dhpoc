function App(config, mq) {
    var restify = require('./node_modules/restify'),
	    App = null,
	    jsonSchemaValidator = require('./node_modules/json-schema'),
	    mq = mq,
	    port = config.Port,
	    host = config.Host,
	    name = config.Name,
	    version = config.Version;

    function initServer() {
        App = restify.createServer({
            name: name,
            version: version
        });
        App.use(restify.acceptParser(App.acceptable));
    }
    
    function registerRoutes() {
        App.post(config.LogMethodRoute, restify.bodyParser(), function (req, res) {
            handleRequest(req, res, req.body);
        });

        App.get(config.LogMethodRoute, restify.queryParser(), function (req, res) {
            handleRequest(req, res, req.query, !!(req.query.callback));
        });

        App.get('/', restify.serveStatic({
            directory: './public/', default: 'index.html'
        }));
    }

    function handleRequest(req, res, logentry, isJSONPReq) {
        if (jsonSchemaValidator.validate(logentry, AppConfig.Schema).valid) {
            mq.publish(logentry);
            isJSONPReq &&  res.header('Content-Type', 'application/x-javascript');
            res.send(req.query.callback + "('" + JSON.stringify({ Response: AppConfig.Messages.LogPushSuccess }) + "');");
        } else {
            res.send(JSON.stringify({ Response: AppConfig.Messages.InvalidSchema }));
            AppConfig.EnableLogging && console.log( '>>' + AppConfig.Messages.InvalidSchema + ' for ' +  JSON.stringify(logentry) + '<<');
        }
    }

    this.start = function () {
        App.listen(port);
        
        console.log('===========================================');
        console.log( name + '(' + version + ')' );
        console.log('===========================================');

        AppConfig.EnableLogging && console.log(AppConfig.Messages.HTTPServerRunning + host + ', ' + port);
	}

    initServer();
    registerRoutes();
}

exports.App = App;

