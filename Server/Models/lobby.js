var async = require('async');

var Lobby = function(name) {
    this.name = name;
    this.isGoingInGame = false;
    this.player.isInGame = false;
    this.players = new Array();
};

Lobby.prototype.AddPlayer = function(player)
{
    if(this.players.length > 0)
    {
        players[0].socket.emit('playerjoined', player)
    }
    this.players.push(player);
    
}

Lobby.prototype.SetPlayerReady = function(player)
{
    player.isReady = true;
    player.socket.emit('readyset', null);
    var opponent = this.FindOpponent(player);
    if(opponent != null)
    {
        if(opponent.isReady)
        {
            opponent.socket.emit('playerreadyup', JSON.stringify(player.name));
            this.isGoingInGame = true;
            this.Broadcast('goingame', null);
        }
        else 
        {
            opponent.socket.emit('playerreadyup', JSON.stringify(player.name));
        }
    }
}

Lobby.prototype.SetPlayerInGame = function(player)
{
    player.isInGame = true;
    var opponent = this.FindOpponent(player);
    if(opponent != null && opponent.isInGame)
    {
        broadcast('countdown', 5);
        player.SetStatusInGame();
        opponent.SetStatusInGame();
    }
    
}

Lobby.prototype.SetPlayerUnready = function(player)
{
    if(this.isGoingInGame)
    {
        player.socket.emit('cantunready', null);
    }
    else {
        player.isReady = false;
        player.socket.emit('unreadyset', null);
        var opponent = this.FindOpponent(player);
        if(opponent != null)
        {
            opponent.socket.emit('opponentunready', null);            
        }
    }
}

Lobby.prototype.ExecutePlayerCommand = function(player, command)
{
    player.socket.emit("commandresult", "{result:command not found}");
}

Lobby.prototype.FindOpponent = function(player)
{
    if(this.players[0] == player)
    {
        return this.players[1];
    }
    else 
    {
        return this.players[0];
    }
}

Lobby.prototype.Broadcast = function(eventName, eventArgs)
{
    async.each(this.players, function(player, callback)
    {
        if(eventArgs != null) eventArgs = JSON.stringify(eventArgs);
        player.socket.emit(eventName, eventArgs);
        callback();
    },
    function (err)
    {
        console.log("ERROR WHILE BROADCASTING");
    });
}
module.exports = Lobby;