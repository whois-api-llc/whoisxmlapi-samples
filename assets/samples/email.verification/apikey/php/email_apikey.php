<?php

$email = 'support@whoisxmlapi.com';
$key = 'Your whois api public key';
$secret = 'your whois api secret key';
$username = 'your whois api username';

$time = round(microtime(true) * 1000);
$reqObj = urlencode(base64_encode("{\"t\":{$time},\"u\":\"{$username}\"}"));
$digest = urlencode(hash_hmac('md5', $username . $time . $key, $secret));

$url = 'http://www.whoisxmlapi.com/whoisserver/EmailVerifyService?'
     . "requestObject={$reqObj}&digest={$digest}&emailAddress={$email}"
     . '&checkCatchAll=1';

print(file_get_contents($url));