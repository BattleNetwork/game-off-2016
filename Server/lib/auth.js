var db = require('mongoose');
var config = require('./config/config.js');

module.exports = function(io)
{
    db.connect(config.dbserver);

    var Schema = db.Schema,
        ObjectId = Schema.ObjectId;
    
    var playerSchema = new Schema({
        pseudo        : String,
        password      : String,
        registerdate  : Date
    });

    var playerModel = db.model('Player', playerSchema);

    require('socketio-auth')(io, {
        authenticate: authenticate,
        postAuthenticate: postAuthenticate,
        disconnect: disconnect,
        timeout: 1000
    });

    function authenticate(socket, data, callback) {
        var pseudo = data.pseudo;
        var password = data.password;
        
        playerModel.findOne({ 'pseudo': pseudo }, 'name password', function(err, player) {
            if (err || !user) return callback(new Error("User not found"));
            return callback(null, player.password == password);
        });
    }

    function postAuthenticate(socket, data) {
        var pseudo = data.pseudo;
        
        playerModel.findOne({ 'pseudo': pseudo }, 'name registerdate', function(err, player) {
            socket.client.playerData = player;
        });
    }

    function disconnect(socket) {
        console.log(socket.id + ' disconnected');
        playerModel.update({pseudo:socket.playerData.pseudo}, {$set: JSON.stringify(socket.playerData)});
    }
}