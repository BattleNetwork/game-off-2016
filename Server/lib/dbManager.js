var DatabaseManager = function () {};
var config = require('../config/config.js');

DatabaseManager.prototype.mongoose = require('mongoose');
DatabaseManager.prototype.mongoose.Promise = require('bluebird');
DatabaseManager.prototype.mongoose.connect(config.dbserver);

DatabaseManager.prototype.db = DatabaseManager.mongoose.connection;
DatabaseManager.prototype.db.on('error', console.error.bind(console, 'connection error:'));
DatabaseManager.prototype.db.once('open', function() {
    var Schema = mongoose.Schema;

    var playerSchema = new Schema({
        pseudo        : String,
        password      : String,
        registerdate  : Date
    });

    var playerModel = db.model('Player', playerSchema);
});

DatabaseManager.prototype.GetPlayerAuthentication = function(pseudo) {
    var result = new object();
    playerModel.findOne({ 'pseudo': pseudo }, 'pseudo password', function(err, player) {
        result.err = err;
        result.player = player;
        return result;
    });
}

DatabaseManager.prototype.GetPlayerProfil = function(pseudo) {
    var result = new object();
    playerModel.findOne({ 'pseudo': pseudo }, 'pseudo registerdate', function(err, player) {
        result.err = err;
        result.player = player;
        return result;
    });
}

exports.DBManager = new DatabaseManager();