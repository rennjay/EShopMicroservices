﻿
namespace Catalog.API.Products.GetProductsByCreategory;

public record GetProductsByCategoryQuery(string Category) : IQuery<GetProductsByCategoryResult>;
public record GetProductsByCategoryResult(IEnumerable<Product> Products);

public class GetProductsByCategoryHandler(IDocumentSession session, ILogger<GetProductsByCategoryHandler> logger) 
    : IQueryHandler<GetProductsByCategoryQuery, GetProductsByCategoryResult>
{
    public async Task<GetProductsByCategoryResult> Handle(GetProductsByCategoryQuery query, CancellationToken cancellationToken)
    {
        logger.LogInformation("GetProductsByCategoryHandler.Handle was called with parameter: {@query}", query);

        var products = await session.Query<Product>().Where(p => p.Category.Contains(query.Category)).ToListAsync(cancellationToken);

        return new GetProductsByCategoryResult(products);
    }
}
