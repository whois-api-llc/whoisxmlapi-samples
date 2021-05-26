#!/usr/bin/perl

use HTTP::Request::Common qw{ POST }; # From CPAN
use JSON qw( decode_json );           # From CPAN
use LWP::UserAgent;                   # From CPAN

use strict;
use warnings;

########################
# Fill in your details #
########################
my $api_key = 'Your registrant alert api key';

my $url = 'https://registrant-alert.whoisxmlapi.com/api/v2';

my $search_params_advanced = '{
    "advancedSearchTerms": [
        {
            "field": "RegistrantContact.Name",
            "term": "Test"
        }
    ],
    "apiKey": "' . $api_key . '",
    "mode": "purchase",
    "sinceDate": "2018-07-12"
}';

my $search_params_basic = '{
    "basicSearchTerms": {
        "include": [
            "whois",
            "api"
        ],
        "exclude": [
            ".ga"
        ]
    },
    "apiKey": "' . $api_key . '",
    "mode": "purchase",
    "sinceDate": "2018-06-15"
}';

#######################
# Basic search        #
#######################

my $response = JSON->new->decode(registrantAlertApiSearch(0));
print "Basic\n---\n";
print JSON->new->pretty->encode($response);

#######################
# Advanced search     #
#######################

$response = JSON->new->decode(registrantAlertApiSearch(1));
print "Advanced\n---\n";
print JSON->new->pretty->encode($response);

#######################
# Getting the data    #
#######################

sub registrantAlertApiSearch {
    my ($isAdvanced) = @_;

    my $ua = LWP::UserAgent->new(ssl_opts => { verify_hostname => 0 });
    my $req = HTTP::Request->new('POST', $url);

    my $search_params =
        $isAdvanced ? $search_params_advanced : $search_params_basic;

    $req->header('Content-Type' => 'application/json');
    $req->header('Accept', 'application/json');
    $req->content($search_params);

    my $result = $ua->request($req);

    return $result->content;
}
