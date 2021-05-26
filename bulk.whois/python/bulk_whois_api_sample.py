import requests
import logging
import json
import time
from argparse import ArgumentParser

"""
sample:
./bulkwhois.py -u test -p user -d yandex.ru whoisxmlapi.com --output bulk
"""

# Preparing arguments
argparser = ArgumentParser(description='BulkWhois querry')
argparser.add_argument('-u', '--apiKey', help='Your bulk whois api key', type=str, required=True)
argparser.add_argument('-d', '--domains', help='list of domains separated by spaces',
                       type=str, nargs='+', required=True)
argparser.add_argument('--interval', help='requesting interval', type=int, default=15)
argparser.add_argument('--format', help='data format', type=str, choices=['json', 'xml'], default='json')
argparser.add_argument('--output', help='output name without extension', type=str, required=True)
args = argparser.parse_args()

logging.basicConfig(
        format='%(asctime)s [%(levelname)s] %(message)s',
        level=logging.DEBUG
    )

bulkwhois_base_url = 'https://www.whoisxmlapi.com/BulkWhoisLookup/bulkServices/'
session = requests.session()

data = {
    "domains": args.domains,
    "apiKey": args.apiKey,
    "outputFormat": args.format
}

header = {'Content-Type': 'application/json'}
if args.format == 'xml':
    header = {'Content-Type': 'application/xml'}

logging.debug("data:" + str(data))
logging.debug("headers:" + str(header))

# Posting task to API
response = session.post(bulkwhois_base_url + 'bulkWhois',
                         data=json.dumps(data),
                         headers=header,
                         timeout=5)
if response.status_code != 200:
    logging.error("wrong response code: %i" % response.status_code)
    exit(1)
response_data = json.loads(response.text)
if response_data['messageCode'] == 200:
    logging.debug('Response: ' + response.text)
    del data['domains']
    data.update({
        'requestId': response_data['requestId'],
        'searchType': 'all',
        'maxRecords': 1,
        'startIndex': 1
    })
else:
    logging.error('Response: ' + response.text)
    exit(1)

# waiting for job complete
logging.debug("data:" + str(data))
recordsLeft = len(args.domains)
while recordsLeft > 0:
    time.sleep(args.interval)
    response = session.post(bulkwhois_base_url + 'getRecords',
                            headers=header,
                            data=json.dumps(data))
    if response.status_code != 200:
        logging.error("wrong response code: %i" % response.status_code)
        exit(1)
    recordsLeft = json.loads(response.text)['recordsLeft']
    logging.debug('Response: ' + response.text)

data.update({'maxRecords': len(args.domains)})
# dump json data
time.sleep(args.interval)
response = session.post(bulkwhois_base_url + 'getRecords',
                        headers=header,
                        data=json.dumps(data))
with open(args.output + '.json','w') as json_file:
    json.dump(json.loads(response.text),json_file)

# download csv data
time.sleep(args.interval)
with open(args.output + '.csv', 'wt') as csv_file:
    response = session.post(bulkwhois_base_url + 'download',
                            headers=header,
                            data=json.dumps(data))
    for line in response.text.split('\n'):
        clear_line = line.strip()
        # remove blank lines
        if clear_line != '':
            csv_file.write(clear_line + '\n')
