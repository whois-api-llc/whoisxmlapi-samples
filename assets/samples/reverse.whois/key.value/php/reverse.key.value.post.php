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
            'section' => 'Registrant',
            'attribute' => 'Email',
            'value' => 'a@b.com',
            'exclude' => false
        )
    ),
    'recordsCounter' => false,
    'username' => $user,
    'password' => $password
));

print(file_get_contents($url, false, stream_context_create($options)));