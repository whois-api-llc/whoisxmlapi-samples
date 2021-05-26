try:
    import http.client as http
except ImportError:
    import httplib as http

import json

api_key = 'Your brand alert api key'

payload = {
    'includeSearchTerms': [
        'whois'
    ],
    'excludeSearchTerms': [
        '.win'
    ],
    'mode': 'purchase',
    'sinceDate': '2018-07-14',
    'apiKey': api_key,
    'responseFormat': 'json'
}


def print_response(txt):
    response_json = json.loads(txt)
    print(json.dumps(response_json, indent=4, sort_keys=True))


headers = {
    'Content-Type': 'application/json',
    'Accept': 'application/json'
}

conn = http.HTTPSConnection('brand-alert.whoisxmlapi.com')

conn.request(
        'POST',
        '/api/v2',
        json.dumps(payload),
        headers)

response = conn.getresponse()
text = response.read().decode('utf8')

print_response(text)
