

'use strict';
const low = require('lowdb')
const FileSync = require('lowdb/adapters/FileSync')

const adapter = new FileSync('db.json')
const db = low(adapter)
var gen = require('random-seed');
var logger = require('./logger.js');

// Set some defaults (required if your JSON file is empty)
db.defaults({ ipAddressMap: []})
  .write();

//// Add a post
// db.get('ipAddressMap')
//   .push({ id: 1, title: 'test'})
//   .write();

// // Set a user using Lodash shorthand syntax
// db.set('meta.test', 'test complete')
//   .write();
  
// // Increment count
// db.update('count', n => n + 1)
//     .write();

// db.get('ipAddressMap')
//   .find({ id: 1 })
//   .value();

// ip parameters = [depth1 count, depth2 count, depth 3 count]
const ipCount = [ 100, 20, 5];
const seed = 42;
const rand = gen.create(seed);

const childSpawn = [.4,.1];

// IP example
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

generateIPAddresses();

function generateIPAddresses(){

    let ipAddOct1_Lvl1 = rand(255);
    let ipAddOct2_Lvl1 = rand(255);

    let octetArray = [];

    for (let i = 0; i < ipCount[0]; i++)
    {
        let ipAddOct3 = rand(255);
        let ipAddOct4 = rand(255);

        //let concatIp = `${ipAddOct1_Lvl1}.${ipAddOct2_Lvl1}.${ipAddOct3}.${ipAddOct4}`;
        let ipObj = {octets: [ipAddOct1_Lvl1, ipAddOct2_Lvl1, ipAddOct3, ipAddOct4]};
        ipObj.child = [];

        let hasChild = rand.floatBetween(0, 1) > childSpawn[0] ? true : false;

        if(hasChild)
        {
            let ipAddOct1_Lvl2 = rand(255);
            let ipAddOct2_Lvl2 = rand(255);
            
            let randChildCount = rand(ipCount[1]) + 1

            for (let i = 0; i < randChildCount; i++)
            {
                ipAddOct3 = rand(255);
                ipAddOct4 = rand(255);
                ipObj.child.push( {octets: [ipAddOct1_Lvl2, ipAddOct2_Lvl2, ipAddOct3, ipAddOct4], child: []});
            }
        }

        octetArray.push(ipObj);
    }

    db.get('ipAddressMap')
       .push(octetArray)
       .write();
       
    logger.debug(octetArray);
}