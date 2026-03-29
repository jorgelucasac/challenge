using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;
using Ambev.DeveloperEvaluation.Domain.Repositories;

namespace Ambev.DeveloperEvaluation.Application.Sales.ActivateSale;

public class ActivateSaleHandler : IRequestHandler<ActivateSaleCommand, CreateSaleResult>
{
    private readonly ISaleRepository _saleRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<ActivateSaleHandler> _logger;

    public ActivateSaleHandler(
        ISaleRepository saleRepository,
        IUnitOfWork unitOfWork,
        IMapper mapper,
        ILogger<ActivateSaleHandler> logger)
    {
        _saleRepository = saleRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<CreateSaleResult> Handle(ActivateSaleCommand request, CancellationToken cancellationToken)
    {
        var sale = await _saleRepository.GetByIdForUpdateAsync(request.Id, cancellationToken);
        if (sale == null)
        {
            throw new KeyNotFoundException($"Sale with ID {request.Id} not found");
        }

        var activatedEvent = sale.Activate();
        var activatedSale = await _saleRepository.UpdateAsync(sale, cancellationToken);
        await _unitOfWork.CommitAsync(cancellationToken);

        _logger.LogInformation(
            "SaleActivated: SaleId={SaleId}, SaleNumber={SaleNumber}, ItemCount={ItemCount}, TotalAmount={TotalAmount}",
            activatedEvent.SaleId,
            activatedEvent.SaleNumber,
            activatedSale.Items.Count,
            activatedSale.TotalAmount);

        return _mapper.Map<CreateSaleResult>(activatedSale);
    }
}
