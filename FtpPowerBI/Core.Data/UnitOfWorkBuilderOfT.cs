// Changelogs Date  | Author                | Description
// 2023-12-23       | Anthony Coudène       | Creation

using Microsoft.Extensions.DependencyInjection;

namespace Core.Data;

public class UnitOfWorkBuilder<TUnitOfWork> where TUnitOfWork : IUnitOfWork
{
  public IServiceCollection ServiceCollection => _serviceCollection;

  private readonly IServiceCollection _serviceCollection;

  public UnitOfWorkBuilder()
    : this(new ServiceCollection())
  { }

  public UnitOfWorkBuilder(IServiceCollection serviceCollection)
  {
    if (serviceCollection is null)
      throw new ArgumentNullException(nameof(serviceCollection));

    _serviceCollection = serviceCollection;
  }

  public TUnitOfWork Build()
  {
    return _serviceCollection
      .BuildServiceProvider()
      .GetRequiredService<TUnitOfWork>();
  }
}
