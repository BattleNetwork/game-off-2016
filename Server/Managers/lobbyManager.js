var Lobby = require('../Models/lobby.js');

var LobbyManager = function () {};

LobbyManager.prototype.lobbyList = new Array();

LobbyManager.prototype.AddLobby = function(name) {
    var newLobby = new Lobby(name);
    this.lobbyList.push(newLobby);
    return newLobby;
};

LobbyManager.prototype.LobbyExist = function(name) {
    var CheckLobby = function(lobby)
    {
        return lobby.name == name;
    }
    return this.lobbyList.find(CheckLobby);
};

exports.LobbyManager = new LobbyManager();