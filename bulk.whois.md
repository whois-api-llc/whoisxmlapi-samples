<div class="toc">
        <a class="button" href="./brand.alert.html"><div class="toc-item">Brand Alert API</div></a>
	<a class="button" href="./dns.lookup.html"><div class="toc-item">DNS Lookup API</div></a>
        <a class="button" href="./domain.availability.html"><div class="toc-item">Domain Availability API</div></a>
	<a class="button" href="./email.verifier.html"><div class="toc-item">Email Verification API</div></a>
        <a class="button" href="./reg.alert.html"><div class="toc-item">Registrant Alert API</div></a>
        <a class="button" href="./reverse.whois.html"><div class="toc-item">Reverse Whois API</div></a>
	<a class="button" href="./"><div class="toc-item">Whois API</div></a>
</div>

# Making a query to Bulk Whois API web service


Here you'll find short examples of using
[www.whoisxmlapi.com](https://www.whoisxmlapi.com/) Hosted Bulk Whois Web API
implemented in multiple languages.

You can view more sample code, incl. dealing with the API's response formats,
requests order and more, in the
[repository]({{ site.github.repository_url }}).


Please, refer to
[Bulk Whois API User Guide](https://www.whoisxmlapi.com/bulk-whois-api-userguide.php) for
 request and response formats, usage limits.

<ul id="profileTabs" class="nav nav-tabs" role="tablist">
    <li class="active"><a href="#nodejs" data-toggle="tab">Node.js</a></li>
    <li><a href="#python" data-toggle="tab">Python</a></li>
</ul>

<div class="tab-content">

<div role="tabpanel" class="tab-pane active" id="nodejs">
<div class="container-fluid" markdown="1">
```js
{% include_relative assets/samples/bulk.whois/node/bulk.search.js %}
```
</div>
</div>

<div role="tabpanel" class="tab-pane" id="python">
<div class="container-fluid" markdown="1">
```python
{% include_relative assets/samples/bulk.whois/python/bulkwhois.py %}
```
</div>
</div>

