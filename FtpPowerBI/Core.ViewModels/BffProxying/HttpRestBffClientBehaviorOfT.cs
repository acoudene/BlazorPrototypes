// Changelogs Date  | Author                | Description
// 2023-12-23       | Anthony Coudène       | Creation

using Core.ViewObjects;
using Microsoft.AspNetCore.JsonPatch;
using System.Net.Http.Json;
using System.Text;

namespace Core.ViewModels.BffProxying;

public class HttpRestBffClientBehavior<TViewObject>
  where TViewObject : class, IIdentifierViewObject
{
    private readonly IHttpClientFactory _httpClientFactory;
    protected IHttpClientFactory HttpClientFactory { get => _httpClientFactory; }

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="httpClientFactory"></param>
    /// <exception cref="ArgumentNullException"></exception>
    public HttpRestBffClientBehavior(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
    }

    public virtual async Task<List<TViewObject>> GetAllAsync(string configurationName, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(configurationName))
            throw new InvalidOperationException("Missing configuration name");

        using HttpClient httpClient = _httpClientFactory.CreateClient(configurationName);
        var items = await httpClient.GetFromJsonAsync<List<TViewObject>>(string.Empty, cancellationToken);
        if (items is null)
            throw new InvalidOperationException($"Problem while getting resources from: [{httpClient.BaseAddress}]");

        return items;
    }

    public virtual async Task<TViewObject?> GetByIdAsync(Guid id, string configurationName, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(configurationName))
            throw new InvalidOperationException("Missing configuration name");

        using HttpClient httpClient = _httpClientFactory.CreateClient(configurationName);
        return await httpClient.GetFromJsonAsync<TViewObject>($"{id}", cancellationToken);
    }

    public virtual async Task<List<TViewObject>> GetByIdsAsync(List<Guid> ids, string configurationName, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(configurationName))
            throw new InvalidOperationException("Missing configuration name");

        using HttpClient httpClient = _httpClientFactory.CreateClient(configurationName);

        var builder = new StringBuilder(ids.Count);
        builder.AppendJoin("&", ids.Select(id => $"ids={id}"));

        var requestUri = $"byIds?{builder.ToString()}";

        var items = await httpClient.GetFromJsonAsync<List<TViewObject>>(requestUri, cancellationToken);
        if (items is null)
            throw new InvalidOperationException($"Problem while getting resources from: [{httpClient.BaseAddress}]");

        return items;
    }

    public virtual async Task<HttpResponseMessage> CreateAsync(
      TViewObject dto,
      string configurationName,
      bool checkSuccessStatusCode = true,
      CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(configurationName))
            throw new InvalidOperationException("Missing configuration name");

        using HttpClient httpClient = _httpClientFactory.CreateClient(configurationName);
        var response = await httpClient.PostAsJsonAsync(string.Empty, dto, cancellationToken);
        if (response is null)
            throw new InvalidOperationException($"Problem while creating resource from: [{httpClient.BaseAddress}]");

        if (checkSuccessStatusCode)
            response.EnsureSuccessStatusCode();
        return response;
    }

    public virtual async Task<HttpResponseMessage> CreateOrUpdateAsync(
      TViewObject dto,
      string configurationName,
      bool checkSuccessStatusCode = true,
      CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(configurationName))
            throw new InvalidOperationException("Missing configuration name");

        using HttpClient httpClient = _httpClientFactory.CreateClient(configurationName);
        var response = await httpClient.PostAsJsonAsync("CreateOrUpdate", dto, cancellationToken);
        if (response is null)
            throw new InvalidOperationException($"Problem while creating resource from: [{httpClient.BaseAddress}]");

        if (checkSuccessStatusCode)
            response.EnsureSuccessStatusCode();
        return response;
    }

    public virtual async Task<HttpResponseMessage> UpdateAsync(
      Guid id,
      TViewObject dto,
      string configurationName,
      bool checkSuccessStatusCode = true,
      CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(configurationName))
            throw new InvalidOperationException("Missing configuration name");

        using HttpClient httpClient = _httpClientFactory.CreateClient(configurationName);
        var response = await httpClient.PutAsJsonAsync($"{id}", dto, cancellationToken);
        if (response is null)
            throw new InvalidOperationException($"Problem while updating resource from: [{httpClient.BaseAddress}]");

        if (checkSuccessStatusCode)
            response.EnsureSuccessStatusCode();
        return response;
    }

    public virtual async Task<TViewObject?> DeleteAsync(
      Guid id,
      string configurationName,
      CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(configurationName))
            throw new InvalidOperationException("Missing configuration name");

        using HttpClient httpClient = _httpClientFactory.CreateClient(configurationName);
        return await httpClient.DeleteFromJsonAsync<TViewObject>($"{id}", cancellationToken);
    }

    public virtual async Task<HttpResponseMessage> PatchAsync(
      Guid id,
      JsonPatchDocument<TViewObject> patch,
      string configurationName,
      bool checkSuccessStatusCode = true,
      CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(configurationName))
            throw new InvalidOperationException("Missing configuration name");

        using HttpClient httpClient = _httpClientFactory.CreateClient(configurationName);
        var response = await httpClient.PatchAsJsonAsync($"{id}", patch, cancellationToken);
        if (response is null)
            throw new InvalidOperationException($"Problem while patching resource from: [{httpClient.BaseAddress}]");

        if (checkSuccessStatusCode)
            response.EnsureSuccessStatusCode();
        return response;
    }
}


