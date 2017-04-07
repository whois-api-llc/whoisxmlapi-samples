# Making a query to Whois API web service

Examples of using [www.whoisxmlapi.com](https://www.whoisxmlapi.com/) Hosted Whois Web Service RESTful API
implemented in multiple languages and could be divided into two types:


## Password authentication

```php
<?php

// Fill in your details
$username = "YOUR_USERNAME";
$password = "YOUR_PASSWORD";
$domain = "google.com";
$format = "JSON";

$url = 'http://www.whoisxmlapi.com/whoisserver/WhoisService?' 
    . 'domainName=' . $domain 
    . '&username=' . $username 
    . '&password='. $password 
    . '&outputFormat='. $format;


$result = json_decode(file_get_contents($url), true);
print_r($result);
```

You may browse full password authentication examples in multiple languages 
[here](https://github.com/whois-api-llc/whoisxmlapi-samples/tree/master/password)


## API key authentication

```php
<?php
$username = 'username';
$apiKey = 'api_key';
$secret = 'secret_key';
$url = 'https://whoisxmlapi.com/whoisserver/WhoisService?';
$timestamp = null
$domains = array(
    'google.com',
    'example.com',
    'whoisxmlapi.com',
    'twitter.com',
);
$digest = null;

generateParameters($timestamp, $digest, $username, $apiKey, $secret);

foreach ($domains as $domain) {
    $response = request($url, $username, $timestamp, $digest, $domain);
    if (strpos($response, 'Request timeout') !== false) {
        generateParameters($timestamp, $digest, $username, $apiKey, $secret);
        $response = request($url, $username, $timestamp, $digest, $domain);
    }
    print_r(json_decode($response, true));
    echo '----------------------------' . "\n";
}

function generateParameters(
    &$timestamp, &$digest, $username, $apiKey, $secret
)
{
    $timestamp = round(microtime(true) * 1000);
    $digest = generateDigest($username, $timestamp, $apiKey, $secret);
}

function request($url, $username, $timestamp, $digest, $domain)
{
    $requestString = buildRequest($username, $timestamp, $digest, $domain);
    return file_get_contents($url . $requestString);
}

function generateDigest($username, $timestamp, $apiKey, $secretKey)
{
    $digest = $username . $timestamp . $apiKey;
    $hash = hash_hmac('md5', $digest, $secretKey);
    return urlencode($hash);
}

function buildRequest($username, $timestamp, $digest, $domain)
{
    $requestString = 'requestObject=';
    $request = array(
        'u' => $username,
        't' => $timestamp
    );
    $requestJson = json_encode($request);
    $requestBase64 = base64_encode($requestJson);
    $requestString .= urlencode($requestBase64);
    $requestString .= '&digest=';
    $requestString .= $digest;
    $requestString .= '&domainName=';
    $requestString .= $domain;
    $requestString .= '&outputFormat=json';
    return $requestString;
}
```

You may browse full API key authentication examples in multiple languages 
[here](https://github.com/whois-api-llc/whoisxmlapi-samples/tree/master/apikey)
