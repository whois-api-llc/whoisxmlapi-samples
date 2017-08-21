#!/usr/bin/perl

use LWP::Simple;                # From CPAN
use JSON qw( decode_json );     # From CPAN
use Data::Dumper;               # Perl core module
use strict;                     # Good practice
use warnings;                   # Good practice

########################
# Fill in your details #
########################
my $user_name = "";
my $password = "";

my $emailAddress = 'support@whoisxmlapi.com';
my $base_url = 'https://www.whoisxmlapi.com/whoisserver/EmailVerifyService';
my $validateDNS = 'true';
my $validateSMTP = 'true';
my $checkCatchAll = 'true';
my $checkFree = 'true';
my $checkDisposable = 'true';


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
    my $url = "$base_url?emailAddress=$emailAddress&validateDNS=$validateDNS&validateSMTP=$validateSMTP&checkCatchAll=checkCatchAll&checkFree=checkFree&checkDisposable=checkDisposable&outputFormat=$format";
    if ($user_name ne "") {
        $url = "$url&userName=$user_name&password=$password";
    }

    print "Get data by URL: $url\n";
    # 'get' is exported by LWP::Simple;
    my $object = get($url);
    die "Could not get $base_url!" unless defined $object;
    return $object
}