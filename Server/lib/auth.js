var mongoose = require('mongoose');
mongoose.Promise = require('bluebird');

var config = require('../config/config.js');

var lobbyManager = require('../lib/lobby.js').LobbyManager;

module.exports = function(io)
{
    mongoose.connect(config.dbserver);
    var db = mongoose.connection;
    db.on('error', console.error.bind(console, 'connection error:'));
    db.once('open', function() {
        var Schema = mongoose.Schema;
    
        var playerSchema = new Schema({
            pseudo        : String,
            password      : String,
            registerdate  : Date
        });

        var playerModel = db.model('Player', playerSchema);

        require('socketio-auth')(io, {
            authenticate: authenticate,
            postAuthenticate: postAuthenticate,
            disconnect: disconnect,
            timeout: 1000
        });

        function authenticate(socket, data, callback) {
            var pseudo = data.pseudo;
            var password = data.password;
            console.log("pseudo :" + pseudo + " / pass : " + password);

            playerModel.findOne({ 'pseudo': pseudo }, 'pseudo password', function(err, player) {
                if (err || !player) return callback(new Error("User not found"));
                return callback(null, player.password == password);
            });
        }

        function postAuthenticate(socket, data) {
            var pseudo = data.pseudo;
            
            playerModel.findOne({ 'pseudo': pseudo }, 'pseudo registerdate', function(err, player) {
                socket.playerData = player;
                socket.on('listlobby', function(parameters){
                    socket.emit('lobbylist', JSON.stringify(lobbyManager.lobbyList));
                });
                socket.on('createlobby', function(parameters){
                    lobbyManager.AddLobby(parameters.lobbyName);
                    socket.join(parameters.lobbyName);
                    socket.emit('lobbycreated');
                });
                socket.on('joinlobby', function(parameters){
                    if(lobbyManager.LobbyExist(parameters.lobbyName))
                    {
                        socket.join(parameters.lobbyName);
                        socket.emit('lobbyjoined');
                    }
                    else {
                        socket.emit('lobbydontexist');
                    }
                });
            });
        }

        function disconnect(socket) {
            console.log(socket.id + ' disconnected');
            if(socket.playerData != undefined) playerModel.update({'pseudo':socket.playerData.pseudo}, {$set: JSON.stringify(socket.playerData)});
        }
    });
    
}