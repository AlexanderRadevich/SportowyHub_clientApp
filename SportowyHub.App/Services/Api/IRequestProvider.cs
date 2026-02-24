namespace SportowyHub.Services.Api;

public interface IRequestProvider
{
    Task<TResult> GetAsync<TResult>(string uri, string token = "");
    Task<TResponse> PostAsync<TRequest, TResponse>(string uri, TRequest data, string token = "");
    Task<TResult> PutAsync<TResult>(string uri, TResult data, string token = "");
    Task DeleteAsync(string uri, string token = "");
}
