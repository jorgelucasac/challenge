using AutoMapper;
using MediatR;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;

namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSale;

public class CreateSaleHandler : IRequestHandler<CreateSaleCommand, CreateSaleResult>
{
    private readonly ISaleRepository _saleRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ISaleNumberGenerator _saleNumberGenerator;
    private readonly IMapper _mapper;

    public CreateSaleHandler(
        ISaleRepository saleRepository,
        IUnitOfWork unitOfWork,
        ISaleNumberGenerator saleNumberGenerator,
        IMapper mapper)
    {
        _saleRepository = saleRepository;
        _unitOfWork = unitOfWork;
        _saleNumberGenerator = saleNumberGenerator;
        _mapper = mapper;
    }

    public async Task<CreateSaleResult> Handle(CreateSaleCommand command, CancellationToken cancellationToken)
    {
        var saleNumber = _saleNumberGenerator.Generate();
        var sale = Sale.Create(
            saleNumber,
            command.SaleDate,
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
        await _unitOfWork.CommitAsync(cancellationToken);

        return _mapper.Map<CreateSaleResult>(createdSale);
    }
}
