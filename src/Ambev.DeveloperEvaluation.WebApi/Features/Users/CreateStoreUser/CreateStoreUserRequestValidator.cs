using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Users.CreateStoreUser;

public class CreateStoreUserRequestValidator : AbstractValidator<CreateStoreUserRequest>
{
    public CreateStoreUserRequestValidator()
    {
        RuleFor(user => user.Email).NotEmpty().EmailAddress();
        RuleFor(user => user.Username).NotEmpty();
        RuleFor(user => user.Password).NotEmpty();
        RuleFor(user => user.Name.Firstname).NotEmpty();
        RuleFor(user => user.Name.Lastname).NotEmpty();
        RuleFor(user => user.Address.City).NotEmpty();
        RuleFor(user => user.Address.Street).NotEmpty();
        RuleFor(user => user.Address.Number).GreaterThan(0);
        RuleFor(user => user.Address.Zipcode).NotEmpty();
        RuleFor(user => user.Address.Geolocation.Lat).NotEmpty();
        RuleFor(user => user.Address.Geolocation.Long).NotEmpty();
        RuleFor(user => user.Phone).NotEmpty();
        RuleFor(user => user.Status)
            .Must(value => Enum.TryParse<Ambev.DeveloperEvaluation.Domain.Enums.UserStatus>(value, true, out var parsed) && parsed != Ambev.DeveloperEvaluation.Domain.Enums.UserStatus.Unknown)
            .WithMessage("Status must be Active, Inactive, or Suspended.");
        RuleFor(user => user.Role)
            .Must(value => Enum.TryParse<Ambev.DeveloperEvaluation.Domain.Enums.UserRole>(value, true, out var parsed) && parsed != Ambev.DeveloperEvaluation.Domain.Enums.UserRole.None)
            .WithMessage("Role must be Customer, Manager, or Admin.");
    }
}
