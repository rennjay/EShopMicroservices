
namespace Catalog.API.Products.DeleteProduct;

public record DeleteProductCommand(Guid Id) : ICommand<DeleteProductResult>;
public record DeleteProductResult(bool IsSuccess);

internal class DeleteProductCommandHandler(ILogger<DeleteProductCommandHandler> logger, IDocumentSession session) : ICommandHandler<DeleteProductCommand, DeleteProductResult>
{
    public async Task<DeleteProductResult> Handle(DeleteProductCommand command, CancellationToken cancellationToken)
    {
        logger.LogInformation("DeleteProductCommandHandler.Handle is called with product Id: {Id}", command.Id);
        try
        {
            session.Delete<Product>(command.Id);
            await session.SaveChangesAsync(cancellationToken);

            return new DeleteProductResult(IsSuccess: true);
        }
        catch (Exception)
        {
            logger.LogError("DeleteProductCommandHandler.Handle failed and unable to delete product Id: {Id}", command.Id);
            return new DeleteProductResult(IsSuccess: false);
        }
    }
}
