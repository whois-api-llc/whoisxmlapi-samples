<?php
$username = 'username';
$apiKey = 'api_key';
$secret = 'secret_key';
$url = 'https://whoisxmlapi.com/whoisserver/DNSService?';
$timestamp = null;
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
    printResponse($response);
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

function printResponse($response)
{
    echo $response;
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
    $requestString .= '&type=_all';
    $requestString .= '&digest=';
    $requestString .= $digest;
    $requestString .= '&domainName=';
    $requestString .= $domain;
    $requestString .= '&outputFormat=json';
    return $requestString;
}