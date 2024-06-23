using BlazorApp.Client.Pages;
using RichardSzalay.MockHttp;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Collections.Generic;

namespace MyBlazorTestProject;

/// <summary>
/// These tests are written entirely in C#.
/// Learn more at https://bunit.dev/docs/getting-started/writing-tests.html#creating-basic-tests-in-cs-files
/// </summary>
public class HttpClientCSharpTests : TestContext
{
  [Fact]
  public void TestBasic()
  {
    // Arrange
    // TODO
    //var mock = Services.AddMockHttpClient();
    //mock.When("/getData").RespondJson(new List<Data> { ... });

    // Act
    //var cut = RenderComponent<Auth>();

    // Assert
    //cut.Find("h1").MarkupMatches(@"<h1>You are authenticated</h1>");
  } 
}
