var Lobby = require('../Models/lobby.js');

var LobbyManager = function () {};

LobbyManager.prototype.lobbyList = new Array();

LobbyManager.prototype.AddLobby = function(name) {
    var newLobby = new Lobby(name);
    this.lobbyList.push(newLobby);
    return newLobby;
};

LobbyManager.prototype.LobbyExist = function(name) {
   var lobby = this.lobbyList.find(CheckLobby, [name]) ;
   var exist = lobby != undefined;
    return exist;
};

LobbyManager.prototype.GetLobby = function(name)
{
    
    return this.lobbyList.find(CheckLobby, [name]);
};

var CheckLobby = function(lobby)
{
    return lobby.name == this[0];
};

LobbyManager.prototype.RemoveLobby = function(lobby)
{
    this.lobbyList.splice(this.lobbyList.indexOf(lobby), 1);
}

exports.LobbyManager = new LobbyManager();