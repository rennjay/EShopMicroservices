using BuildingBlocks.Behaviours;
using BuildingBlocks.Exceptions.Handlers;
using Catalog.API.Data;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;

var builder = WebApplication.CreateBuilder(args);
{
    var assembly = typeof(Program).Assembly;
    builder.Services.AddMediatR(config =>
    {
        config.RegisterServicesFromAssembly(assembly);
        config.AddOpenBehavior(typeof(ValidationBehaviour<,>));
        config.AddOpenBehavior(typeof(LoggingBehaviour<,>));
    });

    builder.Services.AddCarter();
    builder.Services.AddValidatorsFromAssembly(assembly);

    builder.Services.AddMarten(options =>
    {
        options.Connection(builder.Configuration.GetConnectionString("Database")!);
    }).UseLightweightSessions();

    if(builder.Environment.IsDevelopment())
        builder.Services.InitializeMartenWith<CatalogInitialData>();

    builder.Services.AddExceptionHandler<CustomExeptionHandler>();

    builder.Services.AddHealthChecks()
        .AddNpgSql(builder.Configuration.GetConnectionString("Database")!);
}

var app = builder.Build();
{
    app.UseExceptionHandler(option => { });
    app.UseHealthChecks("/health",
     new HealthCheckOptions
     {
         ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
     });
    app.MapCarter();
    app.Run();
}
