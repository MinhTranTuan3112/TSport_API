
using Serilog;
using TSport.Api.Extensions;
using TSport.Api.Middlewares;
using TSport.Api.Repositories.Extensions;
using TSport.Api.Services.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var configuration = builder.Configuration;
builder.Services.AddApiDependencies(configuration)
                .AddServicesDependencies()
                .AddRepositoriesDependencies();

//Add serilog
builder.Host.UseSerilog((ctx, lc) => lc.WriteTo.Console().ReadFrom.Configuration(ctx.Configuration));
Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", @"C:\Users\Toant\Downloads\fpt_stuff\SWD392\TSport\tsport-a98e7-firebase-adminsdk-zmnm7-85b610a946.json");

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseSwagger();   
app.UseSwaggerUI();

app.UseCors("AllowAll");

app.UseHttpsRedirection();

app.UseMiddleware<GlobalExceptionMiddleware>();

app.UseAuthorization();

app.MapControllers();

app.Run();
