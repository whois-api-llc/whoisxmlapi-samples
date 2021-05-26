<?php

$apiKey = 'Your registrant alert api key';

$termsAdvanced = array(
    'advancedSearchTerms' => array(
        array(
            'field' => 'RegistrantContact.Name',
            'term' => 'Test'
        )
    ),
    'mode' => 'purchase',
    'apiKey' => $apiKey,
    'sinceDate' => '2018-07-15'
);

$termsBasic = array(
    'basicSearchTerms' => array(
        'include' => array(
            'whois',
            'api'
        ),
        'exclude' => array(
            '.tk'
        )
    ),
    'mode' => 'purchase',
    'apiKey' => $apiKey,
    'sinceDate' => '2018-06-15'
);

function registrant_alert_api(array $data=array())
{
    $header ="Content-Type: application/json\r\nAccept: application/json\r\n";

    $url = 'https://registrant-alert.whoisxmlapi.com/api/v2';

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
print_r(json_decode(registrant_alert_api($termsBasic)));

print('Advanced:' . PHP_EOL);
print_r(json_decode(registrant_alert_api($termsAdvanced)));
