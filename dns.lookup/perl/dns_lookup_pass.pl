#!/usr/bin/perl

use LWP::Protocol::https;         # From CPAN
use LWP::Simple;                  # From CPAN
use URI::Escape qw( uri_escape ); # From CPAN

use strict;
use warnings;

my $base_url = 'https://www.whoisxmlapi.com/whoisserver/DNSService';
my $type = '_all';
my $domain_name = 'google.com';
my $api_key = 'Your dns lookup api key';

#######################
# Use a JSON resource #
#######################
print "JSON\n---\n" . getDnsData('json');

#######################
# Use an XML resource #
#######################
print "XML\n---\n" . getDnsData('xml');

#######################
# Getting DNS Data    #
#######################
sub getDnsData {
    my $format = $_[0];
    my $url = $base_url
            . '?type=' . uri_escape($type)
            . '&domainName=' . uri_escape($domain_name)
            . '&outputFormat=' . uri_escape($format)
            . '&apiKey=' . uri_escape($api_key);

    print "Get data by URL: $url\n";

    # 'get' is exported by LWP::Simple;
    my $object = get($url);

    die "Could not get $base_url!" unless defined $object;

    return $object
}
