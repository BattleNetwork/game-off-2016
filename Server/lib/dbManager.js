var config = require('../config/config.js');
var DatabaseManager = function () {
    this.mongoose = require('mongoose');
    this.mongoose.Promise = require('bluebird');
    this.mongoose.connect(config.dbserver);
    var Schema = this.mongoose.Schema;
    var instance = this;
    this.db = this.mongoose.connection;
    this.db.on('error', console.error.bind(console, 'connection error:'));
    this.db.once('open', function() {
        var playerSchema = new Schema({
            pseudo        : String,
            password      : String,
            registerdate  : Date
        });

        instance.playerModel = instance.db.model('Player', playerSchema);
    });
};

DatabaseManager.prototype.GetPlayerAuthentication = function(pseudo, callback) {
    var result = new Object();
    this.playerModel.findOne({ 'pseudo': pseudo }, 'pseudo password', callback);
}

DatabaseManager.prototype.GetPlayerProfil = function(pseudo, callback) {
    var result = new Object();
    this.playerModel.findOne({ 'pseudo': pseudo }, 'pseudo registerdate', callback);
}

exports.DBManager = new DatabaseManager();