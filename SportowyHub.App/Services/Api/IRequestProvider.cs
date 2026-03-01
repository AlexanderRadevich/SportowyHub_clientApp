namespace SportowyHub.Services.Api;

public interface IRequestProvider
{
    Task<TResult> GetAsync<TResult>(string uri, string token = "", CancellationToken ct = default);
    Task<TResponse> PostAsync<TRequest, TResponse>(string uri, TRequest data, string token = "", Dictionary<string, string>? headers = null, CancellationToken ct = default);
    Task<TResult> PutAsync<TResult>(string uri, TResult data, string token = "", CancellationToken ct = default);
    Task<TResponse> PutAsync<TRequest, TResponse>(string uri, TRequest data, string token = "", CancellationToken ct = default);
    Task DeleteAsync(string uri, string token = "", CancellationToken ct = default);
}
