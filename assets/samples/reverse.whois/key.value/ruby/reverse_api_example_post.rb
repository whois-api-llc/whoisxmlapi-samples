require 'uri'
require 'net/https'
require 'openssl'

url = URI("https://www.whoisxmlapi.com/reverse-whois-api/search.php")

http = Net::HTTP.new(url.host, url.port)
http.use_ssl = true
http.verify_mode = OpenSSL::SSL::VERIFY_NONE

request = Net::HTTP::Post.new(url)
request["cache-control"] = 'no-cache'
request.body = "{\"terms\": [{\"section\": \"Registrant\", \"attribute\":"\
               + " \"Email\", \"value\": \"support@whoisxmlapi.com\", "\
               + "\"matchType\": \"anywhere\", \"exclude\": \"false\"}],"\
               + "\"recordsCounter\": \"false\", \"outputFormat\": \"json\","\
               + " \"username\": \"Your whois api username\","\
               + " \"password\": \"Your whois api password\""\
               + ", \"rows\": \"10\",  \"searchType\": \"current\"}"

response = http.request(request)
puts response.read_body