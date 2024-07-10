using HackerNewsStories.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HackerNewsStories.Interfaces
{
    /// <summary>
    /// Used to handle external http calls.
    /// </summary>
    public interface IHttpService
    {
        /// <summary>
        /// Gets the result from external api.
        /// </summary>
        /// <param name="url">The url.</param>
        /// <returns>The result.</returns>
        Task<T> GetAsync<T>(string url);
    }
}
