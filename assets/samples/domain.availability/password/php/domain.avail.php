<?php

$domain = 'example.com';
$password = 'your whois api password';
$username = 'your whois api username';

$url = 'http://www.whoisxmlapi.com/whoisserver/WhoisService?'
     . "domainName={$domain}&username={$username}&password={$password}"
     . "&cmd=GET_DN_AVAILABILITY";

print(file_get_contents($url));