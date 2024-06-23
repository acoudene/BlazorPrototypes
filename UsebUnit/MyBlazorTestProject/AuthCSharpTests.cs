using BlazorApp.Client.Pages;
using Bunit.TestDoubles;

namespace MyBlazorTestProject;

/// <summary>
/// These tests are written entirely in C#.
/// Learn more at https://bunit.dev/docs/getting-started/writing-tests.html#creating-basic-tests-in-cs-files
/// </summary>
public class AuthCSharpTests : TestContext
{
	[Fact]
	public void TestBasic()
	{
		// Arrange
		this.AddTestAuthorization();

		// Act
		var cut = RenderComponent<Auth>();

		// Assert
		cut.Find("h1").MarkupMatches(@"<h1>You are authenticated</h1>");
	}

	[Fact]
	public void TestAuthorized()
	{
		// Arrange
		var authContext = this.AddTestAuthorization();
		authContext.SetAuthorized("TEST USER", AuthorizationState.Authorized);

		// Act
		var cut = RenderComponent<Auth>();

		// Assert
		cut.Find("p").MarkupMatches(@"<p>Hello TEST USER!</p>");
	}

}
