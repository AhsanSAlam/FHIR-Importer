using System;
using System.Linq;
using System.Text;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;


namespace FHIRTestAppWin
{
    /// <summary>
    /// Helper class to call a protected API and process its result
    /// </summary>
    public class ProtectedApiCallHelper
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="httpClient">HttpClient used to call the protected API</param>
        public ProtectedApiCallHelper(HttpClient httpClient)
        {
            HttpClient = httpClient;
        }

        protected HttpClient HttpClient { get; private set; }


        /// <summary>
        /// Calls the protected Web API and processes the result
        /// </summary>
        /// <param name="webApiUrl">Url of the Web API to call (supposed to return Json)</param>
        /// <param name="accessToken">Access token used as a bearer security token to call the Web API</param>
        /// <param name="processResult">Callback used to process the result of the call to the Web API</param>
        public async Task GetDataFromAPIAsync(string webApiUrl, string accessToken, Action<JObject> processResult)
        {
            if (!string.IsNullOrEmpty(accessToken))
            {
                // Set Request Header with appropriate content type and authentication header
                var defaultRequetHeaders = HttpClient.DefaultRequestHeaders;
                if (defaultRequetHeaders.Accept == null || !defaultRequetHeaders.Accept.Any(m => m.MediaType == "application/json"))
                {
                    HttpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                }
                defaultRequetHeaders.Authorization = new AuthenticationHeaderValue("bearer", accessToken);
                // Issue API GET
                HttpResponseMessage response = await HttpClient.GetAsync(webApiUrl);
                if (response.IsSuccessStatusCode)
                {
                    // API successfully returned data
                    string json = await response.Content.ReadAsStringAsync();
                    JObject result = JsonConvert.DeserializeObject(json) as JObject;
                    processResult(result);
                }
                else
                {
                    // API returned an error
                    string content = await response.Content.ReadAsStringAsync();
                    JObject result = JsonConvert.DeserializeObject(content) as JObject;
                    processResult(result);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="webApiUrl"></param>
        /// <param name="accessToken"></param>
        /// <param name="resource"></param>
        /// <param name="processResult"></param>
        /// <returns></returns>
        public async Task SendFHIRResourceDataToAPI(string webApiUrl, string accessToken, FHIRResource resource, Action<JObject> processResult)
        {
            if (!string.IsNullOrEmpty(accessToken))
            {
                if (!String.IsNullOrEmpty(resource.Resource))
                {
                    Uri fhirServerUrl = new Uri(webApiUrl);
                    // Resource content
                    StringContent content = new StringContent(resource.Resource, Encoding.UTF8, "application/json");
                    // Build HTTP Request message for the API. If the data has ID then use PUT; otherwise 
                    // use POST
                    var message = string.IsNullOrEmpty(resource.ResourceID)
                        ? new HttpRequestMessage(HttpMethod.Post, new Uri(fhirServerUrl, $"/{resource.ResourceType}"))
                        : new HttpRequestMessage(HttpMethod.Put, new Uri(fhirServerUrl, $"/{resource.ResourceType}/{resource.ResourceID}"));
                    // Assing content
                    message.Content = content;
                    // Assign Authorization Header
                    message.Headers.Authorization = new AuthenticationHeaderValue("bearer", accessToken);
                    // Invoke API and get result
                    HttpResponseMessage response = await HttpClient.SendAsync(message);

                    if (response.IsSuccessStatusCode)
                    {
                        string json = await response.Content.ReadAsStringAsync();
                        JObject result = JsonConvert.DeserializeObject(json) as JObject;
                        processResult(result);
                    }
                    else
                    {
                        string errorContent = await response.Content.ReadAsStringAsync();
                        JObject result = JsonConvert.DeserializeObject(errorContent) as JObject;
                        processResult(result);
                    }
                }
            }
        }

         public async Task PostDataToAPIAsync(string webApiUrl, string accessToken, string content, Action<JObject> processResult)
        {
            if (!string.IsNullOrEmpty(accessToken))
            {
                // Set Request Header with appropriate 
                var defaultRequetHeaders = HttpClient.DefaultRequestHeaders;
                if (defaultRequetHeaders.Accept == null || !defaultRequetHeaders.Accept.Any(m => m.MediaType == "application/json"))
                {
                    HttpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                }
                defaultRequetHeaders.Authorization = new AuthenticationHeaderValue("bearer", accessToken);

                var jsonContent = new StringContent(content, UnicodeEncoding.UTF8, "application/json");
                //HttpContent httpContent = new HttpContent();

                HttpResponseMessage response = await HttpClient.PostAsync(webApiUrl, jsonContent);
                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync();
                    JObject result = JsonConvert.DeserializeObject(json) as JObject;
                    processResult(result);
                }
                else
                {
                    string errorContent = await response.Content.ReadAsStringAsync();
                    JObject result = JsonConvert.DeserializeObject(errorContent) as JObject;
                    processResult(result);
                }
            }
        }

    }
}
