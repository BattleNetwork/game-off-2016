var Lobby = function(name) {
    this.name = name;
};

var LobbyManager = function () {};

LobbyManager.prototype.lobbyList = new Array();

LobbyManager.prototype.AddLobby = function(name) {
    var newLobby = new Lobby(name);
    this.lobbyList.push(newLobby);
};

LobbyManager.prototype.LobbyExist = function(name) {
    var CheckLobby = function(lobby)
    {
        return lobby.name == name;
    }
    return this.lobbyList.find(CheckLobby);
};

exports.LobbyManager = new LobbyManager();