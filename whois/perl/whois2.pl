#!/usr/bin/perl

use LWP::Protocol::https;         # From CPAN
use LWP::Simple;                  # From CPAN
use JSON qw( decode_json );       # From CPAN
use URI::Escape qw( uri_escape ); # From CPAN

use strict;
use warnings;

my $base_url = 'https://www.whoisxmlapi.com/whoisserver/WhoisService';

my $api_key = 'Your Whois API key';

my $domain_name = 'google.com';
my $format = 'json';

# 'get' is exported by LWP::Simple;
my $url = $base_url
        . '?domainName=' . uri_escape($domain_name)
        . '&outputFormat=' . uri_escape($format)
        . '&apiKey=' . uri_escape($api_key);

print "Get data by URL: $url\n";

my $json = get($url);

die "Could not get $base_url!" unless defined $json;

# Decode the entire JSON
my $decoded_json = decode_json($json);

if (defined $decoded_json->{'ErrorMessage'}) {
    die 'Error: ', $decoded_json->{'ErrorMessage'}{'msg'}, "\n";
}

# Print fetched attributes
print 'Domain Name: ', $decoded_json->{'WhoisRecord'}->{'domainName'}, "\n";
print 'Contact Email: ',$decoded_json->{'WhoisRecord'}->{'contactEmail'},"\n";