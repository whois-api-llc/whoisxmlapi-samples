<?php

$domain = 'example.com';
$password = 'your whois api password';
$username = 'your whois api username';

$url ="http://www.whoisxmlapi.com/whoisserver/DNSService?domainName={$domain}"
     ."&username={$username}&password={$password}&type=A,SOA,TXT";

print(file_get_contents($url));