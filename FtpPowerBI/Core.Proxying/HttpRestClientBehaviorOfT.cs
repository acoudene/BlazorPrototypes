// Changelogs Date  | Author                | Description
// 2023-12-23       | Anthony Coudène       | Creation

using Core.Dtos;
using Microsoft.AspNetCore.JsonPatch;
using Newtonsoft.Json;
using System.Net.Http.Json;
using System.Net.Mime;
using System.Text;

namespace Core.Proxying;

public class HttpRestClientBehavior<TDto>
  where TDto : class, IIdentifierDto
{
  private readonly IHttpClientFactory _httpClientFactory;
  protected IHttpClientFactory HttpClientFactory { get => _httpClientFactory; }

  /// <summary>
  /// Constructor
  /// </summary>
  /// <param name="httpClientFactory"></param>
  /// <exception cref="ArgumentNullException"></exception>
  public HttpRestClientBehavior(IHttpClientFactory httpClientFactory)
  {
    _httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
  }

  public virtual async Task<List<TDto>> GetAllAsync(string configurationName, CancellationToken cancellationToken = default)
  {
    if (string.IsNullOrWhiteSpace(configurationName))
      throw new InvalidOperationException("Missing configuration name");

    using HttpClient httpClient = _httpClientFactory.CreateClient(configurationName);
    var items = await httpClient.GetFromJsonAsync<List<TDto>>(string.Empty, cancellationToken);
    if (items is null)
      throw new InvalidOperationException($"Problem while getting resources from: [{httpClient.BaseAddress}]");

    return items;
  }

  public virtual async Task<TDto?> GetByIdAsync(Guid id, string configurationName, CancellationToken cancellationToken = default)
  {
    if (string.IsNullOrWhiteSpace(configurationName))
      throw new InvalidOperationException("Missing configuration name");

    using HttpClient httpClient = _httpClientFactory.CreateClient(configurationName);
    return await httpClient.GetFromJsonAsync<TDto>($"{id}", cancellationToken);
  }

  public virtual async Task<List<TDto>> GetByIdsAsync(List<Guid> ids, string configurationName, CancellationToken cancellationToken = default)
  {
    if (string.IsNullOrWhiteSpace(configurationName))
      throw new InvalidOperationException("Missing configuration name");

    using HttpClient httpClient = _httpClientFactory.CreateClient(configurationName);

    StringBuilder builder = new StringBuilder(ids.Count);
    builder.AppendJoin("&", ids.Select(id => $"ids={id}"));

    string requestUri = $"byIds?{builder.ToString()}";

    var items = await httpClient.GetFromJsonAsync<List<TDto>>(requestUri, cancellationToken);
    if (items is null)
      throw new InvalidOperationException($"Problem while getting resources from: [{httpClient.BaseAddress}]");

    return items;
  }

  public virtual async Task<HttpResponseMessage> CreateAsync(
    TDto dto,
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
    TDto dto,
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
    TDto dto,
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

  public virtual async Task<TDto?> DeleteAsync(
    Guid id,
    string configurationName,
    CancellationToken cancellationToken = default)
  {
    if (string.IsNullOrWhiteSpace(configurationName))
      throw new InvalidOperationException("Missing configuration name");

    using HttpClient httpClient = _httpClientFactory.CreateClient(configurationName);
    return await httpClient.DeleteFromJsonAsync<TDto>($"{id}", cancellationToken);
  }

  public virtual async Task<HttpResponseMessage> PatchAsync(
    Guid id,
    JsonPatchDocument<TDto> patch,
    string configurationName,
    bool checkSuccessStatusCode = true,
    CancellationToken cancellationToken = default)
  {
    if (string.IsNullOrWhiteSpace(configurationName))
      throw new InvalidOperationException("Missing configuration name");

    string json = JsonConvert.SerializeObject(patch);

    if (string.IsNullOrWhiteSpace(json))
      throw new InvalidOperationException("Json serialization problem");

    using HttpClient httpClient = _httpClientFactory.CreateClient(configurationName);
    
    var request = new HttpRequestMessage(HttpMethod.Patch, $"{id}")
    {
      Content = new StringContent(json, Encoding.UTF8, MediaTypeNames.Application.JsonPatch)
    };

    var response = await httpClient.SendAsync(request, cancellationToken);

    // In the future, we should use exclusively System.Text.Json but today works exclusively with Newtonsoft.Json:
    //var response = await httpClient.PatchAsJsonAsync($"{id}", patch, cancellationToken);

    if (response is null)
      throw new InvalidOperationException($"Problem while patching resource from: [{httpClient.BaseAddress}]");

    if (checkSuccessStatusCode)
      response.EnsureSuccessStatusCode();
    return response;
  }
}


