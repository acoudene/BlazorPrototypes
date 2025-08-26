using CameraCaptureApp.Models;
using Microsoft.EntityFrameworkCore;

namespace CameraCaptureApp.Persistance.Context;

public class PhotoCaptureDbContext : DbContext
{
  public PhotoCaptureDbContext(DbContextOptions<PhotoCaptureDbContext> options)
    : base(options)
  {
      
  }

  public DbSet<PhotoCapture> PhotoCaptures => Set<PhotoCapture>();
}
