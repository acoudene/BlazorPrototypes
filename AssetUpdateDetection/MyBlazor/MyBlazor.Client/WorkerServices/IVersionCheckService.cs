namespace MyBlazor.Client.WorkerServices;

public interface IVersionCheckService
{
  ValueTask<VersionInfo?> GetLocalVersionAsync();
  Task<bool> IsNewVersionAvailableAsync();
}