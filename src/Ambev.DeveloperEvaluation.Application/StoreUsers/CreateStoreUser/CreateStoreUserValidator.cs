using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.StoreUsers.CreateStoreUser;

public class CreateStoreUserValidator : AbstractValidator<CreateStoreUserCommand>
{
    public CreateStoreUserValidator()
    {
        RuleFor(user => user.Email).NotEmpty().EmailAddress();
        RuleFor(user => user.Username).NotEmpty();
        RuleFor(user => user.Password).NotEmpty();
        RuleFor(user => user.Firstname).NotEmpty();
        RuleFor(user => user.Lastname).NotEmpty();
        RuleFor(user => user.City).NotEmpty();
        RuleFor(user => user.Street).NotEmpty();
        RuleFor(user => user.Number).GreaterThan(0);
        RuleFor(user => user.Zipcode).NotEmpty();
        RuleFor(user => user.Lat).NotEmpty();
        RuleFor(user => user.Long).NotEmpty();
        RuleFor(user => user.Phone).NotEmpty();
    }
}
