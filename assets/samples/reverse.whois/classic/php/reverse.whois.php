<?php

$term = 'wikimedia';
$password = 'your whois api password';
$username = 'your whois api username';

$url ="https://www.whoisxmlapi.com/reverse-whois-api/search.php?term1={$term}"
     ."&username={$username}&password={$password}&mode=preview";

print(file_get_contents($url));