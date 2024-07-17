using HackerNewsStories.Interfaces;
using HackerNewsStories.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace HackerNewsStories.Services
{
    /// <summary>
    /// Used to handle operations on stories.
    /// </summary>
    /// <seealso cref="IStoryService" />
    public class StoryService: IStoryService
    {
        /// The http client.
        private readonly IHttpService _httpService;

        /// The instance for logging story service.
        private readonly ILogger<StoryService> _logger;

        /// The memory cache.
        private readonly IMemoryCache _cache;

        /// <summary>
        /// Initializes a new instance of the <see cref="StoryService"/> class.
        /// <param name="httpService">The http service.</param>
        /// <param name="logger">The logger.</param>
        /// <param name="cache">The memory cache.</param>
        /// </summary>
        public StoryService(IHttpService httpService, ILogger<StoryService> logger,
            IMemoryCache cache) {
            _cache = cache;
            _logger = logger;
            _httpService=httpService;
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<Story>> GetNewStoriesAsync(int limit)
        { 
            try
            {
                var cacheKey = $"newStories_{limit}";
                _logger.LogInformation($"Fetching data for key: {cacheKey} from cache.");
                if (!_cache.TryGetValue(cacheKey, out List<Story> stories))
                {
                        _logger.LogInformation($"The data for key: {cacheKey} not found.");
                        var storiesBag = new ConcurrentBag<Story>();
                        string url = $"https://hacker-news.firebaseio.com/v0/newstories.json?orderBy=%22$priority%22&limitToFirst={limit}";
                        var storyIds = await this._httpService.GetAsync<IEnumerable<int>>(url);
                        if (storyIds != null)
                        {
                            List<Task> tasks = new List<Task>();
                            var parallelLoop = Parallel.ForEach(storyIds, id =>
                            {
                                var itemUrl = $"https://hacker-news.firebaseio.com/v0/item/{id}.json?print=pretty";
                                tasks.Add(Task.Run(async () =>
                                {
                                    var story = await this._httpService.GetAsync<Story>(itemUrl);
                                    if (story.Url != null)
                                    {
                                        storiesBag.Add(story);
                                    }
                                }));
                            }
                            );
                            await Task.WhenAll(tasks);
                            this.setCache<List<Story>>(cacheKey, storiesBag.ToList());
                            return storiesBag;
                        }
                }
                else
                {
                    _logger.LogInformation($"Found the cache key: {cacheKey}");
                }
                return stories ?? new List<Story>();
            }
            catch (Exception ex)
            {
                _logger.LogError($"GetNewStoriesAsync service failed due to: Message: {ex.Message} Stacktrace: {ex.StackTrace}");
                throw;

            }        
        }

        /// <summary>
        /// Sets the data in memory cache.
        /// <param name="cacheKey">The cache key.</param>
        /// <param name="data">The data.</param>
        /// </summary>
        private void setCache<T>(string cacheKey, T data)
        {
            var cacheOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromMinutes(30))
                    .SetAbsoluteExpiration(TimeSpan.FromMinutes(60))
                    .SetPriority(CacheItemPriority.NeverRemove)
                    .SetSize(2048);
            _logger.LogInformation($"Setting data for key: {cacheKey} to cache.");
            _cache.Set(cacheKey, data, cacheOptions);
        }
    }
}
