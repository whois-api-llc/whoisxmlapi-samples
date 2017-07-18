var http = require('http');

var email = 'support@whoisxmlapi.com';
var password = 'your_whois_api_password';
var username = 'your_whois_api_username';

var url = 'http://www.whoisxmlapi.com/whoisserver/EmailVerifyService'
        + '?emailAddress=' + email
        + '&validateDNS=true&validateSMTP=true&checkCatchAll=true'
        + '&checkFree=true&checkDisposable=true'
        + '&username=' + username + '&password=' + password

http.get(url, function(response) {
    var str = '';
    response.on('data', function(chunk) { str += chunk; });
    response.on('end', function() { console.log(str); });
}).end();