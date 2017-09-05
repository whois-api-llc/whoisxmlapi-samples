var fs = require('fs'), https = require('https');

var dom = ['google.com', 'youtube.com', 'facebook.com', 'whoisxmlapi.com'];
var srv = 'www.whoisxmlapi.com', url = '/BulkWhoisLookup/bulkServices/';
var pass = 'Your whois api password';
var username = 'Your whois api username', file = 'bulk.csv';

function api(path, data, callback) {
    var baseData = {password: pass, username: username, outputFormat: 'json'};
    var header = {'Content-Type': 'application/json'}, body = '';
    var opts = {host: srv, path: url + path, method:'POST', headers: header};
    https.request(opts, function(response) {
        response.on('data', function(chunk) { body += chunk; });
        response.on('end', function() { callback(body); });
    }).end(JSON.stringify(Object.assign({}, baseData, data)));
}
// This will save whois record info for all domains as 'bulk.csv' in curr. dir
api('bulkWhois', {domains:dom}, function(body) {
    var id = {requestId:JSON.parse(body).requestId, startIndex: dom.length+1};
    var timer = setInterval(function() {
        api('getRecords', Object.assign({},id,{maxRecords:0}),function(body) {
            if (JSON.parse(body).recordsLeft) { return; }
            clearInterval(timer);
            if (typeof JSON.parse(body).recordsLeft==='undefined') { return; }
            api('download', id, function(csv) { fs.writeFile(file,csv); });
        });
    }, 3000);
});