#!/usr/bin/perl

use LWP::Simple;                # From CPAN and install LWP::Protocol::Https
use JSON qw( decode_json );     # From CPAN
use Data::Dumper;               # Perl core module
use strict;
use warnings;

my $base_url = "https://www.whoisxmlapi.com/brand-alert-api/search.php";
my $term1 = "whois";
my $exclude_term1 = "domain";
my $exclude_term2 = "news";
my $user_name = "Your whois api username";
my $password = "Your whois api password";

print "JSON\n---\n".getDnsData("json");

sub getDnsData {
    my $format = $_[0];
    my $url = "$base_url?username=$user_name&password=$password&term1="
        . "$term1&exclude_term1=$exclude_term1&exclude_term2=" 
        . "$exclude_term2&output_format=$format";
    return get($url);
}