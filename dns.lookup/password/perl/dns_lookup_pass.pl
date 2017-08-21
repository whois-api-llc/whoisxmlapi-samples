#!/usr/bin/perl

use LWP::Simple;                # From CPAN
use JSON qw( decode_json );     # From CPAN
use Data::Dumper;               # Perl core module
use strict;                     # Good practice
use warnings;                   # Good practice

my $base_url = "https://www.whoisxmlapi.com/whoisserver/DNSService";
my $type = "_all";
my $domain_name = "google.com";
my $user_name = "";
my $password = "";


#######################
# Use a JSON resource #
#######################
print "JSON\n---\n".getDnsData("json");

#######################
# Use an XML resource #
#######################
print "XML\n---\n".getDnsData("xml");

#######################
#   getting DNS Data  #
#######################
sub getDnsData {
    my $format = $_[0];
    my $url = "$base_url?type=$type&domainName=$domain_name&outputFormat=$format";
    if ($user_name ne "") {
        $url = "$url&userName=$user_name&password=$password";
    }

    print "Get data by URL: $url\n";
    # 'get' is exported by LWP::Simple;
    my $object = get($url);
    die "Could not get $base_url!" unless defined $object;
    return $object
}