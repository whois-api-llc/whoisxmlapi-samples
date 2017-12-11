#!/usr/bin/perl

use LWP::Simple;                # From CPAN
use JSON qw( decode_json );     # From CPAN
use Data::Dumper;               # Perl core module
use strict;                     # Good practice
use warnings;                   # Good practice

########################
# Fill in your details #
########################
my $api_key = "Your_email_verification_api_key";

my $emailAddress = 'support@whoisxmlapi.com';
my $base_url = 'https://emailverification.whoisxmlapi.com/api/v1';


#######################
# Use a JSON resource #
#######################
print "JSON\n---\n".getEmailVerifyRecord("json");

#######################
# Use an XML resource #
#######################
print "\nXML\n---\n".getEmailVerifyRecord("xml");

#######################
#   getting DNS Data  #
#######################
sub getEmailVerifyRecord {
    my $format = $_[0];
    my $url = "$base_url?apiKey=$api_key&emailAddress=$emailAddress&outputFormat=$format";

    print "Get data by URL: $url\n";
    # 'get' is exported by LWP::Simple;
    my $object = get($url);
    die "Could not get $base_url!" unless defined $object;
    return $object
}
