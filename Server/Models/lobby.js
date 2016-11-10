var Lobby = function(name) {
    this.name = name;
    this.isCountdownStarted = false;
    this.players = new Array();
};

Lobby.prototype.AddPlayer = function(player)
{
    this.players.push(player);
}

Lobby.prototype.SetPlayerReady = function(player)
{
    var opponent = this.FindOpponent(player);
    if(opponent.isReady)
    {
        player.isReady = true;
        this.isCountdownStarted = true;
    }
    else {
        player.isReady = true;
    }
}

Lobby.prototype.SetPlayerUnready = function(player)
{
    if(this.isCountdownStarted)
    {
        //nope
    }
    else {
        player.isReady = false;
    }
}

Lobby.prototype.ExecutePlayerCommand = function(player, command)
{

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

module.exports = Lobby;