try:
    from urllib.request import urlopen
    from urllib import request
except ImportError:
    from urllib2 import urlopen
    import urllib2 as request
import json, time

domains = ['whoisxmlapi.com', 'threatintelligenceplatform.com']
username = 'Your whois api username'
password = 'Your whois api password'
output_format = 'json'
interval = 10

bulkwhois_base_url = 'https://www.whoisxmlapi.com/BulkWhoisLookup/bulkServices/'
data = {
    "domains": domains, "password": password,  "username": username, 
     "outputFormat": output_format
}
req = request.Request(bulkwhois_base_url + 'bulkWhois')
req.add_header('Content-Type', 'application/json')
response = urlopen(req, json.dumps(data).encode('utf-8'))
response_data = json.loads(response.read().decode('utf-8'))
print('Response: ' + json.dumps(response_data))
del data['domains']
data.update({
   'requestId': response_data['requestId'], 'searchType': 'all', 'maxRecords': 1,
   'startIndex': 1
})
recordsLeft = len(domains)
req2 = request.Request(bulkwhois_base_url + 'getRecords')
while recordsLeft > 0:
    req2.add_header('Content-Type', 'application/json')
    time.sleep(interval)
    response = urlopen(req2, json.dumps(data).encode('utf-8'))
    recordsLeft = json.loads(response.read().decode('utf-8'))['recordsLeft']
data.update({'maxRecords': len(domains)})
time.sleep(interval)
response = urlopen(req2, json.dumps(data).encode('utf-8'))
print(response.read().decode('utf-8'))