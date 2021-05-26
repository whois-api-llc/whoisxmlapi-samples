#!/usr/bin/perl

use JSON qw( decode_json );       # From CPAN
use LWP::Protocol::https;         # From CPAN
use LWP::Simple;                  # From CPAN
use URI::Escape qw( uri_escape ); # From CPAN

use strict;
use warnings;

########################
# Fill in your details #
########################
my $api_key = 'Your email verification api key';
my $emailAddress = 'support@whoisxmlapi.com';

my $base_url = 'https://emailverification.whoisxmlapi.com/api/v1';


#########################
# Use the JSON resource #
#########################
my $responseJson = JSON->new->decode(getEmailVerifyRecord('json'));
print "JSON\n---\n" . JSON->new->pretty->encode($responseJson);

########################
# Use the XML resource #
########################
print "\nXML\n---\n" . getEmailVerifyRecord('xml');

#######################
# Getting the Data    #
#######################
sub getEmailVerifyRecord {
    my $format = $_[0];
    my $url = $base_url
            . '?apiKey=' . uri_escape($api_key)
            . '&emailAddress=' . uri_escape($emailAddress)
            . '&outputFormat=' . uri_escape($format);

    print "Get URL: $url\n";

    # 'get' is exported by LWP::Simple;
    my $object = get($url);

    die "Could not get $base_url!" unless defined $object;
    return $object
}