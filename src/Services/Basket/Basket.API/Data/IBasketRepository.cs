namespace Basket.API.Data;

public interface IBasketRepository
{
    Task<ShoppingCart> GetBasketAsync(string userName, CancellationToken cancellation);
    Task<ShoppingCart> StoreBasketAsync(ShoppingCart basket, CancellationToken cancellation);
    Task<bool> DeleteBasketAsync(string userName, CancellationToken cancellation);
}
