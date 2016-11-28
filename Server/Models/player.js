_ = require("underscore");

var Player = function(profil, socket) {
    this.profil = profil;
    this.socket = socket;
    this.socket.player = this;//because when in event callback this = socket and not the player...
    this.status = "init";
    this.room = "";//no room attributed in the first place
    this.isReady = false;
    this.isInGame = false;
};

Player.prototype.SetStatusAuthenticated = function()
{
    this.ClearSocketEvents();
    this.socket.on('listlobby',this.ListLobby);
    this.socket.on('createlobby',this.CreateLobby);
    this.socket.on('joinlobby',this.JoinLobby);

    this.status = "authenticated";
}

Player.prototype.QuitLobby = function()
{
    if(this.socket.lobby != null)
    {
        this.socket.lobby.RemovePlayer(this);
        if(this.socket.lobby.IsEmpty())
        {
            this.socket.lobbyManager.RemoveLobby(this.socket.lobby);
        }
    }
}

Player.prototype.SetStatusInLobby = function()
{
    this.ClearSocketEvents();
    this.socket.on('leavelobby',this.LeaveLobby);
    this.socket.on('readytoplay',this.ReadyToPlay);
    this.socket.on('unreadytoplay',this.UnReadyToPlay);
    this.socket.on('ingame',this.InGame);
    
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
            this.socket.removeListener('ingame',this.InGame);
        break;
        case "ingame":
            this.socket.removeListener('command',this.Command);
        break;
    }
}

Player.prototype.ListLobby = function(eventContent)
{
    var result = _.map(this.lobbyManager.lobbyList, function(val, index)
    {
        return _.mapObject(val, replacer)
    });
    //var result = JSON.parse(JSON.stringify(this.lobbyManager.lobbyList, ["name"]));
    this.emit('lobbylist', result);
}

function replacer(val, key) {
   if(key == "players") return val.length;
   else return val;
}

Player.prototype.CreateLobby = function(eventContent)
{
    this.lobby = this.lobbyManager.AddLobby(eventContent.lobbyName);
    this.lobby.AddPlayer(this.player);
    this.player.room = eventContent.lobbyName;
    this.player.SetStatusInLobby();
    this.join(eventContent.lobbyName);
    this.emit('lobbycreated', {'lobbyName':eventContent.lobbyName});
}
Player.prototype.JoinLobby = function(eventContent)
{
    if(this.lobbyManager.LobbyExist(eventContent.lobbyName))
    {
        this.lobby = this.lobbyManager.GetLobby(eventContent.lobbyName);
        if(!this.lobby.isFull)
        {
            this.lobby.AddPlayer(this.player);
            this.player.room = eventContent.lobbyName;
            this.join(eventContent.lobbyName);
            this.player.SetStatusInLobby();
            var opponent = {'pseudo':'', 'status':''}//empty
            if(this.lobby.isFull)//means somebody is already there
            {
                opponent.pseudo = this.lobby.FindOpponent(this.player).profil.pseudo;
                opponent.status = this.lobby.FindOpponent(this.player).isReady;
            }
            this.emit('lobbyjoined', {'lobbyName':eventContent.lobbyName, 'opponentInfo':opponent});
            
        }
        else {
            this.emit('lobbyfull', {});
        }
        
    }
    else {
        this.emit('lobbydontexist', {'lobbyName':eventContent.lobbyName});
    }
}
Player.prototype.LeaveLobby = function(eventContent)
{
    this.leave(this.room);
    this.player.QuitLobby();
    this.player.SetStatusAuthenticated();
    this.emit('lobbyleft', null);
}

//The following functions ara listened when the player is in a lobby so we delegate the work to it
Player.prototype.InGame = function(eventContent)
{
    this.lobby.SetPlayerInGame(this.player);
}
Player.prototype.ReadyToPlay = function(eventContent)
{
    this.lobby.SetPlayerReady(this.player);
}
Player.prototype.UnReadyToPlay = function(eventContent)
{
    this.lobby.SetPlayerUnready(this.player);
}

Player.prototype.Command = function(eventContent)
{
    this.lobby.ExecutePlayerCommand(this.player, eventContent);
}
module.exports = Player;