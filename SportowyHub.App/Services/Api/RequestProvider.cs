using System.Net;
using System.Text.Json;
using SportowyHub.Services.Exceptions;

namespace SportowyHub.Services.Api;

public class RequestProvider(IHttpClientFactory httpClientFactory) : IRequestProvider
{
    public async Task<TResult> GetAsync<TResult>(string uri, string token = "", CancellationToken ct = default)
    {
        var client = httpClientFactory.CreateClient("Api");
        var request = new HttpRequestMessage(HttpMethod.Get, uri);
        SetAuthHeader(request, token);

        var response = await client.SendAsync(request, ct);
        await HandleResponse(response, ct);

        var content = await response.Content.ReadAsStringAsync(ct);
        return (TResult)JsonSerializer.Deserialize(content, typeof(TResult), SportowyHubJsonContext.Default)!;
    }

    public async Task<TResponse> PostAsync<TRequest, TResponse>(string uri, TRequest data, string token = "", Dictionary<string, string>? headers = null, CancellationToken ct = default)
    {
        var client = httpClientFactory.CreateClient("Api");
        var request = new HttpRequestMessage(HttpMethod.Post, uri);
        SetAuthHeader(request, token);

        if (headers != null)
        {
            foreach (var (key, value) in headers)
            {
                request.Headers.TryAddWithoutValidation(key, value);
            }
        }

        var json = JsonSerializer.Serialize(data, typeof(TRequest), SportowyHubJsonContext.Default);
        request.Content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

        var response = await client.SendAsync(request, ct);
        await HandleResponse(response, ct);

        var content = await response.Content.ReadAsStringAsync(ct);
        return (TResponse)JsonSerializer.Deserialize(content, typeof(TResponse), SportowyHubJsonContext.Default)!;
    }

    public async Task<TResult> PutAsync<TResult>(string uri, TResult data, string token = "", CancellationToken ct = default)
    {
        var client = httpClientFactory.CreateClient("Api");
        var request = new HttpRequestMessage(HttpMethod.Put, uri);
        SetAuthHeader(request, token);

        var json = JsonSerializer.Serialize(data, typeof(TResult), SportowyHubJsonContext.Default);
        request.Content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

        var response = await client.SendAsync(request, ct);
        await HandleResponse(response, ct);

        var content = await response.Content.ReadAsStringAsync(ct);
        return (TResult)JsonSerializer.Deserialize(content, typeof(TResult), SportowyHubJsonContext.Default)!;
    }

    public async Task<TResponse> PutAsync<TRequest, TResponse>(string uri, TRequest data, string token = "", CancellationToken ct = default)
    {
        var client = httpClientFactory.CreateClient("Api");
        var request = new HttpRequestMessage(HttpMethod.Put, uri);
        SetAuthHeader(request, token);

        var json = JsonSerializer.Serialize(data, typeof(TRequest), SportowyHubJsonContext.Default);
        request.Content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

        var response = await client.SendAsync(request, ct);
        await HandleResponse(response, ct);

        var content = await response.Content.ReadAsStringAsync(ct);
        return (TResponse)JsonSerializer.Deserialize(content, typeof(TResponse), SportowyHubJsonContext.Default)!;
    }

    public async Task DeleteAsync(string uri, string token = "", CancellationToken ct = default)
    {
        var client = httpClientFactory.CreateClient("Api");
        var request = new HttpRequestMessage(HttpMethod.Delete, uri);
        SetAuthHeader(request, token);

        var response = await client.SendAsync(request, ct);
        await HandleResponse(response, ct);
    }

    private static void SetAuthHeader(HttpRequestMessage request, string token)
    {
        if (!string.IsNullOrEmpty(token))
        {
            request.Headers.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        }
    }

    private static async Task HandleResponse(HttpResponseMessage response, CancellationToken ct)
    {
        if (response.IsSuccessStatusCode)
        {
            return;
        }

        var content = await response.Content.ReadAsStringAsync(ct);

        if (response.StatusCode is HttpStatusCode.Unauthorized or HttpStatusCode.Forbidden)
        {
            throw new ServiceAuthenticationException(content, response.StatusCode);
        }

        throw new HttpRequestException(content, null, response.StatusCode);
    }
}
