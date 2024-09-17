namespace Catalog.API.Products.CreateProduct;

public record CreateProductCommand(string Name, List<string> Category, string Description, string ImageFile, decimal Price) : ICommand<CreateProductResult>;
public record CreateProductResult(Guid Id);

public class CreateProductValidator : AbstractValidator<CreateProductCommand>
{
    public CreateProductValidator()
    {
        RuleFor(p => p.Name).NotEmpty().WithMessage("Name is required");
        RuleFor(p => p.Category).NotEmpty().WithMessage("Name is required");
        RuleFor(p => p.Description).NotEmpty().WithMessage("Name is required");
        RuleFor(p => p.ImageFile).NotEmpty().WithMessage("Name is required");
        RuleFor(p => p.Price).GreaterThan(0).WithMessage("Price should not be zero");
    }
}

public class CreateProductHandler(IDocumentSession session) 
    : ICommandHandler<CreateProductCommand, CreateProductResult>
{
    public async Task<CreateProductResult> Handle(CreateProductCommand command, CancellationToken cancellationToken)
    {
        var newProduct = new Product()
        {
            Id = new Guid(),
            Name = command.Name,
            Category = command.Category,
            Description = command.Description,
            ImageFile = command.ImageFile,
            Price = command.Price
        };

        session.Store(newProduct);
        await session.SaveChangesAsync(cancellationToken);

        return new CreateProductResult(newProduct.Id);
    }
}