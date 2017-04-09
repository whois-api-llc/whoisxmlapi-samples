require 'base64'
require 'openssl'
require 'open-uri'

domain = 'example.com'
key = 'your whois api key'
secret = 'your whois api secret key'
username = 'your whois api username'

time = (Time.now.to_f * 1000).to_i
req = Base64.encode64('{"t":' + time.to_s + ',"u":"' + username + '"}')
data = username + time.to_s + key
digest = OpenSSL::HMAC.hexdigest(OpenSSL::Digest::MD5.new, secret, data)

url = 'http://www.whoisxmlapi.com/whoisserver/WhoisService?'\
    + 'requestObject=' + req + '&digest=' + digest + '&domainName=' + domain

puts open(url).read