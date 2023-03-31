
using TatBlog.WebApi.Endpoints;
using TatBlog.WebApi.Mapsters;
using TatBlog.WebApi.Validations;
using TatBlog.WebApp.Extensions;

var builder = WebApplication.CreateBuilder(args);
{
    // Add services to the container.
    builder
        .ConfigureCors()
        .ConfigureNLog()
        .ConfigureServices()
        .ConfigureSwaggerOpenApi()
        .ConfigureMapster()
        .ConfigureFluentValidation();
}
var app = builder.Build();
{
    // Configure the HTTP request pipeline.
    app.SetupRequestPipeLine();

    // Configure API endpoints
    app.MapAuthorEndpoints();
    app.MapCategoryEndpoints();
    app.MapPostEndpoints();
    app.MapTagEndPoints();

    app.Run();
}

