var http = require('https');
var querystring = require('querystring');

var api_key = 'Your email verification api key';
var email = 'support@whoisxmlapi.com';

var url = 'https://emailverification.whoisxmlapi.com/api/v1';

var params = {
    apiKey: api_key,
    emailAddress: email,
    outputFormat: 'json'
};

url = url + '?' + querystring.stringify(params);

http.get(url, function(response) {
    var str = '';
    response.on('data', function(chunk) {
        str += chunk;
    });
    response.on('end', function() {
        console.log(JSON.parse(str));
    });
}).end();