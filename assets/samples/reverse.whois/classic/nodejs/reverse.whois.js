var http = require('https');

var term = 'wikimedia';
var password = 'your whois api password';
var username = 'your whois api username';

var url = 'https://www.whoisxmlapi.com/reverse-whois-api/search.php?mode=preview'
        + '&term1=' + term + '&username=' + username + '&password=' + password;

http.get(url, function(response) {
    var str = '';
    response.on('data', function(chunk) { str += chunk; });
    response.on('end', function() { console.log(str); });
}).end();