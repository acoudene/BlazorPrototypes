var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.CameraCaptureApp>("cameracaptureapp");

builder.Build().Run();
