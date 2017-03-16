using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Http;

namespace Tests
{
    public class WebServerTestFixture<TStartup> : IDisposable
    {
        private readonly TestServer _server;
        private readonly HttpClient _client;

        public WebServerTestFixture(Action<IServiceCollection> configureServices)
        {
            var builder = WebHostBuilderFactory.Create<TStartup>(configureServices);

            // Create the TestServer    
            _server = new TestServer(builder);

            // Setup the HttpClient
            _client = _server.CreateClient();
        }

        public HttpResponseMessage Get(string relativeUrl)
        {
            return _client.GetAsync(relativeUrl).Result;
        }

        public HttpResponseMessage Post<T>(string relativeUrl, T content)
        {
            return _client.PostAsJsonAsync(relativeUrl, content).Result;
        }

        public void Dispose()
        {
            _client.Dispose();
            _server.Dispose();
        }
    }
}
