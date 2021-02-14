using dotnetCore_SearchEngine.Models;
using dotnetCore_SearchEngine.Services.Logging;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace dotnetCore_SearchEngine.Services
{
    public class Service : IService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _config;
        private readonly EventLogger _eventLogger;

        public Service(IConfiguration config)
        {
            _httpClient = new HttpClient();
            _config = config;
            _eventLogger = EventLogger.Instance;
        }

        public async Task<IActionResult> ProcessData(SearchData search)
        {
            try
            {
                await _eventLogger.LogEvent($".ProcessData -> Search Engine: {search.SearchEngine}, Keyword: {search.SearchKeywords}, Target Url: {search.SearchTargetUrl}");

                // is valid engine
                string appKey = GetAppsettingKey(search.SearchEngine.ToLower());
                if (string.IsNullOrEmpty(appKey))
                {
                    SearchResult searchResult = new SearchResult();
                    searchResult.Result = "Search engine not configured.";

                    return new OkObjectResult(searchResult);
                }
                    

                Uri uri = BuildSearchUri(search.SearchEngine.ToLower(), search.SearchKeywords, appKey, _config[appKey + ":PageStart"]);
                string regxPattern = GetRegxForUrl(uri.Host, out int resultPerPage, appKey);
                Regex regx = new Regex(regxPattern);

                // Get and filter response.
                List<Match> urls = new List<Match>();
                int nextPage = int.Parse(_config[appKey + ":Results"]);
                int pagemultiplier = 0;
                int maxResults = int.Parse(_config["MaxResults"]);

                while (urls.Count <= maxResults)
                {
                    string[] unwantedTags = _config["UnwantedTags"].Split(',');
                    string response = await GetSearchResults(uri);

                    response = FilterUnwantedTags(response, unwantedTags);

                    MatchCollection matches = regx.Matches(response);
                    urls.AddRange(matches);

                    //Rebuild Uri
                    pagemultiplier++;
                    uri = BuildSearchUri(search.SearchEngine.ToLower(), search.SearchKeywords, appKey, (nextPage * pagemultiplier).ToString());
                }

                int[] urlIndx = urls.Take(maxResults).Where(a => a.Groups[2].Value == search.SearchTargetUrl).Select(a => urls.IndexOf(a)+1).ToArray();

                // return Object
                SearchResult result = new SearchResult();
                result.Result = BuildResponse(urlIndx);

                return new OkObjectResult(result);
            }
            catch (Exception)
            {
                return new BadRequestObjectResult(search);
            }

        }

        private string BuildResponse(int[] urlIndexArr)
        {
            if (urlIndexArr.Length > 0)
            {
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < urlIndexArr.Length; i++)
                {
                    string value = (i == (urlIndexArr.Length - 1)) ? $"{urlIndexArr[i]}" : $"{urlIndexArr[i]}, ";
                    sb.Append(value);
                }
                return sb.ToString();
            }

            return "0";
        }

        private string FilterUnwantedTags(string content, string[] unwantedTags)
        {
            if (unwantedTags.Length > 0)
            {
                foreach (string tag in unwantedTags)
                {
                    int indexStartTag = content.IndexOf($"<{tag}");
                    int indexEndTag = content.IndexOf($"/{tag}>");
                    ;
                    do
                    {
                        string topContent = content.Substring(0,indexStartTag);
                        string endContent = content.Substring(indexEndTag + (tag.Length + 2));
                        content = topContent + endContent;
                        indexStartTag = content.IndexOf($"<{tag}");
                        indexEndTag = content.IndexOf($"/{tag}>");
                    } while (indexStartTag != -1);

                }
            }
            return content;
        }

        private async Task<string> GetSearchResults(Uri searchUri)
        {
            //_httpClient.BaseAddress = searchUri;
            HttpResponseMessage response = await _httpClient.GetAsync(searchUri);
            string x = await response.Content.ReadAsStringAsync();
            
            return x ;
        }

        private Uri BuildSearchUri(string searchUrl, string searchParams, string appKey, string page)
        {
            //Start initial first page only -> scale to other pages
            string basePrefix = "https://";
            if (searchUrl.IndexOf("http") == -1)
                searchUrl = basePrefix + searchUrl;

            StringBuilder sb = new StringBuilder(searchUrl);
            sb.Append("/")
              .Append(_config[appKey + ":Path"])
              .Append("?")
              .Append(_config[appKey + ":QueryParam"])
              .Append("=").Append(searchParams).Append("&")
              .Append(_config[appKey + ":PageQuery"])
              .Append("=")
              .Append(page);

            return new Uri(sb.ToString());
        }

        private string GetRegxForUrl(string uriHost, out int resultCount, string parentKey)
        {
            resultCount = Convert.ToInt32(_config[parentKey + ":Results"]);
            return _config[parentKey + ":Regex"];
        }

        private string GetAppsettingKey(string searchUrl)
        {
            Regex regex = new Regex(@"(?:[w]{3}\.)([\d\w]+)\.");
            Match match = regex.Match(searchUrl);
            if (match.Success && !string.IsNullOrEmpty(_config[match.Groups[1].Value + ":Regex"]))
            {
                return match.Groups[1].Value;
            }
            return string.Empty;
        }
    }
}