<?php

$user = 'your whois api username';
$password = 'your whois api password';

$header = "Content-Type: application/json\r\nAccept: application/json\r\n";

$options = array('http' => array(
    'method' => 'POST',
    'header' => $header
));

$url = 'https://www.whoisxmlapi.com/reverse-whois-api/search.php';

$options['http']['content'] = json_encode(array(
    'terms'=>array(
        array(
            'section' => 'Admin',
            'attribute' => 'name',
            'value' => 'Brett Branch',
            'exclude' => false
        ),
        array(
            'section' => 'General',
            'attribute' => 'DomainName',
            'value' => '.com',
            'exclude' => 'false',
            'matchType' => 'EndsWith'
        )
    ),
    'recordsCounter' => false,
    'username' => $user,
    'password' => $password,
    'outputFormat' => 'json',
    'rows' => '100'
));

print(file_get_contents($url, false, stream_context_create($options)));