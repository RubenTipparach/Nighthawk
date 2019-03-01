

'use strict';
const low = require('lowdb')
const FileSync = require('lowdb/adapters/FileSync')

const adapter = new FileSync('db.json')
const db = low(adapter)
var gen = require('random-seed');
var logger = require('./logger.js');

// Set some defaults (required if your JSON file is empty)
db.defaults({ data: []})
  .write();

// ip parameters = [depth1 count, depth2 count, depth 3 count]
const ipCount = 100;
const seed = 42;
const rand = gen.create(seed);

const childSpawn = [.4,.1];

const deviceType = ["Broadband Router", "General Purpose"];
const routerOs = ["Linksys Default", "Cisco Default"];
const osTypes = [ "Ubuntu 18.04 LTS", "Raspbian 8.9", "MacOSX - Poodle", "Windows 90000 SP4.5"];

// IP example
const ipFound = [
    {
        id: 1,
        octets: [192, 168, 0, 1],
        macAddress: [255,255,255,255,255], // using octets to represent these
       
        status: 1, //1 means up, 0 down, -1 unknown
        latency: 115, //in ms
        deviceType: "Server Machine",
        os: "Windows 9000 SP4.5",

        connections: [
            { id:2 },
            { id:3 }
            ],
        ports: [20,11,80,225,3001]
    }
];
const connectionsRange = [1, 4];

// concetrate all the work on ports
const portsRange =[5, 10];

// Based off of this article, these are ports that are most likely hacked by systems.
// https://www.dummies.com/programming/networking/commonly-hacked-ports/
const portsDataBase = [
{
    port: 21,
    protocol: "TCP",
    service: "FTP",
    version:"1.0",
    description: "File Transfer Protocol"
},
{
    port: 22,
    protocol: "TCP",
    service: "SSH",
    version: "1.0",
    description: "Secure Shell"
},
{
    port: 23,
    protocol: "TCP",
    service: "Telnet",
    version: "1.0",
    description: ""
},
{
    port: 25,
    protocol: "TCP",
    service: "SMTP",
    version: "1.0",
    description: "Simple Mail Transfer Protocol"
},
{
    port: 53,
    protocol: "TCP/UDP",
    service: "DNS",
    version: "1.0",
    description: "Domain Name System"
},
{
    port: 80,
    protocol: "TCP",
    service: "HTTP",
    version: "1.0",
    description: "Hyper Text Transfer Protocol"
},
{
    port: 443,
    protocol: "TCP",
    service: "HTTPS",
    version: "1.0",
    description: "HTTP over SSL"
},
{
    port: 110,
    protocol: "TCP",
    service: "POP3",
    version: "3.0",
    description: "Post Office Protocol version 3"
},
]

// Windows exclusive servers.
const windowsExclusivePorts = [
    {
        port: 135,
        protocol: "TCP/UDP",
        service: "RPC",
        version: "8.0",
        description: "Windows RPC"
    },
    {
        port: 137,
        protocol: "TCP/UDP",
        service: "NetBIOS",
        version: "1.0",
        description: "Windows NetBIOS over TCP/IP"
    },
    {
        port: 138,
        protocol: "TCP/UDP",
        service: "NetBIOS",
        version: "1.0",
        description: "Windows NetBIOS over TCP/IP"
    },
    {
        port: 139,
        protocol: "TCP/UDP",
        service: "NetBIOS",
        version: "1.0",
        description: "Windows NetBIOS over TCP/IP"
    },
    {
        port: 1433,
        protocol: "TCP",
        service: "MSSQL",
        version: "11.0",
        description: "Microsoft SQL Server 2012"
    },
    {
        port: 1434,
        protocol: "UDP",
        service: "MSSQL",
        version: "11.0",
        description: "Microsoft SQL Server 2012"
    },
];

generateIPAddresses();



function generateIPAddresses(){

    let hosts = [];

    for (let i = 0; i < ipCount; i++)
    {
        // let concatIp = `${ipAddOct1_Lvl1}.${ipAddOct2_Lvl1}.${ipAddOct3}.${ipAddOct4}`;
        let host = {id: i, octets: [rand(256), rand(256), rand(256), rand(256)]};
        host.macAddress = [rand(256), rand(256), rand(256), rand(256), rand(256), rand(256)];

        host.status = 1;
        host.latency = rand.intBetween(100, 500);
        host.deviceType = deviceType[rand(deviceType.length)];

        if (host.deviceType  == "Broadband Router") {
            host.os = routerOs[rand(routerOs.length)];
        }
        else {
            host.os = osTypes[rand(osTypes.length)];
        }


        // host.child = [];
        let numOfconnections = rand.intBetween(connectionsRange[0], connectionsRange[1]);
        let numOfports = rand.intBetween(portsRange[0], portsRange[1]);
        host.connections = [];

        // some random ports, some exploitable ports.
        host.ports = [];

        // creates an undirected graph of stuff.
        for (let j = 0; j < numOfconnections; j++)
        {
            host.connections.push(rand(ipCount));
        }

        for (let k = 0; k < numOfports; k++)
        {
            host.ports.push(rand.intBetween(1, 5000));
        }

        hosts.push(host);
    }



    db.set('data', hosts)
       .write();
       
    logger.debug(hosts);
}