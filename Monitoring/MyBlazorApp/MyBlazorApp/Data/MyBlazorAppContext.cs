using Microsoft.EntityFrameworkCore;

namespace MyBlazorApp.Data;

public class MyBlazorAppContext : DbContext
{
  public MyBlazorAppContext(DbContextOptions<MyBlazorAppContext> options)
      : base(options)
  {
  }

  public DbSet<MyBlazorApp.Client.Dtos.MyEntityDto> MyEntityDto { get; set; } = default!;
}
