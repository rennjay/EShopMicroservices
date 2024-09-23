
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

internal class StoreBasketHandler : ICommandHandler<StoreBasketCommand, StoreBasketResult>
{
    public Task<StoreBasketResult> Handle(StoreBasketCommand command, CancellationToken cancellationToken)
    {
        return Task.FromResult(new StoreBasketResult(command.Cart.UserName));
    }
}
