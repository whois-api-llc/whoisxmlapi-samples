<?php
$username = 'username';
$apiKey = 'api_key';
$secret = 'secret_key';
$reuseDigest = false;
$url = 'https://whoisxmlapi.com/whoisserver/WhoisService?';
$timestamp = round(microtime(true) * 1000);
$domains = array(
    'google.com',
    'example.com',
    'whoisxmlapi.com',
    'twitter.com',
);

$digest = generateDigest($username, $timestamp, $apiKey, $secret);
foreach ($domains as $domain) {
    if (!$reuseDigest) {
        $timestamp = round(microtime(true) * 1000);
        $digest = generateDigest($username, $timestamp, $apiKey, $secret);
    }
    $requestString = buildRequest($username, $timestamp, $digest, $domain);
    $response = file_get_contents($url . $requestString);
    if (strpos($response, 'Request timeout') !== false) {
        $timestamp = round(microtime(true) * 1000);
        $digest = generateDigest($username, $timestamp, $apiKey, $secret);
        $requestString = buildRequest($username, $timestamp, $digest, $domain);
        $response = file_get_contents($url . $requestString);
    }
    printResponse($response);
    echo '----------------------------' . "\n";
}

function printResponse($response)
{
    $responseArray = json_decode($response, true);
    if (!empty($responseArray['WhoisRecord']['createdDate'])) {
        echo 'Created Date: '
            . $responseArray['WhoisRecord']['createdDate']
            . "\n";
    }
    if (!empty($responseArray['WhoisRecord']['expiresDate'])) {
        echo 'Expires Date: '
            . $responseArray['WhoisRecord']['expiresDate']
            . "\n";
    }
    if (!empty($responseArray['WhoisRecord']['registrant']['rawText'])) {
        echo $responseArray['WhoisRecord']['registrant']['rawText'] . "\n";
    }
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
    $requestString .= '&domainName='
        . $domain
        . '&outputFormat=json';
    return $requestString;
}