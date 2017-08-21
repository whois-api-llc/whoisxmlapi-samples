var https = require('https');

// Fill in your details
var username = 'your_whois_api_username';
var password = 'your_whois_api_password';

// Build the post string
var post_data ='{                       \
    "terms":[                           \
        {                               \
            "section":"Admin",          \
            "attribute":"name",         \
            "value":"Brett Branch",     \
            "exclude":"false"           \
        },                              \
        {                               \
            "section":"General",        \
            "attribute":"DomainName",   \
            "value":".com",             \
            "exclude":"false",          \
            "matchType":"EndsWith"      \
        }],                             \
    "recordsCounter":false,             \
    "outputFormat":"json",              \
    "username": "' + username + '",     \
    "password": "' + password +'",      \
    "rows":100                          \
}';

// set options fo request
var options = {
    hostname: 'www.whoisxmlapi.com',
    path: '/reverse-whois-api/search.php',
    port: 443,
    method: 'POST',
    headers: {
        'Content-Type': 'application/json',
        "Accept": "application/json",
        'Content-Length': post_data.length
    },
    json: true
};

// create request and get response
var req = https.request(options, function(res)  {
    var str = '';
    res.on('data', function(chunk) { str+=chunk; });
    res.on('end', function() { console.log(str); });

});

// handle errors
req.on('error', function(e) { console.error(e);});

// send request
req.write(post_data);
req.end();