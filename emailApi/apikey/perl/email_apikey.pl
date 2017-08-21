use strict;
use warnings;
use Data::Dumper;
use LWP::Simple;                            # From CPAN
use JSON qw( decode_json encode_json );     # From CPAN
use Time::HiRes qw( time );                 # From CPAN
use Digest::HMAC_MD5 qw( hmac_md5_hex );    # From CPAN
use URI::Escape;                            # From CPAN
use MIME::Base64 qw( encode_base64 );

my $username = 'username';
my $apiKey = 'api_key';
my $secret = 'secret_key';

my $emailAddress = 'support@whoisxmlapi.com';
my $url = 'https://whoisxmlapi.com/whoisserver/EmailVerifyService?';

my $timestamp = int((time * 1000 + 0.5));
my $digest = generateDigest($username, $timestamp, $apiKey, $secret);

my $requstString = buildRequest(
    $username,
    $timestamp,
    $digest,
    $emailAddress
);
my $response = get($url . $requstString);
if (index($response, 'Request timeout')){
    $timestamp = int((time * 1000 + 0.5));
    $digest = generateDigest($username, $timestamp, $apiKey, $secret);
    $requstString = buildRequest(
        $username,
        $timestamp,
        $digest,
        $emailAddress
    );
    $response = get($url . $requstString);
}
printResponse($response);


sub generateDigest{
    my ($username, $timestamp, $apiKey, $secret) = @_;

    my $digest = $username . $timestamp . $apiKey;
    my $hash = hmac_md5_hex($digest, $secret);
    return uri_escape($hash);
}

sub buildRequest{
    my ($username, $timestamp, $digest, $emailAddress) = @_;
    my $requestString = 'requestObject=';
    my %request =(
        'u' => $username,
        't' => $timestamp
    );
    my $requestJson = encode_json(\%request);
    my $requestBase64 = encode_base64($requestJson);
    $requestString .= uri_escape($requestBase64);
    $requestString .= "&emailAddress=";
    $requestString .= $emailAddress;
    $requestString .= '&digest=';
    $requestString .= $digest;
    $requestString .= "&validateDNS=true&validateSMTP=true";
    $requestString .= "&checkCatchAll=true&checkFree=true&checkDisposable=true";
    $requestString .= '&outputFormat=json';
    return $requestString;
}

sub printResponse{
    my ($response) = @_;
    print $response;
    print "\n---------------------------------------\n";
}