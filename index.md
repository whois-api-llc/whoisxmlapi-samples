<div class="toc">
        <a class="button" href="/brand.alert.html"><div class="toc-item">Brand Alert API</div></a>
	<a class="button" href="/bulk.whois.html"><div class="toc-item">Bulk Whois API</div></a>
	<a class="button" href="/dns.lookup.html"><div class="toc-item">DNS Lookup API</div></a>
        <a class="button" href="/domain.availability.html"><div class="toc-item">Domain Availability API</div></a>
	<a class="button" href="/email.verifier.html"><div class="toc-item">Email Verification API</div></a>
        <a class="button" href="/reg.alert.html"><div class="toc-item">Registrant Alert API</div></a>
        <a class="button" href="/reverse.whois.html"><div class="toc-item">Reverse Whois API</div></a>
</div>

# Making a query to Whois API web service


Here you'll find short examples of using
[www.whoisxmlapi.com](https://www.whoisxmlapi.com/) Hosted Whois Web API
implemented in multiple languages.

You can view more sample code, incl. dealing with the API's response formats,
regenerating access tokens and more, in the
[repository]({{ site.github.repository_url }}).


Please, refer to
[Whois API User Guide](https://www.whoisxmlapi.com/whois-api-guide.php) for
authentication instructions.

## Password authentication

<ul id="profileTabs" class="nav nav-tabs" role="tablist">
    <li class="active"><a href="#csharp" data-toggle="tab">C#</a></li>
    <li><a href="#java" data-toggle="tab">Java</a></li>
    <li><a href="#jquery" data-toggle="tab">jQuery</a></li>
    <li><a href="#nodejs" data-toggle="tab">Node.js</a></li>
    <li><a href="#php" data-toggle="tab">PHP</a></li>
    <li><a href="#perl" data-toggle="tab">Perl</a></li>
    <li><a href="#powershell" data-toggle="tab">PowerShell</a></li>
    <li><a href="#python" data-toggle="tab">Python</a></li>
    <li><a href="#ruby" data-toggle="tab">Ruby</a></li>
</ul>

<div class="tab-content">

<div role="tabpanel" class="tab-pane active" id="csharp">
<div class="container-fluid" markdown="1"> 
```c#
{% include_relative assets/samples/whois/password/net/WhoisApiQuery.cs %}
```
</div>
</div>

<div role="tabpanel" class="tab-pane" id="java">
<div class="container-fluid" markdown="1"> 
```java
{% include_relative assets/samples/whois/password/java/WhoisApiQuery.java %}
```
</div>
</div>

<div role="tabpanel" class="tab-pane" id="jquery">
<div class="container-fluid" markdown="1">
```html
{% include_relative assets/samples/whois/password/js/whois-api-query.html %}
```
</div>
</div>

<div role="tabpanel" class="tab-pane" id="nodejs">
<div class="container-fluid" markdown="1">
```js
{% include_relative assets/samples/whois/password/nodejs/WhoisApiQuery.js %}
```
</div>
</div>

<div role="tabpanel" class="tab-pane" id="php">
<div class="container-fluid" markdown="1">
```php
{% include_relative assets/samples/whois/password/php/WhoisApiQuery.php %}
```
</div>
</div>

<div role="tabpanel" class="tab-pane" id="perl">
<div class="container-fluid" markdown="1">
```perl
{% include_relative assets/samples/whois/password/perl/WhoisApiQuery.pl %}
```
</div>
</div>

<div role="tabpanel" class="tab-pane" id="powershell">
<div class="container-fluid" markdown="1">
```posh
{% include_relative assets/samples/whois/password/powershell/WhoisApiQuery.ps1 %}
```
</div>
</div>

<div role="tabpanel" class="tab-pane" id="python">
<div class="container-fluid" markdown="1">
```python
{% include_relative assets/samples/whois/password/python/WhoisApiQuery.py %}
```
</div>
</div>

<div role="tabpanel" class="tab-pane" id="ruby">
<div class="container-fluid" markdown="1">
```ruby
{% include_relative assets/samples/whois/password/ruby/WhoisApiQuery.rb %}
```
</div>
</div>

</div>

## API key authentication

<ul id="profileTabs" class="nav nav-tabs" role="tablist">
    <li class="active"><a href="#csharp-key" data-toggle="tab">C#</a></li>
    <li><a href="#java-key" data-toggle="tab">Java</a></li>
    <li><a href="#jquery-key" data-toggle="tab">jQuery</a></li>
    <li><a href="#nodejs-key" data-toggle="tab">Node.js</a></li>
    <li><a href="#php-key" data-toggle="tab">PHP</a></li>
    <li><a href="#perl-key" data-toggle="tab">Perl</a></li>
    <li><a href="#powershell-key" data-toggle="tab">PowerShell</a></li>
    <li><a href="#python-key" data-toggle="tab">Python</a></li>
    <li><a href="#ruby-key" data-toggle="tab">Ruby</a></li>
</ul>

<div class="tab-content">

<div role="tabpanel" class="tab-pane active" id="csharp-key">
<div class="container-fluid" markdown="1"> 
```c#
{% include_relative assets/samples/whois/apikey/net/WhoisApiKeyAuth.cs %}
```
</div>
</div>

<div role="tabpanel" class="tab-pane" id="java-key">
<div class="container-fluid" markdown="1"> 
```java
{% include_relative assets/samples/whois/apikey/java/WhoisApiKeyAuth.java %}
```
</div>
</div>

<div role="tabpanel" class="tab-pane" id="jquery-key">
<div class="container-fluid" markdown="1"> 
```html
{% include_relative assets/samples/whois/apikey/js/whois-api-key-auth.html %}
```
</div>
</div>

<div role="tabpanel" class="tab-pane" id="nodejs-key">
<div class="container-fluid" markdown="1"> 
```js
{% include_relative assets/samples/whois/apikey/nodejs/WhoisApiKeyAuth.js %}
```
</div>
</div>

<div role="tabpanel" class="tab-pane" id="php-key">
<div class="container-fluid" markdown="1"> 
```php
{% include_relative assets/samples/whois/apikey/php/WhoisApiKeyAuth.php %}
```
</div>
</div>

<div role="tabpanel" class="tab-pane" id="perl-key">
<div class="container-fluid" markdown="1"> 
```perl
{% include_relative assets/samples/whois/apikey/perl/WhoisApiKeyAuth.pl %}
```
</div>
</div>

<div role="tabpanel" class="tab-pane" id="powershell-key">
<div class="container-fluid" markdown="1"> 
```posh
{% include_relative assets/samples/whois/apikey/powershell/WhoisApiKeyAuth.ps1 %}
```
</div>
</div>

<div role="tabpanel" class="tab-pane" id="python-key">
<div class="container-fluid" markdown="1"> 
```python
{% include_relative assets/samples/whois/apikey/python/WhoisApiKeyAuth.py %}
```
</div>
</div>

<div role="tabpanel" class="tab-pane" id="ruby-key">
<div class="container-fluid" markdown="1"> 
```ruby
{% include_relative assets/samples/whois/apikey/ruby/WhoisApiKeyAuth.rb %}
```
</div>
</div>

</div>
