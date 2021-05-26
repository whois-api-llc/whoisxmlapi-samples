var https = require('https');
var querystring = require('querystring');

var url = 'https://www.whoisxmlapi.com/whoisserver/WhoisService';

var parameters = {
    domainName: 'google.com',
    apiKey: 'Your Whois API key',
    outputFormat: 'json'
};

url = url + '?' + querystring.stringify(parameters);

https.get(url, function (res) {
    const statusCode = res.statusCode;

    if (statusCode !== 200) {
        console.log('Request failed: ' + statusCode);
    }

    var rawData = '';

    res.on('data', function(chunk) {
        rawData += chunk;
    });

    res.on('end', function () {
        try {
            var parsedData = JSON.parse(rawData);

            if (parsedData.WhoisRecord) {
                console.log(
                    'Domain name: ' + parsedData.WhoisRecord.domainName);

                console.log(
                    'Contact email: ' + parsedData.WhoisRecord.contactEmail);
            } else {
                console.log(parsedData);
            }
        }
        catch (e) {
            console.log(e.message);
        }
    });
}).on('error', function(e) {
    console.log('Error: ' + e.message);
});