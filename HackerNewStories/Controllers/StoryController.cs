using HackerNewsStories.Interfaces;
using HackerNewsStories.Models;
using HackerNewsStories.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HackerNewsStories.Controllers
{
    /// <summary>
    /// Used to handle operations on stories.
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class StoryController : ControllerBase
    {
        /// The logger.
        private readonly ILogger<StoryController> _logger;

        /// The story service.
        private readonly IStoryService _storyService;

        /// <summary>
        /// Initializes a new instance of the <see cref="StoryController"/> class.
        /// <param name="logger">The logger.</param>
        /// <param name="storyService">The story service.</param>
        /// </summary>
        public StoryController(ILogger<StoryController> logger, IStoryService storyService)
        {
            _logger = logger;
            _storyService = storyService;
        }

        /// <summary>
        /// Gets the new stories.
        /// </summary>
        /// <param name="limit">The limit.</param>
        /// <returns>The List of stories.</returns>
        [HttpGet("")]
        public async Task<ActionResult<IEnumerable<Story>>> GetStories(int limit = 200)
        {
            try
            {
                var result = await this._storyService.GetNewStoriesAsync(limit);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"GetStories Api failed due to: Message: {ex.Message} Stacktrace: {ex.StackTrace}");
                return StatusCode(StatusCodes.Status500InternalServerError,ex.Message);
            }
        }
    }
}
