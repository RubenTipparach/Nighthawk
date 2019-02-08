'use strict';

const express = require('express'),
    bodyParser = require('body-parser');

    const cors = require('cors');

const colors = require('colors');
const strFormat = require('string-format');

const logger = require('./logger.js');

// config stuff
const yaml = require('js-yaml');
const fs = require('fs');

var config = null;
try {
    config = yaml.safeLoad(fs.readFileSync('config.yaml', 'utf8'));
    logger.debug(config);
} catch (e) {
    logger.error(e);
}

const low = require('lowdb')
const FileSync = require('lowdb/adapters/FileSync')
const adapter = new FileSync('db.json')
const db = low(adapter)

const app = express();
app.use(bodyParser.json());
app.use(bodyParser.urlencoded({ extended: false }));
app.use(cors());

app.listen(config.network.port, () => {
    logger.info(`Map started on port ${config.network.port.toString().green.bold}`);
});

app.get('/', (req,res) => {
	res.send('Node API Server is online');
});

app.get('/get/ipAddresses', (req,res) => {
    res.send({data : db.get('ipAddressMap').value()[0]});
})

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

