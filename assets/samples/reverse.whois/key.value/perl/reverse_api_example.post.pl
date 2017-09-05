#!/usr/bin/perl

use strict;
use warnings;

use LWP::UserAgent;             # From CPAN

my $user_name = "Your whois api username";
my $password = "Your whois api password";
my $url = 'https://www.whoisxmlapi.com/reverse-whois-api/search.php';
my $content = '{"terms":[{"section":"Admin", "attribute":"organization",
    "value":"Wikimedia", "matchType":"anywhere", "exclude":"false"},
    {"section": "technical", "attribute": "city", "value": "San Francisco",
    "matchType":"anywhere", "exclude":"false"}], "recordsCounter":false,
    "outputFormat":"json", "username":"'.$user_name.'", 
    "password":"'.$password.'", "rows":100
}';

my $ua = LWP::UserAgent->new(ssl_opts => { verify_hostname => 0 });
print "\n" . $ua->post($url, Content => $content)->decoded_content . "\n";