using Ambev.DeveloperEvaluation.Application.StoreUsers.Common;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Users.ListStoreUsers;

public class ListStoreUsersRequestValidator : AbstractValidator<ListStoreUsersRequest>
{
    public ListStoreUsersRequestValidator()
    {
        RuleFor(user => user._page).GreaterThan(0);
        RuleFor(user => user._size).GreaterThan(0).LessThanOrEqualTo(ListStoreUsersDefaults.MaxPageSize);
        RuleFor(user => user._order)
            .Must(ListStoreUsersOrderParser.IsSupported)
            .WithMessage("Order must use supported fields and directions.");
    }
}
