var config = require('../Config/config.js');
var DatabaseManager = function () {
    this.mongoose = require('mongoose');
    this.mongoose.Promise = require('bluebird');
    this.mongoose.connect(config.dbserver);

    //closures
    var Schema = this.mongoose.Schema;
    var instance = this;      

    this.db = this.mongoose.connection;
    this.db.on('error', console.error.bind(console, 'connection error:'));
    this.db.once('open', function() {
        var playerSchema = new Schema({
            pseudo        : {
                type: String,
                required: true,
                unique: true 
            },
            password      : {
                type: String,
                required: true
            },
            registerdate  : Date
        });

        instance.playerModel = instance.db.model('Player', playerSchema);
    });
};

DatabaseManager.prototype.GetPlayerAuthentication = function(pseudo, Callback) {
    var result = new Object();
    this.playerModel.findOne({ 'pseudo': pseudo }, 'pseudo password', Callback);
}

DatabaseManager.prototype.GetPlayerProfil = function(pseudo, Callback) {
    var result = new Object();
    this.playerModel.findOne({ 'pseudo': pseudo }, 'pseudo registerdate', Callback);
}

DatabaseManager.prototype.CreatePlayerProfile = function(pseudo, pass, Callback)
{
    var newPlayer = new this.playerModel({
            pseudo        : pseudo,
            password      : pass,
            registerdate  : Date.now()
        });

    newPlayer.save(Callback);
}

exports.DBManager = new DatabaseManager();