<?php

$domain = 'example.com';
$apiKey = 'Your dns lookup api key';
$type = 'A,SOA,TXT';

$url
   = 'https://www.whoisxmlapi.com/whoisserver/DNSService'
   . '?domainName=' . urlencode($domain)
   . '&apiKey=' . urlencode($apiKey)
   . '&type=' . urlencode($type);

print(file_get_contents($url) . PHP_EOL);
