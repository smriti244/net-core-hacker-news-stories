using HackerNewsStories.Interfaces;
using HackerNewsStories.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace HackerNewsStories.Services
{
    /// <summary>
    /// Used to call the external http apis.
    /// </summary>
    /// <seealso cref="IHttpSrevice" />
    public class HttpService : IHttpService
    {
        /// The instance for http client.
        private readonly HttpClient _client;

        /// The logger.
        private readonly ILogger<HttpService> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpService"/> class.
        /// <param name="clientFactory">The client factory.</param>
        /// <param name="logger">The logger.</param>
        /// </summary>
        public HttpService(IHttpClientFactory clientFactory, ILogger<HttpService> logger)
        {
            _client = clientFactory.CreateClient();
            _logger = logger;
        }

        /// <inheritdoc/>
        public async Task<T> GetAsync<T>(string url)
        {
            try
            {
                _logger.LogInformation($"Get Request for url: {url}");
                return await _client.GetFromJsonAsync<T>(url);
            }
            catch (Exception ex)
            {
                _logger.LogError($"GetFromJsonAsync Failed: url: {url} StackTrace: {ex.StackTrace} message:{ex.Message}");
                return default;
            }
        }
    }
}
