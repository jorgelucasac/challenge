using MediatR;
using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;

namespace Ambev.DeveloperEvaluation.Application.Sales.GetSale;

public record GetSaleCommand(Guid Id) : IRequest<CreateSaleResult>;
