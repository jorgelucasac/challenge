using Ambev.DeveloperEvaluation.Application.StoreUsers.Common;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.StoreUsers.GetStoreUser;

public record GetStoreUserCommand(int Id) : IRequest<StoreUserResult>;
