using BlazorApp.Client.Pages;

namespace MyBlazorTestProject;

/// <summary>
/// These tests are written entirely in C#.
/// Learn more at https://bunit.dev/docs/getting-started/writing-tests.html#creating-basic-tests-in-cs-files
/// </summary>
public class HomeCSharpTests : TestContext
{
	[Fact]
	public void TestWelcomeBasic()
	{
		// Arrange
		var cut = RenderComponent<Home>();

		// Assert
		cut.Find("h1").MarkupMatches(@"<h1>Hello, world!</h1>"+ "\r\n\n\n\n\n");
	}

	[Fact]
	public void TestWelcomeFindComponent()
	{
		// Arrange
		var cut = RenderComponent<Home>();

		// Assert
		var foundComponent = cut.FindComponent<Weather>();
	}

}
