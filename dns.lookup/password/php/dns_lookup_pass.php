<?php

$domain = 'example.com';
$password = 'your_whois_api_password';
$username = 'your_whois_api_username';

$url ="http://www.whoisxmlapi.com/whoisserver/DNSService?domainName={$domain}"
     ."&username={$username}&password={$password}&type=A,SOA,TXT";

print(file_get_contents($url));