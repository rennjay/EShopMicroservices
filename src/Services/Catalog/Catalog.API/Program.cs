using BuildingBlocks.Behaviours;
using BuildingBlocks.Exceptions.Handlers;
using Catalog.API.Data;

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
}

var app = builder.Build();
{
    app.UseExceptionHandler(option => { });
    app.MapCarter();
    app.Run();
}
