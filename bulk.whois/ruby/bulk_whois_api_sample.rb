require 'json'
require 'net/https'
require 'openssl'
require 'uri'

domains = %w[
  google.com
  example.com
  whoisxmlapi.com
  twitter.com
]

csv_filename = 'whois_records.csv'

########################
# Fill in your details #
########################

def call_api(path, data)
  base_data = {
    apiKey: 'Your bulk whois api key',
    outputFormat: 'json'
  }

  url = 'https://www.whoisxmlapi.com/BulkWhoisLookup/bulkServices' + path

  body = base_data.merge(data).to_json

  http_post(url, body)
end

def create_request(domains)
  data = {
    domains: domains
  }

  response = JSON.parse(call_api('/bulkWhois', data))

  code = response['messageCode']
  message = response['message']

  raise "#{code}: #{message}" if code != 200

  response['requestId']
end

def download(id, file_name)
  data = {
    requestId: id,
    startIndex: 1
  }

  File.write(file_name, call_api('/download', data))
end

def get_records(id, start, max)
  data = {
    requestId: id,
    startIndex: start,
    maxRecords: max
  }

  JSON.parse(call_api('/getRecords', data))
end

def http_post(url, data)
  uri = URI.parse(url)
  http = Net::HTTP.new(uri.host, uri.port)

  # Connect using ssl
  http.use_ssl = true
  http.verify_mode = OpenSSL::SSL::VERIFY_NONE
  request = Net::HTTP::Post.new(uri.request_uri)

  # Set headers
  request.add_field('Content-Type', 'application/json')
  request.add_field('Accept', 'application/json')
  request.body = data

  http.request(request).body
end

puts 'Requesting bulk whois lookup...'

id = create_request(domains)

puts "Request created: #{id}"

puts 'Waiting for processing...'

loop do
  response = get_records(id, domains.count + 1, 0)
  records_left = response['recordsLeft']

  puts "Records left: #{records_left}"

  break if records_left.zero?

  sleep 3
end

puts 'Downloading results...'

download(id, csv_filename)

puts "Saved to #{csv_filename}"
