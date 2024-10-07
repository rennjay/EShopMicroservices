
using Discount.Grpc;
using Grpc.Core;

namespace Basket.API.Basket.StoreBasket;

public record StoreBasketCommand(ShoppingCart Cart) : ICommand<StoreBasketResult>;

public record StoreBasketResult(string UserName);

public class StoreBasketValidator : AbstractValidator<StoreBasketCommand>
{
    public StoreBasketValidator()
    {
        RuleFor(c => c.Cart.UserName).NotEmpty().WithMessage("Username is required");
        RuleFor(x => x.Cart).NotNull().WithMessage("Cart can not be null");
    }
}

internal class StoreBasketHandler(IBasketRepository basketRepository, DiscountProtoService.DiscountProtoServiceClient discountServiceClient, ILogger<StoreBasketHandler> logger) 
    : ICommandHandler<StoreBasketCommand, StoreBasketResult>
{
    public async Task<StoreBasketResult> Handle(StoreBasketCommand command, CancellationToken cancellationToken)
    {
        ApplyDiscountsToCartItems(discountServiceClient, command, cancellationToken);

        await basketRepository.StoreBasketAsync(command.Cart, cancellationToken);

        return new StoreBasketResult(command.Cart.UserName);
    }
    private static void ApplyDiscountsToCartItems(DiscountProtoService.DiscountProtoServiceClient discountServiceClient, StoreBasketCommand command, CancellationToken cancellationToken)
    {
        foreach (var cartItem in command.Cart.Items)
        {
            try
            {
                var discount = discountServiceClient.GetDiscount(new GetDiscountRequest() { ProductName = cartItem.ProductName }, cancellationToken: cancellationToken);
                cartItem.Price -= discount.Amount;
            }
            catch (RpcException ex) when (ex.StatusCode == StatusCode.NotFound)
            {
                // No discount
            }
        }
    }

}
