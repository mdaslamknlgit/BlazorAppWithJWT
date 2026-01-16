var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.BlazorAppWithJWT>("blazorappwithjwt");
builder.AddProject<Projects.MyCRM_Api>("mycrmapi");

//builder.AddProject<Projects.ConsoleApp1>("ConsoleApp");

builder.Build().Run();
