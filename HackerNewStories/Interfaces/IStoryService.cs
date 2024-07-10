using HackerNewsStories.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HackerNewsStories.Interfaces
{
    /// <summary>
    /// Used to handle operations on story.
    /// </summary>
    public interface IStoryService
    {
        /// <summary>
        /// Gets the new stories details.
        /// </summary>
        /// <param name="limit">The limit.</param>
        /// <returns>The list of stories.</returns>
        Task<IEnumerable<Story>> GetNewStoriesAsync(int limit);
        }
}
