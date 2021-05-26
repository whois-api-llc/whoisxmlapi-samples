package bulkwhois;

import java.util.Collections;

import com.fasterxml.jackson.annotation.JsonIgnoreProperties;
import com.fasterxml.jackson.annotation.JsonUnwrapped;

import org.slf4j.Logger;
import org.slf4j.LoggerFactory;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.beans.factory.annotation.Qualifier;
import org.springframework.beans.factory.annotation.Value;
import org.springframework.boot.CommandLineRunner;
import org.springframework.boot.SpringApplication;
import org.springframework.boot.autoconfigure.SpringBootApplication;
import org.springframework.boot.web.client.RestTemplateBuilder;
import org.springframework.context.ApplicationContext;
import org.springframework.context.annotation.Bean;
import org.springframework.context.annotation.Configuration;
import org.springframework.http.*;
import org.springframework.stereotype.Component;
import org.springframework.web.client.RestOperations;

@Configuration
class AppConfig
{
    private static final String apiKey = "Your Bulk Whois Api key";
    private static final String format = "json";

    private static final String url =
        "https://www.whoisxmlapi.com/BulkWhoisLookup/bulkServices";

    private static final String propPrefix = "com.whoisxmlapi.bulkwhois";

    public AppConfig()
    {
        System.setProperty(propPrefix + ".apiKey", apiKey;
        System.setProperty(propPrefix + ".format", format);
        System.setProperty(propPrefix + ".url", url);
    }

    @Bean
    public HttpHeaders httpHeaders()
    {
        return new HttpHeaders();
    }

    @Bean
    public RestOperations restOperations(RestTemplateBuilder restBuilder)
    {
        return restBuilder.build();
    }
}

@SpringBootApplication
public class Application
{
    private String[] domains = {
        "google.com",
        "whoisxmlapi.com"
    };

    private static final Logger log =
            LoggerFactory.getLogger(Application.class);

    public static void main(String[] args)
    {
        SpringApplication.run(Application.class, args);
    }

    @Bean
    public CommandLineRunner commandLineRunner(ApplicationContext ctx)
            throws NullPointerException
    {
        return args ->
        {
            ApiClientInterface apiClient =
                ctx.getBean(ApiClientInterface.class);

            log.info("Creating a bulk processing request...");

            RequestResponse response = apiClient.createRequest(domains);
            if (! response.getMessageCode().equals(200)) {
                log.info("    error: " + response.getMessage());
                return;
            }

            String id = response.getRequestId();
            log.info("    got request ID: " + id);

            log.info("Waiting for processing to finish...");

            Integer recordsLeft;
            while (true) {
                recordsLeft = apiClient.getRecords(id, 5, 0).getRecordsLeft();
                log.info("    records left: " + recordsLeft.toString());
                if (recordsLeft.equals(0))
                    break;
                Thread.sleep(5000);
            }

            log.info("Downloading CSV results");

            String csv = apiClient.download(id);
            log.info(csv);
        };
    }
}

@JsonIgnoreProperties(ignoreUnknown = true)
@Component
@Qualifier("bulkWhoisApiClient")
class CommonParams
{
    private String outputFormat;
    private String apiKey;

    public static CommonParams create(
        String apiKey,
        String outputFormat
    )
    {
        return new CommonParams(apiKey, outputFormat);
    }

    public CommonParams()
    {
    }

    @Autowired
    public CommonParams(
        @Value("${com.whoisxmlapi.bulkwhois.apiKey}") String apiKey,
        @Value("${com.whoisxmlapi.bulkwhois.format}") String outputFormat
    )
    {
        this.setApiKey(apiKey)
            .setOutputFormat(outputFormat);
    }

    public String getOutputFormat()
    {
        return outputFormat;
    }

    public String getApiKey()
    {
        return apiKey;
    }

    public CommonParams setOutputFormat(String outputFormat)
    {
        this.outputFormat = outputFormat;
        return this;
    }

    public CommonParams setApiKey(String apiKey)
    {
        this.apiKey = apiKey;
        return this;
    }
}

@JsonIgnoreProperties(ignoreUnknown = true)
class DownloadParams
{
    @JsonUnwrapped
    private CommonParams commonParams;

    private String requestId;

    public static DownloadParams create(
        CommonParams commonParams,
        String requestId
    )
    {
        return new DownloadParams().setCommonParams(commonParams)
                                   .setRequestId(requestId);
    }

    public CommonParams getCommonParams()
    {
        return commonParams;
    }

    public String getRequestId()
    {
        return requestId;
    }

    public DownloadParams setCommonParams(CommonParams commonParams) {
        this.commonParams = commonParams;
        return this;
    }

    public DownloadParams setRequestId(String requestId)
    {
        this.requestId = requestId;
        return this;
    }
}

@JsonIgnoreProperties(ignoreUnknown = true)
class GetRecordsParams
{
    @JsonUnwrapped
    private DownloadParams downloadParams;

    private Integer maxRecords;
    private Integer startIndex;

    public static GetRecordsParams create(
        DownloadParams downloadParams,
        Integer maxRecords,
        Integer startIndex
    )
    {
        return new GetRecordsParams().setDownloadParams(downloadParams)
                                     .setMaxRecords(maxRecords)
                                     .setStartIndex(startIndex);
    }

