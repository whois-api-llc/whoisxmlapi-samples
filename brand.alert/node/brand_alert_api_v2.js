var https = require('https');

// Fill in your details
var api_key = 'Your brand alert api key';

// Build the post string
var post_data = {
    includeSearchTerms: [
        'google',
        'blog'
    ],
    excludeSearchTerms: [
        'analytics'
    ],
    responseFormat: 'json',
    apiKey: api_key,
    mode: 'purchase',
    sinceDate: '2018-06-15'
};

// Set request options
var options = {
    hostname: 'brand-alert.whoisxmlapi.com',
    path: '/api/v2',
    port: 443,
    method: 'POST',
    headers: {
        'Content-Type': 'application/json',
        'Accept': 'application/json',
        'Content-Length': JSON.stringify(post_data).length
    },
    json: true
};

// Create request and get response
var req = https.request(options, function(res)  {
    var str = '';
    res.on('data', function(chunk) {
        str += chunk;
    });
    res.on('end', function() {
        console.log(JSON.parse(str));
    });

});

// Handle errors
req.on('error', function(e) {
    console.error(e);
});

// Send request
req.write(JSON.stringify(post_data));
req.end();
