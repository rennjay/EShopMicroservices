using BuildingBlocks.Behaviours;
using BuildingBlocks.Exceptions.Handlers;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using static System.Net.Mime.MediaTypeNames;

var builder = WebApplication.CreateBuilder(args);
{
    var assembly = typeof(Program).Assembly;
    builder.Services.AddMediatR(config =>
    {
        config.RegisterServicesFromAssembly(assembly);
        config.AddOpenBehavior(typeof(ValidationBehaviour<,>));
    });

    builder.Services.AddCarter();
    builder.Services.AddValidatorsFromAssembly(assembly);

    builder.Services.AddMarten(options =>
    {
        options.Connection(builder.Configuration.GetConnectionString("Database")!);
    }).UseLightweightSessions();

    builder.Services.AddExceptionHandler<CustomExeptionHandler>();
}

var app = builder.Build();
{
    app.UseExceptionHandler(option => { });
    app.MapCarter();
    app.Run();
}
