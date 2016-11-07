var PlayerManager = function () {}

PlayerManager.prototype.playerList = new Array();

PlayerManager.prototype.AddPlayer = function(player, socket)
{

}

PlayerManager.prototype.DisconnectPlayer= function(socketid)
{
    
}
/*playerModel.findOne({ 'pseudo': pseudo }, 'pseudo registerdate', function(err, player) {
            socket.playerData = player;
            socket.on('listlobby', function(parameters){
                socket.emit('lobbylist', JSON.stringify(lobbyManager.lobbyList));
            });
            socket.on('createlobby', function(parameters){
                lobbyManager.AddLobby(parameters.lobbyName);
                socket.join(parameters.lobbyName);
                socket.emit('lobbycreated', {'lobbyName':parameters.lobbyName});
            });
            socket.on('joinlobby', function(parameters){
                if(lobbyManager.LobbyExist(parameters.lobbyName))
                {
                    socket.join(parameters.lobbyName);
                    socket.emit('lobbyjoined', {'lobbyName':parameters.lobbyName});
                }
                else {
                    socket.emit('lobbydontexist', {'lobbyName':parameters.lobbyName});
                }
            });

            socket.on('leavelobby', function(parameters){
                socket.leave(socket.room);
                socket.emit('lobbyleft');
            });
        });*/

exports.PlayerManager = new PlayerManager();