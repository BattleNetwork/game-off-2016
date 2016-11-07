var playerManager = require('../lib/playerManager.js').PlayerManager;
var dbManager = require('../lib/dbManager.js').DBManager;

module.exports = function(io)
{
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

        var result = dbManager.GetPlayerAuthentication(pseudo);

        if (result.err || !result.player) return callback(new Error("User not found"));
        return callback(null, player.password == password);
    }

    function postAuthenticate(socket, data) {
        var pseudo = data.pseudo;
        
        var result = dbManager.GetPlayerProfil(pseudo);
        
        var newplayer = playerManager.AddPlayer(result.content, socket);  
    }

    function disconnect(socket) {
        playerManager.DisconnectPlayer(socket.id);
        console.log(socket.id + ' disconnected');
    }   
}