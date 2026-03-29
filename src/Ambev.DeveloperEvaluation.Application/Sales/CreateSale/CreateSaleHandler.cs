using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;

namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSale;

public class CreateSaleHandler : IRequestHandler<CreateSaleCommand, CreateSaleResult>
{
    private readonly ISaleRepository _saleRepository;
    private readonly ISaleNumberGenerator _saleNumberGenerator;
    private readonly IMapper _mapper;
    private readonly ILogger<CreateSaleHandler> _logger;

    public CreateSaleHandler(
        ISaleRepository saleRepository,
        ISaleNumberGenerator saleNumberGenerator,
        IMapper mapper,
        ILogger<CreateSaleHandler> logger)
    {
        _saleRepository = saleRepository;
        _saleNumberGenerator = saleNumberGenerator;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<CreateSaleResult> Handle(CreateSaleCommand command, CancellationToken cancellationToken)
    {
        var saleNumber = _saleNumberGenerator.Generate();
        var sale = Sale.Create(
            saleNumber,
            DateTime.UtcNow,
            command.CustomerExternalId,
            command.CustomerName,
            command.BranchExternalId,
            command.BranchName,
            command.Items.Select(item => new SaleItemInput(
                item.ProductExternalId,
                item.ProductName,
                item.Quantity,
                item.UnitPrice)));

        var createdSale = await _saleRepository.CreateAsync(sale, cancellationToken);
        var createdEvent = createdSale.CreateCreatedEvent();

        _logger.LogInformation(
            "SaleCreated: SaleId={SaleId}, SaleNumber={SaleNumber}, ItemCount={ItemCount}, TotalAmount={TotalAmount}",
            createdEvent.SaleId,
            createdEvent.SaleNumber,
            createdSale.Items.Count,
            createdSale.TotalAmount);

        return _mapper.Map<CreateSaleResult>(createdSale);
    }
}
