#!/usr/bin/perl

use Data::Dumper;
use JSON qw( decode_json );       # From CPAN
use LWP::Protocol::https;         # From CPAN
use LWP::Simple;                  # From CPAN
use URI::Escape qw( uri_escape ); # From CPAN

use strict;
use warnings;

my $base_url = 'https://domain-availability.whoisxmlapi.com/api/v1';
my $domain_name = 'google.com';
my $format = 'json';

my $api_key = 'Your domain availability api key';

my $url = $base_url
        . '?domainName=' . uri_escape($domain_name)
        . '&outputFormat=' . uri_escape($format)
        . '&apiKey=' . uri_escape($api_key);

print "Get data by URL: $url\n";

# 'get' is exported by LWP::Simple;
my $json = get($url);
die "Could not get $base_url!" unless defined $json;

# Decode the entire JSON
my $decoded_json = decode_json($json);

# Dump all data if need
#print Dumper $decoded_json;

# Print fetched attribute
my $domainNameJson = $decoded_json->{'DomainInfo'}->{'domainName'};
my $availJson = $decoded_json->{'DomainInfo'}->{'domainAvailability'};

my $err = 'Empty. Something went wrong.';

print 'Domain Name: ', ($domainNameJson)? $domainNameJson: $err, "\n";
print 'Domain Availability: ', ($availJson)? $availJson: $err, "\n";
