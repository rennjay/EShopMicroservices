using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;

namespace Discount.Grpc.Data;

public static class DataExtensions
{
    public static async void MigrateDatabase(this IApplicationBuilder app)
    {
        using var scopedServices = app.ApplicationServices.CreateScope();

        var dbContext = scopedServices.ServiceProvider.GetRequiredService<DiscountDbContext>();
        await dbContext.Database.MigrateAsync();
    }
}
