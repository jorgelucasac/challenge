using Ambev.DeveloperEvaluation.Application.StoreUsers.Common;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.StoreUsers.DeleteStoreUser;

public record DeleteStoreUserCommand(int Id) : IRequest<StoreUserResult>;
