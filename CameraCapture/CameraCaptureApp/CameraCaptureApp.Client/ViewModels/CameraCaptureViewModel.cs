namespace CameraCaptureApp.Client.ViewModels;

public class CameraCaptureViewModel
{
  private readonly HttpClient _httpClient;

  public CameraCaptureViewModel(HttpClient httpClient)
  {
    _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
  }

  public async Task<Uri?> StorePhotoAsync(string dataUri)
  {
    if (string.IsNullOrWhiteSpace(dataUri))
      return null;

    var response = await _httpClient.PostAsync("image", new StringContent(dataUri));
    response.EnsureSuccessStatusCode();

    var baseAddress = _httpClient.BaseAddress;
    if (baseAddress is null)
      throw new InvalidOperationException("Missing base address for API");

    Uri? relativeApiUri = response.Headers.Location;
    if (relativeApiUri is null)
      throw new InvalidOperationException("Missing relative Uri for API");


    return new Uri(baseAddress, relativeApiUri);
  }
}
