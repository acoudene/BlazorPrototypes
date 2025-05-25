// Changelogs Date  | Author                | Description
// 2023-12-23       | Anthony Coudène       | Creation

using Core.Host.Testing;
using Microsoft.AspNetCore.JsonPatch;

namespace MyFeature.Host.Tests;

/// WARNING - for the moment, I don't have found a solution to reset settings like connexion string on a static test server
/// So be careful when changing settings, the same first settings will remain for server for all tests in this class even if this container is reset.
/// For example, don't change default port to reuse the same.
public class GivenMyEntityApi : HostApiMongoTestBase<Program>
{
  public GivenMyEntityApi(
    WebApplicationFactory<Program> webApplicationFactory,
    ITestOutputHelper output)
    : base("myEntity", webApplicationFactory, output)
  {
  }

  private const string ApiPath = "/api";
  private const string ApiRelativePath = $"{ApiPath}/MyEntity/"; // Warning, this ending slash is important in HttpClientFactory... :(

  [Theory]
  [ClassData(typeof(MyEntityData))]
  public async Task WhenCreatingItem_ThenSingleItemIsCreated_Async(MyEntityDto item)
  {
    // Arrange
    var logger = CreateLogger<HttpMyEntityClient>();
    var httpClientFactory = CreateHttpClientFactory(ApiRelativePath);
    var client = new HttpMyEntityClient(logger, httpClientFactory);

    // Act
    await client.CreateAsync(item);

    // Assert      
    var foundItem = await client.GetByIdAsync(item.Id);
    Assert.NotNull(foundItem);
  }

  [Theory]
  [ClassData(typeof(MyEntityData))]
  public async Task WhenCreatingOrUpdatingItem_ThenSingleItemIsCreatedOrUpdated_Async(MyEntityDto item)
  {
    // Arrange
    var logger = CreateLogger<HttpMyEntityClient>();
    var httpClientFactory = CreateHttpClientFactory(ApiRelativePath);
    var client = new HttpMyEntityClient(logger, httpClientFactory);

    // Act
    await client.CreateOrUpdateAsync(item);

    // Assert      
    var foundItem = await client.GetByIdAsync(item.Id);
    Assert.NotNull(foundItem);
  }

  [Theory]
  [ClassData(typeof(MyEntitysData))]
  public async Task WhenCreatingItems_ThenAllItemsAreGot_Async(List<MyEntityDto> items)
  {
    // Arrange
    var logger = CreateLogger<HttpMyEntityClient>();
    var httpClientFactory = CreateHttpClientFactory(ApiRelativePath);
    var client = new HttpMyEntityClient(logger, httpClientFactory);
    foreach (var item in items)
      await WhenCreatingItem_ThenSingleItemIsCreated_Async(item);
    var ids = items.Select(item => item.Id).ToList();
    int expectedCount = items.Count;

    // Act
    var gotItems = (await client.GetByIdsAsync(ids));

    // Assert
    Assert.True(expectedCount == gotItems.Count);
    Assert.Equivalent(items.Select(item => item.Id), gotItems.Select(item => item.Id));
  }

  [Theory]
  [ClassData(typeof(MyEntitysData))]
  public async Task WhenCreatingOrUpdatingItems_ThenAllItemsAreGot_Async(List<MyEntityDto> items)
  {
    // Arrange
    var logger = CreateLogger<HttpMyEntityClient>();
    var httpClientFactory = CreateHttpClientFactory(ApiRelativePath);
    var client = new HttpMyEntityClient(logger, httpClientFactory);
    foreach (var item in items)
      await WhenCreatingOrUpdatingItem_ThenSingleItemIsCreatedOrUpdated_Async(item);
    var ids = items.Select(item => item.Id).ToList();
    int expectedCount = items.Count;

    // Act
    foreach (var item in items)
      await WhenCreatingOrUpdatingItem_ThenSingleItemIsCreatedOrUpdated_Async(item);
    var gotItems = (await client.GetByIdsAsync(ids));

    // Assert
    Assert.True(expectedCount == gotItems.Count);
    Assert.Equivalent(items.Select(item => item.Id), gotItems.Select(item => item.Id));
  }

  [Theory]
  [ClassData(typeof(MyEntitysData))]
  public async Task WhenDeletingItems_ThenItemsAreDeleted_Async(List<MyEntityDto> items)
  {
    // Arrange
    var logger = CreateLogger<HttpMyEntityClient>();
    var httpClientFactory = CreateHttpClientFactory(ApiRelativePath);
    var client = new HttpMyEntityClient(logger, httpClientFactory);
    foreach (var item in items)
      await WhenCreatingItem_ThenSingleItemIsCreated_Async(item);
    var ids = items.Select(item => item.Id).ToList();

    // Act
    foreach (Guid id in ids)
      await client.DeleteAsync(id);

    var gotItems = (await client.GetByIdsAsync(ids));

    // Assert    
    Assert.Empty(gotItems);
  }

  [Theory]
  [ClassData(typeof(MyEntityData))]
  public async Task WhenPatchingItem_ThenItemIsPatched_Async(MyEntityDto item)
  {
    // Arrange
    var logger = CreateLogger<HttpMyEntityClient>();
    var httpClientFactory = CreateHttpClientFactory(ApiRelativePath, new TestWebApplicationFactoryClientOptions(logger));
    var client = new HttpMyEntityClient(logger, httpClientFactory);
    await client.CreateOrUpdateAsync(item); // Just to setup with an existing item
    Guid id = item.Id;
    string metadata = Guid.NewGuid().ToString();

    // Act
    var patch = new JsonPatchDocument<MyEntityDto>();
    patch.Replace(dto => dto.Metadata, metadata);
    await client.PatchAsync(id, patch);

    // Assert
    var foundItem = await client.GetByIdAsync(id);
    Assert.NotNull(foundItem);
    Assert.Equal(metadata, foundItem.Metadata);
  }
}
