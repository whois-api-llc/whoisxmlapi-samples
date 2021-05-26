#!/usr/bin/perl

use HTTP::Request::Common qw{ POST }; # From CPAN
use JSON qw( decode_json );           # From CPAN
use LWP::UserAgent;                   # From CPAN

use strict;
use warnings;

my $url = 'https://www.whoisxmlapi.com/BulkWhoisLookup/bulkServices';

########################
# Fill in your details #
########################
my $apiKey = 'Your bulk whois api key';

my @domains = ('google.com', 'whoisxmlapi.com');

#########################
# Create a bulk request #
#########################

print "Requesting bulk processing...\n";

my $req_id = create_request(\@domains);

print "    request created: $req_id\n";

###############################
# Download results when ready #
###############################

print "Waiting for processing to finish...\n";

while (1) {
    my $req_records = get_records($req_id, @domains + 1, 0);
    my $records_left = $req_records->{recordsLeft};

    if ($records_left == 0) {
        last;
    }

    sleep(3);
}

print "Downloading results...\n";

my $records = get_records($req_id, 1, @domains + 0);

print JSON->new->pretty->encode($records);

sub create_request {
    my @domain_array = @{$_[0]};

    my $domains_str = '"' . join('", "', @domain_array) . '"';

    my $data = '{
        "apiKey": "' . $apiKey . '",
        "outputFormat": "json",
        "domains": [
          '. $domains_str . '
        ]
    }';

    my $result = post_data('/bulkWhois', $data);

    my $json = JSON->new->decode($result);

    if ($json->{messageCode} != 200) {
        die $json->{message};
    }

    return $json->{requestId};
}

sub get_records {
    my ($id, $start, $max) = @_;

    my $data = '{
        "apiKey": "' . $apiKey . '",
        "outputFormat": "json",
        "requestId": "' . $id . '",
        "startIndex": ' . $start . ',
        "maxRecords": ' . $max . '
    }';

    my $result = post_data('/getRecords', $data);

    my $json = JSON->new->decode($result);

    return $json;
}

sub post_data {
    my ($path, $data) = @_;

    my $ua = LWP::UserAgent->new(ssl_opts => { verify_hostname => 0 });
    my $req = HTTP::Request->new('POST', $url . $path);

    $req->header('Content-Type' => 'application/json');
    $req->header('Accept', 'application/json');
    $req->content($data);

    my $result = $ua->request($req);

    return $result->content;
}
