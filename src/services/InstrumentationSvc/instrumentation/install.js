var Service = require('./node_modules/node-windows').Service;

// Create a new service object
var svc = new Service({
  name:'Rising Tide Instrumentation Service',
  description: 'Listens log requests from Rising Tide application',
  script: require('path').join(__dirname,'risingtide.instrumentation.service.js'),
  env:{
    name: "NODE_ENV",
    value: "development"
  }
});

// Listen for the "install" event, which indicates the
// process is available as a service.
svc.on('install',function(){
  svc.start();
});

// Just in case this file is run twice.
svc.on('alreadyinstalled',function(){
  console.log('This service is already installed.');
});

// Listen for the "start" event and let us know when the
// process has actually started working.
svc.on('start',function(){
  console.log('"' +svc.name +'" installed and started successfully');
});

// Install the script as a service.
svc.install();