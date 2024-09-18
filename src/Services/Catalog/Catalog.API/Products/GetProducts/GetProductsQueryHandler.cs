
using Marten.Pagination;

namespace Catalog.API.Products.GetProducts;

public record GetProductsQuery(int PageNumber = 1, int PageSize = 10) : IQuery<GetProductsResult>;
public record GetProductsResult(IEnumerable<Product> Products);

internal class GetProductsQueryHandler(IDocumentSession session)
    : IQueryHandler<GetProductsQuery, GetProductsResult>
{
    public async Task<GetProductsResult> Handle(GetProductsQuery query, CancellationToken cancellationToken)
    {
        var (pageNumber, pageSize) = query;
        var products = await session.Query<Product>().ToPagedListAsync(pageNumber, pageSize, cancellationToken);
        return new(products);
    }
}