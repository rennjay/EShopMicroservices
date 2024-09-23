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
}

var app = builder.Build();

app.MapCarter();
app.Run();
