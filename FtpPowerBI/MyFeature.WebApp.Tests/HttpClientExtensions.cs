// Changelogs Date  | Author                | Description
// 2023-12-23       | Anthony Coudène       | Creation

using RichardSzalay.MockHttp;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;

namespace MyFeature.WebApp.Tests;

public static class HttpClientExtensions
{
  public static MockHttpMessageHandler AddMockHttpClient(this TestServiceProvider services, Uri uri)
  {
    var mockHttpHandler = new MockHttpMessageHandler();
    var httpClient = mockHttpHandler.ToHttpClient();
    httpClient.BaseAddress = uri;
    services.AddSingleton<HttpClient>(httpClient);
    return mockHttpHandler;
  }

  public static MockedRequest RespondJson<T>(this MockedRequest request, T content)
  {
    request.Respond(req =>
    {
      var response = new HttpResponseMessage(HttpStatusCode.OK);
      response.Content = new StringContent(JsonSerializer.Serialize(content));
      response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
      return response;
    });
    return request;
  }

  public static MockedRequest RespondJson<T>(this MockedRequest request, Func<T> contentProvider)
  {
    request.Respond(req =>
    {
      var response = new HttpResponseMessage(HttpStatusCode.OK);
      response.Content = new StringContent(JsonSerializer.Serialize(contentProvider()));
      response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
      return response;
    });
    return request;
  }
}
