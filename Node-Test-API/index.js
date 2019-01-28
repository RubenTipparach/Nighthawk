'use strict';

var express = require('express'),
    bodyParser = require('body-parser');

var cors = require('cors');

var colors = require('colors');
var strFormat = require('string-format');

var logger = require('./logger.js');

// config stuff
var yaml = require('js-yaml');
var fs = require('fs');
var fileUpload = require('express-fileupload');
const sqlite3 = require('sqlite3').verbose();

var config = null;
try {
    config = yaml.safeLoad(fs.readFileSync('config.yaml', 'utf8'));
    logger.debug(config);
} catch (e) {
    logger.error(e);
}

var app = express();

app.use(bodyParser.json());
app.use(bodyParser.urlencoded({ extended: false }));
app.use(cors());


app.listen(config.network.port, () => {
    logger.info(`Map started on port ${config.network.port.toString().green.bold}`);
});

let db = new sqlite3.Database('./db/default.db', (err) => {
    if (err) {
        console.error(err.message);
    }
    console.log('Connected to the default db database.');
});

db.close();

app.get('/', (req,res) => {
	res.send('Node API Server is online');
});



const ipFound = [
{
	octets: [192, 168, 0, 1],
	children: [
		{ octets:[192, 168, 1, 1] },
		{ octets:[192, 168, 1, 2] }
		]
},
{ octets: [192, 168, 0, 2] }
];

app.get('/iptargets', (req,res) => {

	
	res.send('');
});

