<?php

$apiKey = 'Your domain availability api key';
$domain = 'google.com';

$url = 'https://domain-availability.whoisxmlapi.com/api/v1'
     . '?domainName=' . urlencode($domain)
     . '&apiKey=' . urlencode($apiKey)
     . '&outputFormat=JSON';

$contents = file_get_contents($url);

$res = json_decode($contents);

if ($res) {
    if (isset($res->ErrorMessage)) {
        echo $res->ErrorMessage->msg;
    }
    else {
        $domainInfo = $res->DomainInfo;
        if ($domainInfo) {
            echo 'Domain name: ' .print_r($domainInfo->domainName,1) .PHP_EOL;
            echo 'Domain Availability: '
                 . print_r($domainInfo->domainAvailability, 1) . PHP_EOL;
        }
    }
}
