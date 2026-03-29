using AutoMapper;
using MediatR;
using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;
using Ambev.DeveloperEvaluation.Domain.Repositories;

namespace Ambev.DeveloperEvaluation.Application.Sales.UpdateSale;

public class UpdateSaleHandler : IRequestHandler<UpdateSaleCommand, CreateSaleResult>
{
    private readonly ISaleRepository _saleRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdateSaleHandler(
        ISaleRepository saleRepository,
        IUnitOfWork unitOfWork,
        IMapper mapper)
    {
        _saleRepository = saleRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<CreateSaleResult> Handle(UpdateSaleCommand request, CancellationToken cancellationToken)
    {
        var sale = await _saleRepository.GetByIdForUpdateAsync(request.Id, cancellationToken);
        if (sale == null)
        {
            throw new KeyNotFoundException($"Sale with ID {request.Id} not found");
        }

        sale.UpdateDetails(
            request.SaleDate,
            request.CustomerExternalId,
            request.CustomerName,
            request.BranchExternalId,
            request.BranchName);

        foreach (var item in request.Items)
        {
            if (item.Id.HasValue)
            {
                if (item.IsCancelled)
                {
                    sale.CancelItem(item.Id.Value);
                }
                else
                {
                    sale.UpdateItem(
                        item.Id.Value,
                        item.Quantity,
                        item.UnitPrice,
                        item.ProductName,
                        item.ProductExternalId);
                }

                continue;
            }

            sale.AddItem(item.ProductExternalId, item.ProductName, item.Quantity, item.UnitPrice);
        }

        sale.MarkAsModified();
        var updatedSale = await _saleRepository.UpdateAsync(sale, cancellationToken);
        await _unitOfWork.CommitAsync(cancellationToken);

        return _mapper.Map<CreateSaleResult>(updatedSale);
    }
}
