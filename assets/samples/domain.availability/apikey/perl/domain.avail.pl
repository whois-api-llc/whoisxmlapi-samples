#!/usr/bin/perl

use Digest::HMAC_MD5 qw( hmac_md5_hex );
use LWP::Simple;
use MIME::Base64 qw( encode_base64 );
use Time::HiRes qw( time );
use URI::Escape;

my $domain = 'example.com';
my $key = 'your whois api key';
my $secret = 'your whois api secret key';
my $username = 'your whois api username';

my $time = int((time * 1000 + 0.5));
my $reqObj = uri_escape(encode_base64("{\"t\":$time,\"u\":\"$username\"}"));
my $digest = uri_escape(hmac_md5_hex($username . $time . $key, $secret));

my $url = 'http://www.whoisxmlapi.com/whoisserver/WhoisService?'
        . "requestObject=$reqObj&digest=$digest&domainName=$domain"
        . "&cmd=GET_DN_AVAILABILITY";

print get($url);