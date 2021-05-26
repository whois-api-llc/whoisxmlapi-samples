<?php

$apiKey = 'Your reverse whois api key';

$termsAdvanced = array(
    'advancedSearchTerms' => array(
        array(
            'field' => 'RegistrantContact.Name',
            'term' => 'Test'
        )
    ),
    'mode' => 'purchase',
    'sinceDate' => '2018-07-15'
);

$termsBasic = array(
    'basicSearchTerms' => array(
        'include' => array(
            'test'
        ),
        'exclude' => array(
            'whois',
            'api'
        )
    ),
    'mode' => 'purchase',
    'sinceDate' => '2018-07-15'
);

function reverse_whois_api(array $data=array())
{
    global $apiKey;

    $header  = "Content-Type:application/json\r\nAccept:application/json\r\n";
    $header .= "X-Authentication-Token: {$apiKey}\r\n";

    $url = 'https://reverse-whois-api.whoisxmlapi.com/api/v2';

    $options = array(
        'http' => array(
            'method' => 'POST',
            'header' => $header,
            'content' => json_encode($data)
        )
    );

    return file_get_contents($url, false, stream_context_create($options));
}

print('Basic:' . PHP_EOL);
print_r(json_decode(reverse_whois_api($termsBasic)));

print('Advanced:' . PHP_EOL);
print_r(json_decode(reverse_whois_api($termsAdvanced)));