using Ambev.DeveloperEvaluation.Application.StoreUsers.Common;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.StoreUsers.ListStoreUsers;

public class ListStoreUsersValidator : AbstractValidator<ListStoreUsersCommand>
{
    public ListStoreUsersValidator()
    {
        RuleFor(user => user.Page).GreaterThan(0);
        RuleFor(user => user.Size).GreaterThan(0).LessThanOrEqualTo(ListStoreUsersDefaults.MaxPageSize);
        RuleFor(user => user.Order)
            .Must(ListStoreUsersOrderParser.IsSupported)
            .WithMessage("Order must use supported fields and directions.");
    }
}
