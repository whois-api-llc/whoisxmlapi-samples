var http = require('http');

var domain = 'example.com';
var password = 'your_whois_api_password';
var username = 'your_whois_api_username';

var url = 'http://www.whoisxmlapi.com/whoisserver/DNSService?type=_all'
        + '&domainName=' + domain + '&username=' + username + '&password=' + password;

http.get(url, function(response) {
    var str = '';
    response.on('data', function(chunk) { str += chunk; });
    response.on('end', function() { console.log(str); });
}).end();