using BlazorApp.Client.Pages;
using RichardSzalay.MockHttp;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Collections.Generic;
using BlazorApp.Client;
using System;

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
    var mock = Services.AddMockHttpClient();
    mock.When("/getData").RespondJson(new List<UserInfo> { new UserInfo() { Email = @"anthony.coudene@gmail.com", UserId = "ACE" } });

    // Act
    var cut = RenderComponent<ComponentExpectedHttpClient>();
    
    // Assert
    cut.WaitForAssertion(() => cut.Find("li").MarkupMatches(@"<li>anthony.coudene@gmail.com</li>"),TimeSpan.FromSeconds(5));   
  }

  [Fact]
  public void TestBasicOtherWritting()
  {
    // Arrange
    var mock = Services.AddMockHttpClient();
    mock.When("/getData").RespondJson(new List<UserInfo> { new UserInfo() { Email = @"anthony.coudene@gmail.com", UserId = "ACE" } });

    // Act
    var cut = RenderComponent<ComponentExpectedHttpClient>();
    cut.WaitForState(() => cut.Find("ul").TextContent.Equals(@"anthony.coudene@gmail.com"), TimeSpan.FromSeconds(5));

    // Assert
    cut.Find("li").MarkupMatches(@"<li>anthony.coudene@gmail.com</li>");
  }
}
