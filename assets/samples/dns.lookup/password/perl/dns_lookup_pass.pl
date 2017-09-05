#!/usr/bin/perl

use LWP::Simple;                # From CPAN, also install LWP::Protocol::Https
use JSON qw( decode_json );     # From CPAN
use Data::Dumper;
use strict;
use warnings;

my $base_url = "https://www.whoisxmlapi.com/whoisserver/DNSService";
my $type = "_all";
my $domain_name = "google.com";
my $user_name = "Your whois api username";
my $password = "Your whois api password";

print "JSON\n---\n".getDnsData("json");

sub getDnsData {
    my $format = $_[0];
    my $url = "$base_url?type=$type&domainName=$domain_name" 
        . "&outputFormat=$format&userName=$user_name" 
        . "&password=$password";

    my $object = get($url);
    die "Could not get $base_url!" unless defined $object;
    return $object
}