    public DownloadParams getDownloadParams()
    {
        return this.downloadParams;
    }

    public Integer getMaxRecords()
    {
        return maxRecords;
    }

    public Integer getStartIndex()
    {
        return startIndex;
    }

    public GetRecordsParams setDownloadParams(DownloadParams downloadParams)
    {
        this.downloadParams = downloadParams;
        return this;
    }

    public GetRecordsParams setMaxRecords(Integer maxRecords)
    {
        this.maxRecords = maxRecords;
        return this;
    }

    public GetRecordsParams setStartIndex(Integer startIndex)
    {
        this.startIndex = startIndex;
        return this;
    }
}

@JsonIgnoreProperties(ignoreUnknown = true)
class RequestParams
{
    @JsonUnwrapped
    private CommonParams commonParams;

    private String[] domains;

    public static RequestParams create(
        CommonParams commonParams,
        String[] domains)
    {
        return new RequestParams().setCommonParams(commonParams)
                                  .setDomains(domains);
    }

    public CommonParams getCommonParams()
    {
        return this.commonParams;
    }

    public String[] getDomains()
    {
        return domains;
    }

    public RequestParams setCommonParams(CommonParams commonParams)
    {
        this.commonParams = commonParams;
        return this;
    }

    public RequestParams setDomains(String[] domains)
    {
        this.domains = domains;
        return this;
    }
}

@JsonIgnoreProperties(ignoreUnknown = true)
class RequestResponse
{
    private String message;

    private Integer messageCode;

    private Integer recordsLeft;

    private String requestId;

    public String getMessage()
    {
        return this.message;
    }

    public Integer getMessageCode()
    {
        return this.messageCode;
    }

    public Integer getRecordsLeft()
    {
        return recordsLeft;
    }

    public String getRequestId()
    {
        return requestId;
    }

    public void setMessage(String message)
    {
        this.message = message;
    }

    public void setMessageCode(Integer messageCode)
    {
        this.messageCode = messageCode;
    }

    public void setRecordsLeft(Integer recordsLeft)
    {
        this.recordsLeft = recordsLeft;
    }

    public void setRequestId(String requestId)
    {
        this.requestId = requestId;
    }
}

interface ApiClientInterface
{
    RequestResponse createRequest(String[] domains);

    String download(String id);

    RequestResponse getRecords(
        String id,
        Integer startIndex,
        Integer maxRecords);
}

interface HttpEntityFactoryInterface
{
    HttpEntity create(Object params, HttpHeaders headers);
}

interface RestClientInterface
{
    <T> ResponseEntity<T> callApi(String path, Object params, Class<T> cls);
}

@Component
class BulkWhoisApiClient implements ApiClientInterface
{
    private CommonParams params;

    private RestClientInterface restClient;

    @Autowired
    public BulkWhoisApiClient(
        RestClientInterface restClient,
        CommonParams params
    )
    {
        this.restClient = restClient;
        this.params = params;
    }

    public RequestResponse createRequest(String[] domains)
    {
        RequestParams reqParams = RequestParams.create(params, domains);

        ResponseEntity<RequestResponse> response =
            restClient.callApi("/bulkWhois", reqParams,RequestResponse.class);

        return response.getBody();
    }

    public String download(String id)
    {
        DownloadParams dlParams = DownloadParams.create(params, id);

        ResponseEntity<String> response =
            restClient.callApi("/download", dlParams, String.class);

        return response.getBody();
    }

    public RequestResponse getRecords(
        String id,
        Integer startIndex,
        Integer maxRecords)
    {
        DownloadParams dlParams = DownloadParams.create(params, id);

        GetRecordsParams reqParams =
            GetRecordsParams.create(dlParams, maxRecords, startIndex);

        ResponseEntity<RequestResponse> response =
            restClient.callApi("/getRecords",reqParams,RequestResponse.class);

        return response.getBody();
    }
}

@Component
class HttpEntityFactory implements HttpEntityFactoryInterface
{
    public HttpEntity create(Object params, HttpHeaders headers)
    {
        return new HttpEntity<>(params, headers);
    }
}

@Component
@Qualifier("bulkWhoisApiClient")
class PostBodyRestClient implements RestClientInterface
{
    private HttpEntityFactoryInterface entityFactory;

    private HttpHeaders headers;

    private RestOperations restOperations;

    private String url;

    @Autowired
    public PostBodyRestClient(
        RestOperations restOperations,
        HttpEntityFactoryInterface entityFactory,
        HttpHeaders headers,
        @Value("${com.whoisxmlapi.bulkwhois.url}") String url
    )
    {
        this.restOperations = restOperations;
        this.entityFactory = entityFactory;
        this.headers = headers;
        this.url = url;
    }

    public <T> ResponseEntity<T> callApi(
        String path,
        Object params,
        Class<T> cls
    )
    {
        HttpEntity body = entityFactory.create(params, getHeaders());

        return restOperations.exchange(url+path, HttpMethod.POST, body, cls);
    }

    private HttpHeaders getHeaders()
    {
        this.headers.clear();
        headers.setContentType(MediaType.APPLICATION_JSON);
        headers.setAccept(Collections.singletonList(
                                            MediaType.APPLICATION_JSON));

        return this.headers;
    }
}
