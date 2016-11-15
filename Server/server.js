var app = require('express')();
var http = require('http').Server(app);

var bodyParser = require('body-parser');
app.use(bodyParser.urlencoded({
    extended: true
}));
app.use(bodyParser.json()); // for parsing application/json

var io = require('socket.io');
io = io.listen(http);

require('./gameserver.js')(io, app);

http.listen(3000, function(){
  console.log('listening on *:3000');
});