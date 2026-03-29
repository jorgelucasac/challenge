using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;
using Ambev.DeveloperEvaluation.Domain.Repositories;

namespace Ambev.DeveloperEvaluation.Application.Sales.CancelSale;

public class CancelSaleHandler : IRequestHandler<CancelSaleCommand, CreateSaleResult>
{
    private readonly ISaleRepository _saleRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<CancelSaleHandler> _logger;

    public CancelSaleHandler(
        ISaleRepository saleRepository,
        IUnitOfWork unitOfWork,
        IMapper mapper,
        ILogger<CancelSaleHandler> logger)
    {
        _saleRepository = saleRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<CreateSaleResult> Handle(CancelSaleCommand request, CancellationToken cancellationToken)
    {
        var sale = await _saleRepository.GetByIdForUpdateAsync(request.Id, cancellationToken);
        if (sale == null)
        {
            throw new KeyNotFoundException($"Sale with ID {request.Id} not found");
        }

        var cancelledEvent = sale.Cancel();
        var cancelledSale = await _saleRepository.UpdateAsync(sale, cancellationToken);
        await _unitOfWork.CommitAsync(cancellationToken);

        _logger.LogInformation(
            "SaleCancelled: SaleId={SaleId}, SaleNumber={SaleNumber}, ItemCount={ItemCount}, TotalAmount={TotalAmount}",
            cancelledEvent.SaleId,
            cancelledEvent.SaleNumber,
            cancelledSale.Items.Count,
            cancelledSale.TotalAmount);

        return _mapper.Map<CreateSaleResult>(cancelledSale);
    }
}
