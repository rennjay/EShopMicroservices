using BuildingBlocks.Exceptions.Handlers;

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
}

var app = builder.Build();
app.UseExceptionHandler( _ => {  });
app.MapCarter();
app.Run();
