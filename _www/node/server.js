var connect = require('connect');
var serveStatic = require('serve-static');
connect().use(serveStatic('..')).listen(8080, '0.0.0.0');