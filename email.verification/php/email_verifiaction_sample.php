<?php

$apiKey = 'Your email verification api key';

$emailAddress = 'support@whoisxmlapi.com';

$apiUrl = 'https://emailverification.whoisxmlapi.com/api/v1';

$params = array(
    'emailAddress' => $emailAddress,
    'apiKey' => $apiKey
);

$url = $apiUrl . '?' . http_build_query($params, '', '&');

print($url . PHP_EOL . PHP_EOL);

$response = file_get_contents($url);

print_r(json_decode($response));
print(PHP_EOL);