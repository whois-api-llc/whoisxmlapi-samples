#!/usr/bin/perl

use LWP::Simple;                # From CPAN, also install LWP::Protocol::Https
use JSON qw( decode_json );     # From CPAN
use Data::Dumper;               # Perl core module
use strict;                     # Good practice
use warnings;                   # Good practice

my $base_url = "https://www.whoisxmlapi.com/reverse-whois-api/search.php";
my $mode = "preview";
my $term = "wikimedia";
my $user_name = "your whois api username";
my $password = "your whois api password";

print "JSON\n---\n".getDnsData("json");

sub getDnsData {
    my $format = $_[0];
    my $url = "$base_url?mode=$mode&term1=$term" 
        . "&outputFormat=$format&username=$user_name" 
        . "&password=$password";

    my $object = get($url);
    die "Could not get $base_url!" unless defined $object;
    return $object
}