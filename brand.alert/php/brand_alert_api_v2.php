<?php

$apiKey = 'Your brand alert api key';

$header = "Content-Type: application/json\r\nAccept: application/json\r\n";
$url = 'https://brand-alert.whoisxmlapi.com/api/v2';

$options = array(
    'http' => array(
        'method' => 'POST',
        'header' => $header,
        'content' => json_encode(
            array(
                'includeSearchTerms' => array(
                    'whois',
                    'api'
                ),
                'excludeSearchTerms' => array(
                    '.tk'
                ),
                'mode' => 'purchase',
                'responseFormat' => 'json',
                'rows' => 10,
                'apiKey' => $apiKey,
                'sinceDate' => '2018-06-15'
            )
        )
    )
);

print(file_get_contents($url,false,stream_context_create($options)) .PHP_EOL);
