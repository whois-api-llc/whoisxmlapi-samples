#!/usr/bin/perl

use LWP::Simple;                # From CPAN
use JSON qw( decode_json );     # From CPAN
use Data::Dumper;               # Perl core module
use strict;                     # Good practice
use warnings;                   # Good practice

my $base_url = "https://www.whoisxmlapi.com/whoisserver/WhoisService";
my $cmd = "GET_DN_AVAILABILITY";
my $domain_name = "google.com";
my $format = "json";
my $user_name = "";
my $password = "";

# 'get' is exported by LWP::Simple;
my $url = "$base_url?cmd=$cmd&domainName=$domain_name&outputFormat=$format";
if ($user_name ne "") {
    $url = "$url&userName=$user_name&password=$password";
}

print "Get data by URL: $url\n";
my $json = get($url);
die "Could not get $base_url!" unless defined $json;

# Decode the entire JSON
my $decoded_json = decode_json($json);

# Dump all data if need
#print Dumper $decoded_json;

# Print fetched attribute
my $domainJsonName = $decoded_json->{'DomainInfo'}->{'domainName'};
my $domainAvJson = $decoded_json->{'DomainInfo'}->{'domainAvailability'};

print "Domain Name: " , ($domainJsonName)? $domainJsonName: "Empty. Something went wrong.", "\n";
print "Domain Availability: ", ($domainAvJson)? print$domainAvJson: "Empty. Something went wrong.", "\n";
