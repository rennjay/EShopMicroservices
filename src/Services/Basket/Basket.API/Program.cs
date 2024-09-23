var builder = WebApplication.CreateBuilder(args);
{
    var targetAssembly = typeof(Program).Assembly;
    builder.Services.AddMediatR(opt =>
    {
        opt.RegisterServicesFromAssembly(targetAssembly);
    });
    builder.Services.AddCarter();
}

var app = builder.Build();

app.MapCarter();
app.Run();
