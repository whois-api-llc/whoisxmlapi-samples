var http = require("https");

var options = {
  "method": "POST",
  "hostname": "www.whoisxmlapi.com",
  "port": 443,
  "path": "/reverse-whois-api/search.php",
  "headers": {
    "cache-control": "no-cache"
  }
};

var req = http.request(options, function (res) {

  var chunks = '';
  res.on("data", function (chunk) {chunks += chunk;} );
  res.on("end", function () {
    console.log(chunks);
  });
});
req.write("{\"terms\": [{\"section\": \"Registrant\", \"attribute\":"
  + " \"Email\", \"value\": \"support@whoisxmlapi.com\", \"matchType\":"
  + " \"exact\", \"exclude\": \"false\"}], \"recordsCounter\": \"false\","
  + " \"outputFormat\": \"json\", \"username\": \"your whois api username\", "
  + " \"password\": \"your whois api password\", \"rows\": \"10\", "
  + "\"searchType\": \"current\"}");
req.end();