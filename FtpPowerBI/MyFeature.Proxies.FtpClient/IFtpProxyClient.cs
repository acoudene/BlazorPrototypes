using MyFeature.Dtos;

namespace MyFeature.Proxies.Ftp;

public interface IFtpProxyClient
{
  Task ExportAsync(List<MyEntityDto> dtos, CancellationToken cancellationToken = default);
}
