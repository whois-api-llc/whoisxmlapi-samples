<?php

namespace {
    define('DOMAINS', 'google.com,youtube.com,facebook.com,gmail.com');
    define('URL', 'https://www.whoisxmlapi.com/BulkWhoisLookup/bulkServices');
    define('API_KEY', 'Your Bulk Whois Api key');
}

namespace whoisxmlapi\samples\bulkwhois
{
    /**
     * Class BaseClass
     * @package whoisxmlapi\samples\bulkwhois
     */
    abstract class BaseClass
    {
        /**
         * @return string
         */
        public static function className()
        {
            return get_called_class();
        }
    }

    /**
     * Class AppBulkWhois
     * @package whoisxmlapi\samples\bulkwhois
     */
    class AppBulkWhois extends SampleApp
    {
        /**
         * @return object
         */
        public function boot()
        {
            $factoryAbs = ReflectionFactoryInterface::CLASS_NAME;
            $factory = $this->_make($factoryAbs);
            $resolverAbs = ResolverInterface::CLASS_NAME;
            return $this->_make($resolverAbs, $factory, $this->_bindings);
        }
    }

    /**
     * Class SampleApp
     * @package whoisxmlapi\samples\bulkwhois
     */
    abstract class SampleApp extends BaseClass
    {
        /**
         * @var array
         */
        protected $_bindings;

        /**
         * SampleApp constructor.
         * @param array $bindings
         */
        public function __construct(array $bindings)
        {
            $this->_bindings = $bindings;
        }

        /**
         * @param string $class
         * @return object
         */
        protected function _make($class)
        {
            $args = array_slice(func_get_args(), 1);
            $firstArg = Helpers::getArrValue(0, $args);
            $secondArg = Helpers::getArrValue(1, $args);
            $concrete = Helpers::getArrValue($class, $this->_bindings);
            return new $concrete($firstArg, $secondArg);
        }
    }

    /**
     * Class Helpers
     * @package whoisxmlapi\samples\bulkwhois
     */
    class Helpers
    {
        /**
         * @param string|int $key
         * @param array $array
         * @param mixed $default
         * @return mixed
         */
        public static function getArrValue($key, array $array, $default=null)
        {
            return isset($array[$key]) ? $array[$key] : $default;
        }
    }

    /**
     * Interface ReflectionFactoryInterface
     * @package whoisxmlapi\samples\bulkwhois
     */
    interface ReflectionFactoryInterface
    {
        const CLASS_NAME = __CLASS__;

        /**
         * @param string $class
         * @return \ReflectionClass
         */
        public function makeClass($class);
    }

    /**
     * Class ReflectionFactory
     * @package whoisxmlapi\samples\bulkwhois
     */
    class ReflectionFactory
        extends BaseClass
        implements ReflectionFactoryInterface
    {
        /**
         * @param string $class
         * @return \ReflectionClass
         * @throws \ReflectionException
         */
        public function makeClass($class)
        {
            return new \ReflectionClass($class);
        }
    }

    /**
     * Interface ResolverInterface
     * @package whoisxmlapi\samples\bulkwhois
     */
    interface ResolverInterface
    {
        const CLASS_NAME = __CLASS__;

        /**
         * @param string $class
         * @return object
         */
        public function make($class);
    }

    /**
     * Class AppResolver
     * @package whoisxmlapi\samples\bulkwhois
     */
    class AppResolver extends BaseClass implements ResolverInterface
    {
        /**
         * @var array[string]string
         */
        protected $_bindings;

        /**
         * @var ReflectionFactoryInterface
         */
        protected $_reflectionFactory;

        /**
         * SampleApp constructor.
         * @param ReflectionFactoryInterface $reflectionFactory
         * @param array $bindings
         */
        public function __construct(
            ReflectionFactoryInterface $reflectionFactory, array $bindings)
        {
            $this->_reflectionFactory = $reflectionFactory;
            $this->_bindings = $bindings;
        }

