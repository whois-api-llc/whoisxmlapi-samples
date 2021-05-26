#!/usr/bin/perl

use HTTP::Request::Common qw{ POST }; # From CPAN
use JSON qw( decode_json );           # From CPAN
use LWP::UserAgent;                   # From CPAN

use strict;
use warnings;

########################
# Fill in your details #
########################
my $api_key = 'Your reverse whois api key';

my $url = 'https://reverse-whois-api.whoisxmlapi.com/api/v2';

my $search_params_advanced = '{
    "advancedSearchTerms": [
        {
            "field": "RegistrantContact.Name",
            "term": "Test"
        }
    ],
    "mode": "purchase",
    "sinceDate": "2018-07-12"
}';

my $search_params_basic = '{
    "basicSearchTerms": {
        "include": [
            "test"
        ],
        "exclude": [
            "whois",
            "api"
        ]
    },
    "mode": "purchase",
    "sinceDate": "2018-06-15"
}';

#######################
# Basic search        #
#######################

my $response = JSON->new->decode(reverseWhoisApiSearch(0));
print "Basic\n---\n";
print JSON->new->pretty->encode($response);

#######################
# Advanced search     #
#######################

$response = JSON->new->decode(reverseWhoisApiSearch(1));
print "Advanced\n---\n";
print JSON->new->pretty->encode($response);

#######################
# Getting the data    #
#######################

sub reverseWhoisApiSearch {
    my ($isAdvanced) = @_;

    my $ua = LWP::UserAgent->new(ssl_opts => { verify_hostname => 0 });
    my $req = HTTP::Request->new('POST', $url);

    my $search_params =
        $isAdvanced ? $search_params_advanced : $search_params_basic;

    $req->header('Content-Type' => 'application/json');
    $req->header('Accept', 'application/json');
    $req->header('X-Authentication-Token', $api_key);
    $req->content($search_params);

    my $result = $ua->request($req);

    return $result->content;
}