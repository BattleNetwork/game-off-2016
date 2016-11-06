
        var testplayer = new playerModel({
             pseudo        : 'Test',
            password      : 'Test',
            registerdate  : Date.now()
        });

        testplayer.save(function(err, thor) {
            if (err) return console.error("ERROR = " + err);
            console.dir(thor);
        });