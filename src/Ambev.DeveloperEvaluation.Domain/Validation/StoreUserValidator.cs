using Ambev.DeveloperEvaluation.Domain.Entities;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.Domain.Validation;

public class StoreUserValidator : AbstractValidator<StoreUser>
{
    public StoreUserValidator()
    {
        RuleFor(user => user.Email)
            .NotEmpty()
            .EmailAddress();

        RuleFor(user => user.Username)
            .NotEmpty();

        RuleFor(user => user.Password)
            .NotEmpty();

        RuleFor(user => user.Phone)
            .NotEmpty();

        RuleFor(user => user.Name.Firstname)
            .NotEmpty();

        RuleFor(user => user.Name.Lastname)
            .NotEmpty();

        RuleFor(user => user.Address.City)
            .NotEmpty();

        RuleFor(user => user.Address.Street)
            .NotEmpty();

        RuleFor(user => user.Address.Number)
            .GreaterThan(0);

        RuleFor(user => user.Address.Zipcode)
            .NotEmpty();

        RuleFor(user => user.Address.Geolocation.Lat)
            .NotEmpty();

        RuleFor(user => user.Address.Geolocation.Long)
            .NotEmpty();
    }
}
