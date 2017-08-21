const https = require('https');
const queryString = require('querystring');
const crypto = require('crypto');

const username = 'username';
const apiKey = 'api_key';
const secretKey = 'secret_key';
const url = 'https://whoisxmlapi.com/whoisserver/EmailVerifyService?';
const emailAddress = 'support@whoisxmlapi.com';

getWhois(username, apiKey, secretKey, emailAddress);

function getWhois(username, apiKey, secretKey, emailAddress)
{
    timestamp = (new Date).getTime();
    digest = generateDigest(username, timestamp, apiKey, secretKey);
    var requestString = buildRequest(username, timestamp, digest, emailAddress);
    https.get(url + requestString, function (res) {
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
            console.log(rawData);
        })
    }).on('error', function(e) {
        console.log("Error: " + e.message);
    });

}

function generateDigest(username, timestamp, apiKey, secretKey) {
    var data = username + timestamp + apiKey;
    var hmac = crypto.createHmac('md5', secretKey);
    hmac.update(data);
    return hmac.digest('hex');
}

function buildRequest(username, timestamp, digest, emailAddress) {
    var data = {
        u: username,
        t: timestamp
    };

    var dataJson = JSON.stringify(data);
    var dataBase64 = Buffer.from(dataJson).toString('base64');

    var request = {
        requestObject: dataBase64,
        digest: digest,
        emailAddress: emailAddress,
        validateDNS:true,
        validateSMTP:true,
        checkCatchAll:true,
        checkFree:true,
        checkDisposable:true,
        outputFormat:'json'
    };

    return queryString.stringify(request);
}