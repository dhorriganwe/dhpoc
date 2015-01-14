var Service = require('./node_modules/node-windows').Service;

// Create a new service object
var svc = new Service({
  name:'Rising Tide Instrumentation Service',
  script: require('path').join(__dirname,'risingtide.instrumentation.service.js')
});

// Listen for the "uninstall" event so we know when it's done.
svc.on('uninstall',function(){
  console.log('"' +svc.name +'" uninstalled successfully');  
});

// Uninstall the service.
svc.uninstall();