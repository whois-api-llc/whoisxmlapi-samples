<?php

$apiUrl = 'https://www.whoisxmlapi.com/whoisserver/WhoisService';

$apiKey = 'Your Whois API key';
$domainName = 'whoisxmlapi.com';

$params = array(
    'apiKey' => $apiKey,
    'domainName' => $domainName,
    'outputFormat' => 'JSON'
);

$url = $apiUrl . '?' . http_build_query($params, '', '&');

print($url . PHP_EOL . PHP_EOL);

$response = file_get_contents($url);

$res = json_decode($response);

if (! $res)
    return;

if (! empty($res->ErrorMessage)) {
    print($res->ErrorMessage->msg . PHP_EOL);
    return;
}

$whoisRecord = $res->WhoisRecord;
if (! $whoisRecord)
    return;

echo 'Domain name: ' . print_r($whoisRecord->domainName, 1) . PHP_EOL;
echo 'Created date: ' .print_r($whoisRecord->createdDate, 1) . PHP_EOL;
echo 'Updated date: ' .print_r($whoisRecord->updatedDate, 1) . PHP_EOL;

if (! empty($whoisRecord->registrant))
    echo 'Registrant: ' . PHP_EOL
         . print_r($whoisRecord->registrant->rawText, 1) . PHP_EOL;