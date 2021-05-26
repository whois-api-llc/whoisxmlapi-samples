#!/usr/bin/perl

use HTTP::Request::Common qw{ POST }; # From CPAN
use JSON qw( decode_json );           # From CPAN
use LWP::UserAgent;                   # From CPAN

use strict;
use warnings;

########################
# Fill in your details #
########################
my $api_key = 'Your brand alert api key';

my $search_params = '{
    "includeSearchTerms": [
        "whois",
        "api"
    ],
    "excludeSearchTerms": [
        ".ga"
    ],
    "responseFormat": "json",
    "apiKey": "' . $api_key . '",
    "mode": "purchase",
    "sinceDate": "2018-06-15"
}';

my $url = 'https://brand-alert.whoisxmlapi.com/api/v2';

#######################
# Use a JSON resource #
#######################

my $responseJson = JSON->new->decode(brandAlertApiSearch());
print "JSON\n---\n";
print JSON->new->pretty->encode($responseJson);

#######################
# Getting Data        #
#######################

sub brandAlertApiSearch {
    my $ua = LWP::UserAgent->new(ssl_opts => { verify_hostname => 0 });
    my $req = HTTP::Request->new('POST', $url);

    $req->header('Content-Type' => 'application/json');
    $req->header('Accept', 'application/json');
    $req->content($search_params);

    my $response = $ua->request($req);

    return $response->content;
}
