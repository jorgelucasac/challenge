using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Users.GetStoreUser;

public class GetStoreUserRequestValidator : AbstractValidator<GetStoreUserRequest>
{
    public GetStoreUserRequestValidator()
    {
        RuleFor(user => user.Id).GreaterThan(0);
    }
}
