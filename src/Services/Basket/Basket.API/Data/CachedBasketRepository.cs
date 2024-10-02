
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;
using System.Threading;

namespace Basket.API.Data;

public class CachedBasketRepository(IBasketRepository basketRepository, IDistributedCache cache) : IBasketRepository
{

    public async Task<ShoppingCart> GetBasketAsync(string userName, CancellationToken cancellation = default)
    {
        var cachedBasket = await cache.GetStringAsync(userName, cancellation);
        if (!string.IsNullOrEmpty(cachedBasket))
            return JsonSerializer.Deserialize<ShoppingCart>(cachedBasket)!;

        var basket = await basketRepository.GetBasketAsync(userName, cancellation);
        await cache.SetStringAsync(userName, JsonSerializer.Serialize<ShoppingCart>(basket), cancellation);

        return basket;
    }

    public async Task<ShoppingCart> StoreBasketAsync(ShoppingCart basket, CancellationToken cancellation = default)
    {
        await basketRepository.StoreBasketAsync(basket, cancellation);
        await cache.SetStringAsync(basket.UserName, JsonSerializer.Serialize<ShoppingCart>(basket), cancellation);
        return basket;
    }
    public async Task<bool> DeleteBasketAsync(string userName, CancellationToken cancellation = default)
    {
        await basketRepository.DeleteBasketAsync(userName, cancellation);
        await cache.RemoveAsync(userName, cancellation);
        return true;
    }
}
