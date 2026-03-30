using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.StoreUsers.GetStoreUser;

public class GetStoreUserValidator : AbstractValidator<GetStoreUserCommand>
{
    public GetStoreUserValidator()
    {
        RuleFor(user => user.Id).GreaterThan(0);
    }
}