        /**
         * @param string $class
         * @return object
         */
        public function make($class)
        {
            $cfg = Helpers::getArrValue($class, $this->_bindings, array());
            $concrete = Helpers::getArrValue(0, array_keys($cfg));
            $paramValues = Helpers::getArrValue($concrete, $cfg, array());
            $concreteClass = $concrete ?: $class;
            $reflector = $this->_reflectionFactory->makeClass($concreteClass);
            $constructor = $reflector->getConstructor();

            if (empty($constructor))
                return $reflector->newInstanceWithoutConstructor();

            $parameters = $constructor->getParameters();
            $dependencies = $this->_getDependencies($parameters,$paramValues);

            return $reflector->newInstanceArgs($dependencies);
        }

        /**
         * @param array $parameters
         * @param array $values
         * @return array
         */
        protected function _getDependencies(array $parameters, array $values)
        {
            $result = array();
            foreach ($parameters as $param)
                $result[] = $this->_makeParam($param, $values);

            return $result;
        }

        /**
         * @param \ReflectionParameter $param
         * @param array $vals
         * @return mixed
         */
        protected function _makeParam(\ReflectionParameter $param,array $vals)
        {
            $dependency = $param->getClass();
            if (!empty($dependency))
                return $this->make($dependency->name);

            $isDefaultSet = $param->isDefaultValueAvailable();
            $default = $isDefaultSet ? $param->getDefaultValue() : null;

            return Helpers::getArrValue($param->name, $vals, $default);
        }
    }

    /**
     * Interface ApiExampleInterface
     * @package whoisxmlapi\samples\bulkwhois
     */
    interface ApiExampleInterface
    {
        const CLASS_NAME = __CLASS__;

        public function run();
    }

    /**
     * Class GetWhoisRecordsCsvExample
     * @package whoisxmlapi\samples\bulkwhois
     */
    class GetWhoisRecordsCsvExample
        extends ApiExampleAbstract
        implements ApiExampleInterface
    {
        const DELAY_RETRY = 5;
        const KEY_DOMAINS = 'domains';

        public function run()
        {
            $doms =Helpers::getArrValue(static::KEY_DOMAINS, $this->_params);
            $id =$this->_client()->createRequest($doms ?: array())->requestId;
            $this->_waitForProcessing($id, count($doms));
            print($this->_client()->download($id));
        }

        /**
         * @param string $requestId
         * @param int $domainCount
         */
        protected function _waitForProcessing($requestId, $domainCount)
        {
            while (true) {
                $res =
                    $this->_client()->getRecords($requestId,$domainCount+1,0);
                if (! (bool) $res->recordsLeft)
                    break;
                sleep(static::DELAY_RETRY);
            }
        }
    }

    /**
     * Class ApiExampleAbstract
     * @package whoisxmlapi\samples\bulkwhois
     */
    abstract class ApiExampleAbstract extends BaseClass
    {
        /**
         * @var ApiClientInterface
         */
        protected $_apiClient;

        /**
         * @var array
         */
        protected $_params;

        /**
         * ApiExampleAbstract constructor.
         * @param ApiClientInterface $client
         * @param array $params
         */
        public function __construct(ApiClientInterface $client, array $params)
        {
            $this->_apiClient = $client;
            $this->_params = $params;
        }

        /**
         * @return ApiClientInterface
         */
        protected function _client()
        {
            return $this->_apiClient;
        }
    }

    /**
     * Interface ApiClientInterface
     * @package whoisxmlapi\samples\bulkwhois
     */
    interface ApiClientInterface
    {
        const CLASS_NAME = __CLASS__;

        /**
         * @param string[] $domains
         * @return \stdClass
         */
        public function createRequest(array $domains);

        /**
         * @param string $requestId
         * @return string
         */
        public function download($requestId);

        /**
         * @param string $requestId
         * @param int $startIndex
         * @param int $maxRecords
         * @return \stdClass
         */
        public function getRecords($requestId, $startIndex=1, $maxRecords=10);
    }

    /**
     * Class BulkWhoisClient
     * @package whoisxmlapi\samples\bulkwhois
     */
    class BulkWhoisClient extends BaseClass implements ApiClientInterface
    {
        const HEADER =
            "Content-Type: application/json\r\nAccept: application/json\r\n";

        const OUTPUT = 'json';

        /**
         * @var HttpClientInterface
         */
        protected $_httpClient;

