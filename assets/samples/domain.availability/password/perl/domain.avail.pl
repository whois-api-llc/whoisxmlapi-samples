#!/usr/bin/perl

use LWP::Simple;

my $domain = 'example.com';
my $password = 'your whois api password';
my $username = 'your whois api username';

my $url = 'http://www.whoisxmlapi.com/whoisserver/WhoisService?'
        . "domainName=$domain&userName=$userName&password=$password"
        . "&cmd=GET_DN_AVAILABILITY";

print get($url);