<?php

$domain = 'example.com';
$password = 'your whois api password';
$username = 'your whois api username';

$url = 'http://www.whoisxmlapi.com/whoisserver/WhoisService?'
     . "domainName={$domain}&username={$username}&password={$password}";

print(file_get_contents($url));