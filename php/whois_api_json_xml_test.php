<?php

//////////////////////////
// Fill in your details //
//////////////////////////
$username = "YOUR_USERNAME";
$password = "YOUR_PASSWORD";
$domain = "google.com";
$format = "JSON"; //or XML

$url = 'http://www.whoisxmlapi.com/whoisserver/WhoisService?domainName='. $domain .'&username='. $username .'&password='. $password .'&outputFormat='. $format;
if($format=='JSON'){
  /////////////////////////
  // Use a JSON resource //
  /////////////////////////
  // Get and build the JSON object
  $result = json_decode(file_get_contents($url));

  // Print out a nice informative string
  print ("<div>JSON:</div>" . RecursivePrettyPrint($result));
}
else{
  ////////////////////////
  // Use a XML resource //
  ////////////////////////

  $url = 'http://www.whoisxmlapi.com/whoisserver/WhoisService?domainName='. $domain .'&username='. $username .'&password='. $password .'&outputFormat='. $format;
  // Get and build the XML associative array
  $parser = new XMLtoArray();
  $result = array("WhoisRecord" =>$parser->ParseXML($url));

  // Print out a nice informative string
  print ("<div>XML:</div>" . RecursivePrettyPrint($result));
}


// Function to recursively print all properties of an object and their values
function RecursivePrettyPrint($obj)
{
  $str = "";
  foreach ((array)$obj as $key => $value)
  {
    if (!is_string($key)) // XML parsing leaves a little to be desired as it fills our obj with key/values with just whitespace, ignore that whitespace at the cost of losing hostNames and ips in the final printed result
      continue;
    $str .= '<div style="margin-left: 25px;border-left:1px solid black">' . $key . ": ";
    if (is_string($value))
      $str .= $value;
    else
      $str .= RecursivePrettyPrint($value);
    $str .= "</div>";
  }
  
  return $str;
}

// Class that simply turns an xml tree into a multilevel associative array
class XMLtoArray 
{
  private $root;
  private $stack;

  public function __construct()
  {
    $this->root = null;
    $this->stack = array();
  }
  
  function ParseXML($feed_url)
  {
    $xml_parser = xml_parser_create(); 
    
    xml_parser_set_option($xml_parser, XML_OPTION_CASE_FOLDING, 0);// or throw new Exception('Unable to Set Case Folding option on XML Parser!');
    xml_parser_set_option($xml_parser, XML_OPTION_SKIP_WHITE, 1);// or throw new Exception('Unable to Set Skip Whitespace option on XML Parser!');
    xml_set_object($xml_parser, $this); 
    xml_set_element_handler($xml_parser, "startElement", "endElement"); 
    xml_set_character_data_handler($xml_parser, "characterData"); 

    $fp = fopen($feed_url,"r");// or throw new Exception("Unable to read URL!"); 

    while ($data = fread($fp, 4096)) 
      xml_parse($xml_parser, $data, feof($fp));// or throw new Exception(sprintf("XML error: %s at line %d", xml_error_string(xml_get_error_code($xml_parser)), xml_get_current_line_number($xml_parser))); 

    fclose($fp); 

    xml_parser_free($xml_parser);
    
    return $this->root;
  }
  
  public function startElement($parser, $tagName, $attrs) 
  {
    if ($this->root == null)
    {
      $this->root = array();
      $this->stack[] = &$this->root;
    }
    else
    {
      $parent = &$this->stack[count($this->stack)-1];
      if (!is_array($parent))
        $parent = array($parent);
      if (isset($parent[$tagName]))
      {
        if (!is_array($parent[$tagName]))
          $parent[$tagName] = array($parent[$tagName]);
      }
      else
        $parent[$tagName] = null;
      
      $this->stack[] = &$parent[$tagName];
    }
  }

  public function endElement($parser, $tagName)
  {
    array_pop($this->stack);
  }

  public function characterData($parser, $data) 
  {
    $data = trim($data);
      
    $current = &$this->stack[count($this->stack)-1];
    if (is_array($current))
      $current[] = $data;
    else
      $current = $data;
  }
}

?>