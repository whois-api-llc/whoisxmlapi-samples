#!/usr/bin/perl

use LWP::Simple;                # From CPAN
use JSON qw( decode_json );     # From CPAN
use Data::Dumper;
use strict;
use warnings;

my $user_name = "your whois api username";
my $password = "your whois api password";

my $emailAddress = 'support@whoisxmlapi.com';
my $base_url = 'https://www.whoisxmlapi.com/whoisserver/EmailVerifyService';
my $validateDNS = 'true', my $validateSMTP = 'true', my $checkCatchAll = 'true';
my $checkFree = 'true', my $checkDisposable = 'true';

my $url = "$base_url?emailAddress=$emailAddress&validateDNS=$validateDNS" 
    . "&validateSMTP=$validateSMTP&checkCatchAll=checkCatchAll" 
    . "&checkFree=checkFree&checkDisposable=checkDisposable" 
    . "&outputFormat=json&userName=$user_name&password=$password";
getprint($url);