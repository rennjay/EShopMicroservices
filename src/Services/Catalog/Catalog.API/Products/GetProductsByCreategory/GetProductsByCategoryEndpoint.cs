
namespace Catalog.API.Products.GetProductsByCreategory;

public record GetProductsByCategoryResponse(IEnumerable<Product> Products);
public class GetProductsByCategoryEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/products/category/{category}", async (string category, ISender sender) =>
        {
            var query = new GetProductsByCategoryQuery(category);

            var result = await sender.Send(query);

            var response = result.Adapt<GetProductsByCategoryResponse>();

            return response;
        });
    }
}
