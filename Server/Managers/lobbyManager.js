var Lobby = require('../Models/lobby.js');

var LobbyManager = function () {};

LobbyManager.prototype.lobbyList = new Array();

LobbyManager.prototype.AddLobby = function(name) {
    var newLobby = new Lobby(name);
    this.lobbyList.push(newLobby);
    return newLobby;
};

LobbyManager.prototype.LobbyExist = function(name) {
   
    return this.lobbyList.find(CheckLobby) != undefined;
};

LobbyManager.prototype.GetLobby = function(name)
{
    return this.lobbyList.find(CheckLobby);
};

var CheckLobby = function(lobby)
{
    return lobby.name == name;
};

LobbyManager.prototype.RemoveLobby = function(lobby)
{
    this.lobbyList.splice(this.lobbyList.indexOf(lobby), 1);
}

exports.LobbyManager = new LobbyManager();