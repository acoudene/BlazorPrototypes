namespace MyBlazorTestProject
{
  /// <summary>
  /// These tests are written entirely in C#.
  /// Learn more at https://bunit.dev/docs/getting-started/writing-tests.html#creating-basic-tests-in-cs-files
  /// </summary>
  public class CounterCSharpTests : TestContext
  {
    [Fact]
    public void CounterStartsAtZero()
    {
      // Arrange
      var cut = RenderComponent<TestCounter>();

      // Assert that content of the paragraph shows counter at zero
      cut.Find("p").MarkupMatches("<p>Current count: 0</p>");
    }

    [Fact]
    public void ClickingButtonIncrementsCounter()
    {
      // Arrange
      var cut = RenderComponent<TestCounter>();

      // Act - click button to increment counter
      for (int i = 0; i < 10; i++)
      {
        cut.Find("button").Click();
      }

      // Assert that the counter was incremented
      cut.Find("p").MarkupMatches("<p>Current count: 10</p>");
    }
  }
}
