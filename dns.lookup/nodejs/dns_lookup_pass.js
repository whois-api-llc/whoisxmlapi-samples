var http = require('https');
var querystring = require('querystring');

var domain = 'example.com';
var apiKey = 'Your dns lookup api key';
var checkType = '_all';

var url = 'https://www.whoisxmlapi.com/whoisserver/DNSService';

var params = {
    type: checkType,
    domainName: domain,
    apiKey: apiKey
};

url = url + '?' + querystring.stringify(params);

http.get(url, function(response) {
    var str = '';
    response.on('data', function(chunk) {
        str += chunk;
    });
    response.on('end', function() {
        console.log(str);
    });
}).end();
