var http = require('https');

const username = 'your whois api username';
const password = 'your whois api password';
const term1 = "cinema";
const exclude_term1 = 'movie';
const exclude_term2 = 'online';
const format = "JSON";

var url = 'https://www.whoisxmlapi.com/brand-alert-api/search.php?' +
    'term1=' + term1 +
    '&username=' + username +
    '&password=' + password +
    '&output_format=' + format +
    '&exclude_term1=' + exclude_term1 +
    '&exclude_term2=' + exclude_term2;

http.get(url, function(response) {
    var str = '';
    response.on('data', function(chunk) { str += chunk; });
    response.on('end', function() { console.log(str); });
}).end();