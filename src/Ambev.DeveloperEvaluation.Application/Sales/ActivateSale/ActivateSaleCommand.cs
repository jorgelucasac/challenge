using MediatR;
using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;

namespace Ambev.DeveloperEvaluation.Application.Sales.ActivateSale;

public record ActivateSaleCommand(Guid Id) : IRequest<CreateSaleResult>;
