<?php

    $emailAddress = 'support@whoisxmlapi.com';
    $apiKey = 'your_email_verification_api_key';

    $url = "https://emailverification.whoisxmlapi.com/api/v1"
         . "?emailAddress={$emailAddress}&apiKey={$apiKey}";

    print(file_get_contents($url));

?>