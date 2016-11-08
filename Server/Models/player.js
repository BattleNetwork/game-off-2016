var Player = function(profil, socket) {
    this.profil = profil;
    this.socket = socket;
    this.socket.player = this;//because when in event callback this = socket and not the player...
    this.status = "init";
    this.room = "";//no room attributed in the first place
};

Player.prototype.lobbyManager = require('../Managers/lobbyManager.js').LobbyManager;

Player.prototype.SetStatusAuthenticated = function()
{
    this.ClearSocketEvents();
    this.socket.on('listlobby',this.ListLobby);
    this.socket.on('createlobby',this.CreateLobby);
    this.socket.on('joinlobby',this.JoinLobby);

    this.status = "authenticated";
}

Player.prototype.SetStatusInLobby = function()
{
    this.ClearSocketEvents();
    this.socket.on('leavelobby',this.LeaveLobby);
    this.socket.on('readytoplay',this.ReadyToPlay);
    this.socket.on('unreadytoplay',this.UnReadyToPlay);
    
    this.status = "inlobby";
}

Player.prototype.SetStatusInGame = function()
{
    this.ClearSocketEvents();
    this.socket.on('command',this.Command);

    this.status = "ingame";
}

Player.prototype.ClearSocketEvents = function()
{
    switch(this.status)
    {
        case "authenticated":
            this.socket.removeListener('listlobby',this.ListLobby);
            this.socket.removeListener('createlobby',this.CreateLobby);
            this.socket.removeListener('joinlobby',this.JoinLobby);
        break;
        case "inlobby":
            this.socket.removeListener('leavelobby',this.LeaveLobby);
            this.socket.removeListener('readytoplay',this.ReadyToPlay);
            this.socket.removeListener('unreadytoplay',this.UnReadyToPlay);
        break;
        case "ingame":
            this.socket.removeListener('command',this.Command);
        break;
    }
}

Player.prototype.ListLobby = function(eventContent)
{
    this.emit('lobbylist', JSON.stringify(this.player.lobbyManager.lobbyList));
}
Player.prototype.CreateLobby = function(eventContent)
{
    this.player.lobbyManager.AddLobby(eventContent.lobbyName);
    this.player.room = eventContent.lobbyName;
    this.player.SetStatusInLobby();
    this.join(eventContent.lobbyName);
    this.emit('lobbycreated', {'lobbyName':eventContent.lobbyName});
}
Player.prototype.JoinLobby = function(eventContent)
{
    if(this.player.lobbyManager.LobbyExist(eventContent.lobbyName))
    {
        this.join(eventContent.lobbyName);
        this.player.SetStatusInLobby();
        this.emit('lobbyjoined', {'lobbyName':eventContent.lobbyName});
    }
    else {
        this.emit('lobbydontexist', {'lobbyName':eventContent.lobbyName});
    }
}
Player.prototype.LeaveLobby = function(eventContent)
{
    this.leave(this.room);
    this.player.SetStatusAuthenticated();
    this.emit('lobbyleft', null);
}
Player.prototype.ReadyToPlay = function(eventContent)
{
    
}
Player.prototype.UnReadyToPlay = function(eventContent)
{
    
}

Player.prototype.Command = function(eventContent)
{
    
}
module.exports = Player;