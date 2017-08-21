var https = require('https');
var querystring = require('querystring');

var url = "https://www.whoisxmlapi.com/"
    +"whoisserver/WhoisService?";

var parameters = {
    cmd: 'GET_DN_AVAILABILITY',
    domainName: 'google.com',
    username: 'YOUR_USERNAME',
    password: 'YOUR_PASSWORD',
    outputFormat: 'json'
};

url = url + querystring.stringify(parameters);

https.get(url, function (res) {
    const statusCode = res.statusCode;

    if (statusCode !== 200) {
        console.log('Request failed: '
            + statusCode
        );
    }

    var rawData = '';

    res.on('data', function(chunk) {
        rawData += chunk;
    });
    res.on('end', function () {
        try {
            var parsedData = JSON.parse(rawData);
            if (parsedData.DomainInfo) {
                console.log(
                    'Domain name: '
                    + parsedData.DomainInfo.domainName
                );
                console.log(
                    'Domain Availability: '
                    + parsedData.DomainInfo.domainAvailability
                );
            } else {
                console.log(parsedData);
            }
        } catch (e) {
            console.log(e.message);
        }
    })
}).on('error', function(e) {
    console.log("Error: " + e.message);
});
