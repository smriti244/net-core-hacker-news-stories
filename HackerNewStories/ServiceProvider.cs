using HackerNewsStories.Interfaces;
using HackerNewsStories.Services;
using Microsoft.Extensions.DependencyInjection;

namespace HackerNewsStories
{
    public static class ServiceProvider
    {
        public static void AddServiceProvider(this IServiceCollection services)
        {
            services.AddTransient<IStoryService, StoryService>();
            services.AddScoped<IHttpService, HttpService>();
        }
    }
}
