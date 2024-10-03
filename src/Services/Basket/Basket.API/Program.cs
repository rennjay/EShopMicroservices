using BuildingBlocks.Exceptions.Handlers;
using HealthChecks.UI.Client;

var builder = WebApplication.CreateBuilder(args);
{
    var targetAssembly = typeof(Program).Assembly;
    builder.Services.AddMediatR(opt =>
    {
        opt.RegisterServicesFromAssembly(targetAssembly);
        opt.AddOpenBehavior(typeof(ValidationBehaviour<,>));
        opt.AddOpenBehavior(typeof(LoggingBehaviour<,>));
    });
    builder.Services.AddCarter();
    builder.Services.AddValidatorsFromAssembly(targetAssembly);

    builder.Services.AddMarten(opts =>
    {
        opts.Connection(builder.Configuration.GetConnectionString("Database")!);
        opts.Schema.For<ShoppingCart>().Identity(x => x.UserName);
    }).UseLightweightSessions();

    builder.Services.AddScoped<IBasketRepository, BasketRepository>();

    builder.Services.AddExceptionHandler<CustomExeptionHandler>();

    builder.Services.AddScoped<IBasketRepository, BasketRepository>();
    builder.Services.Decorate<IBasketRepository, CachedBasketRepository>();

    builder.Services.AddStackExchangeRedisCache(options =>
    {
        options.Configuration = builder.Configuration.GetConnectionString("Redis");
        //options.InstanceName = "Basket";
    });

    builder.Services.AddHealthChecks()
        .AddNpgSql(builder.Configuration.GetConnectionString("Database")!)
        .AddRedis(builder.Configuration.GetConnectionString("Redis")!);
}

var app = builder.Build();
app.UseExceptionHandler( _ => {  });
app.UseHealthChecks("/health", new Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions()
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

app.MapCarter();
app.Run();
