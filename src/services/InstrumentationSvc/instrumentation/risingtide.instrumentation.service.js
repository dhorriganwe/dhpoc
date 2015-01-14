var optimist = require('./node_modules/optimist');
var RisingTide = RisingTide || {};
RisingTide.Instrumentation = {};
RisingTide.Instrumentation.App = {};
RisingTide.Instrumentation.DB = {};
RisingTide.Instrumentation.MQ = {};
RisingTide.Instrumentation.Config = {};


RisingTide.Instrumentation.App = require('./RisingTide.Instrumentation.App').App;
RisingTide.Instrumentation.DB = require('./RisingTide.Instrumentation.DB').DB;
RisingTide.Instrumentation.MQ = require('./RisingTide.Instrumentation.Queue').MQ;

GLOBAL.AppConfig = require('./app.config').Config;

var db = new RisingTide.Instrumentation.DB(AppConfig.DB);
var mq = new RisingTide.Instrumentation.MQ(AppConfig.Queue, db);
var app = new RisingTide.Instrumentation.App(AppConfig.Service, mq);

app.start();
