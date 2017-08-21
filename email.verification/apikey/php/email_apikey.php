<?php

    $username = 'username';
    $apiKey = 'api_key';
    $secret = 'secret_key';

    $emailAddress = 'support@whoisxmlapi.com';

    $url = 'https://whoisxmlapi.com/whoisserver/EmailVerifyService?';
    $timestamp = null;
    $digest = null;

    generateParameters($timestamp, $digest, $username, $apiKey, $secret);

    $response = request($url, $username, $timestamp, $digest, $emailAddress);
    if (strpos($response, 'Request timeout') !== false) {
        generateParameters($timestamp, $digest, $username, $apiKey, $secret);
        $response = request($url, $username, $timestamp, $digest, $emailAddress);
    }
    echo $response;
    echo "\n----------------------------\n";

    function generateParameters(&$timestamp, &$digest, $username, $apiKey, $secret)
    {
        $timestamp = round(microtime(true) * 1000);
        $digest = generateDigest($username, $timestamp, $apiKey, $secret);
    }

    function request($url, $username, $timestamp, $digest, $emailAddress)
    {
        $requestString = buildRequest($username, $timestamp, $digest, $emailAddress);
        return file_get_contents($url . $requestString);
    }

    function generateDigest($username, $timestamp, $apiKey, $secretKey)
    {
        $digest = $username . $timestamp . $apiKey;
        $hash = hash_hmac('md5', $digest, $secretKey);
        return urlencode($hash);
    }

    function buildRequest($username, $timestamp, $digest, $emailAddress)
    {
        $requestString = 'requestObject=';
        $request = array(
            'u' => $username,
            't' => $timestamp
        );
        $requestJson = json_encode($request);
        $requestBase64 = base64_encode($requestJson);
        $requestString .= urlencode($requestBase64);
        $requestString .= "&emailAddress=";
        $requestString .= $emailAddress;
        $requestString .= '&digest=';
        $requestString .= $digest;
        $requestString .= "&validateDNS=true&validateSMTP=true";
        $requestString .= "&checkCatchAll=true&checkFree=true&checkDisposable=true";
        $requestString .= '&outputFormat=json';
        return $requestString;
    }
?>