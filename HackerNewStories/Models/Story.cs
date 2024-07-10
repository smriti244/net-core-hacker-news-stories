namespace HackerNewsStories.Models
{
    /// <summary>
    /// Represents the story details.
    /// </summary>
    public class Story
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>The title of the story.</value>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the url.
        /// </summary>
        /// <value>The url of the story.</value>
        public string Url { get; set; }
    }
}
