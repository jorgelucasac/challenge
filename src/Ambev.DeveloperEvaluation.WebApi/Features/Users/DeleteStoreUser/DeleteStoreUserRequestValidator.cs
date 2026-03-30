using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Users.DeleteStoreUser;

public class DeleteStoreUserRequestValidator : AbstractValidator<DeleteStoreUserRequest>
{
    public DeleteStoreUserRequestValidator()
    {
        RuleFor(user => user.Id).GreaterThan(0);
    }
}
