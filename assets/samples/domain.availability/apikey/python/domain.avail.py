try:
    from urllib.request import urlopen
except ImportError:
    from urllib2 import urlopen

import base64
import hashlib
import hmac
import time

domain = 'example.com'
key = 'your whois api key'
secret = 'your whois api secret key'
username = 'your whois api username'

time = int(round(time.time() * 1000))
req = base64.b64encode(
    ('{"t":' + str(time) + ',"u":"' + username + '"}').encode('ascii')
)
digest = hmac.new(
    secret.encode('ascii'), 
    (username + str(time) + key).encode('ascii'), 
    hashlib.md5
).hexdigest()

url = 'http://www.whoisxmlapi.com/whoisserver/WhoisService?'\
    + 'requestObject=' + req.decode('ascii') + '&digest=' + digest \
    + '&domainName=' + domain + '&cmd=GET_DN_AVAILABILITY'

print(urlopen(url).read().decode('utf8'))