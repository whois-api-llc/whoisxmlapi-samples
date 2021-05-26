try:
    from urllib.request import urlopen, pathname2url
except ImportError:
    from urllib import pathname2url
    from urllib2 import urlopen

domain = 'example.com'
apiKey = 'Your dns lookup api key'
checkType = '_all'

url = 'http://www.whoisxmlapi.com/whoisserver/DNSService?'\
    + 'type=' + pathname2url(checkType)\
    + '&domainName=' + pathname2url(domain)\
    + '&apiKey=' + pathname2url(apiKey)

print(urlopen(url).read().decode('utf8'))
