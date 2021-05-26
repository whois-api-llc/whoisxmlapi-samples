var https = require('https');

// Fill in your details
var api_key = 'Your reverse whois api key';

// Build the post string

var post_data_advanced = {
    advancedSearchTerms: [
        {
            field: 'RegistrantContact.Name',
            term: 'Test'
        }
    ],
    mode: 'purchase',
    sinceDate: '2018-07-12'
};

var post_data_basic = {
    basicSearchTerms: {
        include: [
            'test',
            'US'
        ],
        exclude: [
            'Europe',
            'EU'
        ],
    },
    mode: 'purchase',
    sinceDate: '2018-07-12'
};

function api_call(data, callback)
{

    var body = '';

    var opts = {
        hostname: 'reverse-whois-api.whoisxmlapi.com',
        path: '/api/v2',
        method:'POST',
        headers: {
            'Content-Type': 'application/json',
            'Accept': 'application/json',
            'Content-Length': JSON.stringify(data).length,
            'X-Authentication-Token': api_key,
        }
    };

    var req = https.request(opts, function(response) {
        response.on('data', function(chunk) {
            body += chunk;
        });
        response.on('end', function() {
            callback(JSON.parse(body));
        });
    });

    req.on('error', function(e) {
        console.error(e);
        process.exit(1);
    });

    req.write(JSON.stringify(data));
    req.end();
}

// Send requests and log responses

api_call(post_data_basic, function(body) {
    console.log('Basic:');
    console.log(body);

    api_call(post_data_advanced, function(body) {
        console.log('Advanced:');
        console.log(body);
    });
});