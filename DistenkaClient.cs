using System.Net.Http.Headers;
using System.Text;

namespace dotnet_client
{
    public interface IDistenkaClient
    {
        /// <summary>
        /// Enqueues a new run with the <paramref name="data"/>.
        /// </summary>
        /// <param name="data">The data to be processed in processor-plugin for the new run.</param>
        /// <returns>A newly enqueued <see cref="Run"/>.</returns>
        Task<Run> EnqueueAsync(string data);

        /// <summary>
        /// Requeue a new run using the data from the run with ID <paramref name="runId"/>.
        /// </summary>
        /// <param name="runId">The ID of the <see cref="Run"/> whose config will be used.</param>
        /// <returns>A newly enqueued <see cref="Run"/>.</returns>
        Task<Run> RequeueAsync(string runId);

        /// <summary>
        /// Request the cancellation of the run with ID <paramref name="runId"/>.
        /// </summary>
        /// <param name="runId">The ID of the <see cref="Run"/> to cancel.</param>
        /// <returns>The <see cref="Run"/> being cancelled.</returns>
        Task<Run> CancelAsync(string runId);
    }

    /// <summary>
    /// Interface to the Run API, allowing clients to enqueue, requeue, and cancel runs.
    /// </summary>
    public class HttpRunClient : IDistenkaClient
    {
        private readonly HttpClient _httpClient;
        private readonly IAuthenticationProvider _tokenProvider;
        private readonly ClientSettings _settings;

        public HttpRunClient(HttpClient httpClient, IAuthenticationProvider tokenProvider, ClientSettings settings)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _tokenProvider = tokenProvider ?? throw new ArgumentNullException(nameof(tokenProvider));
            _settings = settings ?? throw new ArgumentNullException(nameof(settings));
        }

        public async Task<Run> EnqueueAsync(string data)
        {
            var url = BuildUrl("/runs/enqueue");
            var req = CreateRequest(HttpMethod.Post, url, data);
            return await SendRequestAsync(req);
        }

        public async Task<Run> RequeueAsync(string runId)
        {
            var url = BuildUrl($"/runs/requeue/{runId}");
            var req = CreateRequest(HttpMethod.Post, url);
            return await SendRequestAsync(req);
        }

        public async Task<Run> CancelAsync(string runId)
        {
            var url = BuildUrl($"/runs/cancel/{runId}");
            var req = CreateRequest(HttpMethod.Post, url);
            return await SendRequestAsync(req);
        }

        private string BuildUrl(string path)
        {
            var endpoint = string.Format(Constants.ApiEndpointTemplate, _settings.OrgName, _settings.Env);
            return $"{endpoint}{path}";
        }

        private HttpRequestMessage CreateRequest(HttpMethod method, string url, string content = null)
        {
            var req = new HttpRequestMessage(method, url);

            if (content != null)
            {
                req.Content = new StringContent(content, Encoding.UTF8, "application/json");
            }

            req.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _tokenProvider.AcquireToken().Result);

            if (!string.IsNullOrEmpty(_settings.Processor))
            {
                req.Headers.Add(Constants.ProcessorHeader, _settings.Processor);
            }

            return req;
        }

        private async Task<Run> SendRequestAsync(HttpRequestMessage request)
        {
            var response = await _httpClient.SendAsync(request);
            await response.EnsureSuccess();
            return await response.Content.ReadAsAsync<Run>();
        }
    }
}
