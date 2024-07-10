using Castle.Core.Logging;
using HackerNewsStories.Controllers;
using HackerNewsStories.Interfaces;
using HackerNewsStories.Models;
using HackerNewsStories.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.Protected;
using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Security.Policy;
using System.Threading;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

namespace Test
{
    [TestFixture]
    public class StoriesTest
    {
        private IStoryService _mockService;
        private StoryController _controller;

        [SetUp]
        public async Task SetUp()
        {
            var itemList = new List<int>();
            itemList.Add(11);

            var mockHttpService = new Mock<IHttpService>();
            string itemUrl = "https://hacker-news.firebaseio.com/v0/newstories.json?orderBy=%22$priority%22&limitToFirst=1";

            mockHttpService.Setup(x => x.GetAsync<List<int>>(itemUrl)).ReturnsAsync(itemList);

            var mockData = new Story()
            {
                Id = 11,
                Title = "Mock News",
                Url = "http://www.getdropbox.com/u/2/screencast.html"
            };

            string url = "https://hacker-news.firebaseio.com/v0/item/11.json?print=pretty";

            mockHttpService.Setup(x => x.GetAsync<Story>(url)).ReturnsAsync(mockData);

            _mockService = new StoryService(mockHttpService.Object, (new Mock<ILogger<StoryService>>()).Object, (new Mock<IMemoryCache>()).Object);
            _controller = new StoryController((new Mock<ILogger<StoryController>>()).Object, _mockService);

        }

        [Test]
        public async Task GetStories_ReturnActionResult_WithListOfStories()
        {

            // Act
            var result = await _controller.GetStories(1);

            // Assert
            Assert.That(result.Value.Count(), Is.EqualTo(1));
        }

        [Test]
        public async Task GetStories_ReturnActionResult_With250Limit()
        {
            // Act
            var result = await _controller.GetStories();

            // Assert
            Assert.That(result.Value.Count(), Is.EqualTo(250));
        }
    }
}