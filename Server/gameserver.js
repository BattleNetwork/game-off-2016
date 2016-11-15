var playerManager = require('./Managers/playerManager.js').PlayerManager;
var dbManager = require('./Managers/dbManager.js').DBManager;
var lobbyManager = require('./Managers/lobbyManager.js').LobbyManager;

module.exports = function(io, app)
{
    app.post('/createplayer', function (req, res) {
        dbManager.CreatePlayerProfile(req.body.pseudo, req.body.pass, function(err, player)
        {
            if(err || !player) res.json({status:'error', content: null})
            else res.json({status:'ok', content: player});
        });
    })
    
    require('socketio-auth')(io, {
        authenticate: Authenticate,
        postAuthenticate: PostAuthenticate,
        disconnect: Disconnect,
        timeout: 1000
    });

    function Authenticate(socket, data, callback) {
        var pseudo = data.pseudo;
        var password = data.password;

        var result = dbManager.GetPlayerAuthentication(pseudo, function(err, player)
        {
            if (err || !player) return callback(new Error("User not found"));
            return callback(null, player.password == password);
        });
    }

    function PostAuthenticate(socket, data) {
        var pseudo = data.pseudo;
        
        var result = dbManager.GetPlayerProfil(pseudo, function(err, player)
        {
            socket.lobbyManager = lobbyManager;
            playerManager.AddPlayer(player, socket);
            
        });  
    }

    function Disconnect(socket) {
        playerManager.DisconnectPlayer(socket.id);
    }   
}