var app = require('express')();
var http = require('http').Server(app);
var io = require('socket.io');
io = io.listen(http);

require('./lib/app.js')(io);

http.listen(3000, function(){
  console.log('listening on *:3000');
});