        /**
         * @var string
         */
        protected $_apiKey;

        /**
         * BulkWhoisClient constructor.
         * @param HttpClientInterface $httpClient
         * @param string $apiKey
         */
        public function __construct(
            HttpClientInterface $httpClient, $apiKey)
        {
            $this->_httpClient = $httpClient;
            $this->_apiKey = $apiKey;
        }

        /**
         * @param string[] $domains
         * @return \stdClass
         */
        public function createRequest(array $domains)
        {
            return
                json_decode($this->_callApi('/bulkWhois', compact('domains')));
        }

        /**
         * @param string $requestId
         * @return string
         */
        public function download($requestId)
        {
            return $this->_callApi('/download', compact('requestId'));
        }

        /**
         * @param string $requestId
         * @param int $startIndex
         * @param int $maxRecords
         * @return \stdClass
         */
        public function getRecords($requestId, $startIndex=1, $maxRecords=10)
        {
            $data = compact('requestId', 'startIndex', 'maxRecords');
            return json_decode($this->_callApi('/getRecords', $data));
        }

        /**
         * @param string $path
         * @param array $data
         * @return string
         */
        protected function _callApi($path, array $data)
        {
            $params = array(
                'apiKey' => $this->_apiKey,
                'outputFormat' => static::OUTPUT
            );

            $data = json_encode($params + $data);
            return $this->_httpClient->post($path, $data, static::HEADER);
        }
    }

    /**
     * Interface HttpClientInterface
     * @package whoisxmlapi\samples\bulkwhois
     */
    interface HttpClientInterface
    {
        const CLASS_NAME = __CLASS__;

        /**
         * @param string $path
         * @param string $content
         * @param string $header
         * @return string
         */
        public function get($path, $content, $header = '');

        /**
         * @param string $path
         * @param string $content
         * @param string $header
         * @return string
         */
        public function post($path, $content, $header = '');
    }

    /**
     * Class RemoteFileClient
     * @package whoisxmlapi\samples\bulkwhois
     */
    class RemoteFileClient extends BaseClass implements HttpClientInterface
    {
        /**
         * @var string
         */
        protected $_url;

        /**
         * RemoteFileClient constructor.
         * @param string $url
         */
        public function __construct($url)
        {
            $this->_url = $url;
        }

        /**
         * @param string $path
         * @param string $content
         * @param string $header
         * @return void
         * @throws \Exception
         */
        public function get($path, $content, $header='')
        {
            throw new \Exception('Not implemented');
        }

        /**
         * @param string $path
         * @param string $content
         * @param string $header
         * @return bool|string
         */
        public function post($path, $content, $header='')
        {
            $opts = array(
                'http' =>array('method'=>'POST')+compact('content','header'));

            $context = stream_context_create($opts);
            return file_get_contents($this->_url . $path, false, $context);
        }
    }
}

namespace
{
    use whoisxmlapi\samples\bulkwhois\AppBulkWhois;
    use whoisxmlapi\samples\bulkwhois\AppResolver;
    use whoisxmlapi\samples\bulkwhois\GetWhoisRecordsCsvExample;
    use whoisxmlapi\samples\bulkwhois\RemoteFileClient;
    use whoisxmlapi\samples\bulkwhois\ResolverInterface;

    $config = array(
        whoisxmlapi\samples\bulkwhois\ReflectionFactoryInterface::CLASS_NAME=>
            whoisxmlapi\samples\bulkwhois\ReflectionFactory::className(),

        ResolverInterface::CLASS_NAME => AppResolver::className(),

        whoisxmlapi\samples\bulkwhois\ApiClientInterface::CLASS_NAME => array(
            whoisxmlapi\samples\bulkwhois\BulkWhoisClient::className()=>array(
                'apiKey' => @apiKey
            )
        ),
        'example' => array(
            GetWhoisRecordsCsvExample::className() => array(
                'params' => array('domains' => explode(',', @DOMAINS))
            )
        ),
        whoisxmlapi\samples\bulkwhois\HttpClientInterface::CLASS_NAME =>array(
            RemoteFileClient::className() => array('url' => @URL)
        )
    );

    (new AppBulkWhois($config))->boot()->make('example')->run();
}
