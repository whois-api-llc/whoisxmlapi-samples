try: 
    import http.client as http
except:
    import httplib as http

password = 'your whois api password'
username = 'your whois api username'

payload = '{ "terms": [{ "section": "Registrant", "attribute": "Email",\
                         "value": "a@b.com", "exclude": false }],\
             "recordsCounter": false, "username": "USER", "password": "PASS"\
           }'.replace('USER', username).replace('PASS', password)

headers = {"Content-Type": "application/json", "Accept": "application/json"}
conn = http.HTTPSConnection('www.whoisxmlapi.com')
conn.request("POST", '/reverse-whois-api/search.php', payload, headers)

response = conn.getresponse()
text = response.read()
print(text)