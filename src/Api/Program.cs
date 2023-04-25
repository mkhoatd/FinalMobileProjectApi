using Api.Extensions;
using Api.Service.AdminServices;
using Api.Service.AuthService;
using Api.Service.ClassroomServices;
using Api.Service.UserService;

using Data;
using Data.Seed;

using FastEndpoints.Security;
using FastEndpoints.Swagger;

using JorgeSerrano.Json;

using Microsoft.AspNetCore.HttpLogging;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;

var connectionString = config.GetConnectionString("TutorDb");
var csVar = Environment.GetEnvironmentVariable("CONNECTION_STRING");
if (!String.IsNullOrEmpty(csVar))
{
    connectionString = csVar;
}

if (string.IsNullOrWhiteSpace(connectionString))
{
    throw new ArgumentNullException(nameof(connectionString));
}
else
{
    builder.Services.AddDbContext<TutorDbContext>(options =>
    {
        options.UseNpgsql(connectionString);
        options.EnableSensitiveDataLogging();
        options.EnableDetailedErrors();
    });
}

builder.WebHost.ConfigureKestrel(options =>
{
    options.AddServerHeader = false;
    options.AllowSynchronousIO = false;
});


builder.Host.UseConsoleLifetime(options => options.SuppressStatusMessages = true);


builder.Services.AddFastEndpoints(options =>
{
    options.SourceGeneratorDiscoveredTypes = new Type[] { };
});
builder.Services.AddJWTBearerAuth(config["Token:Key"]!);
builder.Services.AddHttpLogging(logging =>
{
    logging.LoggingFields = HttpLoggingFields.All;
    logging.RequestHeaders.Add("Authorization");
    logging.RequestHeaders.Add("WWW-Authenticate");
    logging.RequestBodyLogLimit = 4096;
    logging.ResponseBodyLogLimit = 4096;
});

builder.Services.AddSwaggerDoc(addJWTBearerAuth: true);

builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ICustomerAuthEndpointService, CustomerAuthEndpointService>();
builder.Services.AddScoped<IClassroomService, ClassroomService>();
builder.Services.AddScoped<IAdminService, AdminService>();

var app = builder.Build();

using (var scoop = app.Services.CreateScope())
{
    var services = scoop.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<TutorDbContext>();
        var logger = services.GetRequiredService<ILogger<Program>>();
        await context.Database.MigrateAsync().ConfigureAwait(false);
        logger.LogInformation("Migrate success");
        await context.SeedDbContextAsync(50, services.GetRequiredService<ILogger<TutorDbContext>>())
            .ConfigureAwait(false);
        logger.LogInformation("Seed success");
    }
    catch (Exception e)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(e, "An error occurred while migrating or seeding the database.");
    }
}

if (app.Environment.IsDevelopment())
{
    app.UseDefaultExceptionHandler();
    app.UseHttpLogging();
}

app.UseAuthentication();
app.UseAuthorization();
app.UseResponseCaching();

app.UseFastEndpoints(options =>
{
    options.Errors.ResponseBuilder = (errors, _, _) => errors.ToResponse();
    options.Errors.StatusCode = StatusCodes.Status422UnprocessableEntity;
    options.Serializer.Options.PropertyNamingPolicy = new JsonSnakeCaseNamingPolicy();
    options.Endpoints.RoutePrefix = "api";
    // options.Endpoints.Configurator = ep =>
    // {
    //     ep.PreProcessors(Order.Before, new HeaderRequestLogger());
    // };
});

// if (app.Environment.IsDevelopment())
// {
//     app.UseOpenApi();
//     app.UseSwaggerUi3(x =>
//     {
//         x.ConfigureDefaults();
//     });
// }
app.UseOpenApi();
app.UseSwaggerUi3(x =>
{
    x.ConfigureDefaults();
});

app.MapGet("/", context =>
{
    context.Response.Redirect("/swagger");
    return Task.CompletedTask;
});

await app.RunAsync().ConfigureAwait(false);

public partial class Program
{
}