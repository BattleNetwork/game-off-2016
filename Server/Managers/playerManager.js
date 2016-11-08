var Player = require('../Models/player.js');
var async = require('async');

var PlayerManager = function () {}

PlayerManager.prototype.playerList = new Array();

PlayerManager.prototype.AddPlayer = function(player, socket)
{
    var newPlayer = new Player(player, socket);
    newPlayer.SetStatusAuthenticated();
    this.playerList.push(newPlayer);
}

PlayerManager.prototype.DisconnectPlayer= function(socketid)
{
    var playerList = this.playerList;
    async.detect(playerList, function(player, callback){
        if(player.socket.id == socketid) callback(null, true);
        else callback(null, false);

    }, function(err, playerToDisconnect){
        if(err || !playerToDisconnect) {
            console.log("Error while disconnecting player");
            return;
        } 
        playerList.splice(playerList.indexOf(playerToDisconnect), 1);
    });
}

exports.PlayerManager = new PlayerManager();