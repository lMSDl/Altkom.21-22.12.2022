using DAL;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContextPool<Context>(x => x.UseSqlServer(), 512);

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.Run();
