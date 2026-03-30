using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.StoreUsers.DeleteStoreUser;

public class DeleteStoreUserValidator : AbstractValidator<DeleteStoreUserCommand>
{
    public DeleteStoreUserValidator()
    {
        RuleFor(user => user.Id).GreaterThan(0);
    }
}
