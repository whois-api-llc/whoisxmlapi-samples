<?php

    $emailAddress = 'support@whoisxmlapi.com';
    $password = 'your_whois_api_password';
    $username = 'your_whois_api_username';

    $url = "http://www.whoisxmlapi.com/whoisserver/EmailVerifyService"
         . "?emailAddress={$emailAddress}&validateDNS=true&validateSMTP=true"
         . "&checkCatchAll=true&checkFree=true&checkDisposable=true"
         . "&username={$username}&password={$password}";

    print(file_get_contents($url));

?>