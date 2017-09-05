<?php
    $emailAddress = 'support@whoisxmlapi.com';
    $password = 'your whois api password';
    $username = 'your whois api username';

    $url = "http://www.whoisxmlapi.com/whoisserver/EmailVerifyService"
         . "?emailAddress={$emailAddress}&validateDNS=true&validateSMTP=true"
         . "&checkCatchAll=true&checkFree=true&checkDisposable=true"
         . "&username={$username}&password={$password}";

    print(file_get_contents($url));