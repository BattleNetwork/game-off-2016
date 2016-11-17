var Player = require('../Models/player.js');
var async = require('async');

var PlayerManager = function () {}

PlayerManager.prototype.playerList = new Array();

PlayerManager.prototype.AddPlayer = function(player, socket)
{
    var newPlayer = new Player(player, socket);
    newPlayer.SetStatusAuthenticated();//il le merite bien non ? 
    this.playerList.push(newPlayer);
}

PlayerManager.prototype.DisconnectPlayer= function(socketid)
{
    var playerList = this.playerList;
    async.detect(playerList, function(player, Callback){
        if(player.socket.id == socketid) Callback(null, true);
        else Callback(null, false);

    }, function(err, playerToDisconnect){
        if(err || !playerToDisconnect) {
            console.log("Error while disconnecting player");
            return;
        }
        // playerToDisconnect.QuitLobby();
        playerList.splice(playerList.indexOf(playerToDisconnect), 1);
    });
}

exports.PlayerManager = new PlayerManager();