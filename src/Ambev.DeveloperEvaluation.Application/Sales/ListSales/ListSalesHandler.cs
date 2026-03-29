using AutoMapper;
using MediatR;
using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.Domain.Repositories;

namespace Ambev.DeveloperEvaluation.Application.Sales.ListSales;

public class ListSalesHandler : IRequestHandler<ListSalesCommand, PagedResult<ListSaleResultItem>>
{
    private readonly ISaleRepository _saleRepository;
    private readonly IMapper _mapper;

    public ListSalesHandler(ISaleRepository saleRepository, IMapper mapper)
    {
        _saleRepository = saleRepository;
        _mapper = mapper;
    }

    public async Task<PagedResult<ListSaleResultItem>> Handle(ListSalesCommand request, CancellationToken cancellationToken)
    {
        var filter = new ListSalesFilter
        {
            Page = request.Page,
            Size = request.Size,
            Order = ListSalesOrderParser.Parse(request.Order),
            SaleNumber = request.SaleNumber?.Trim(),
            CustomerName = request.CustomerName?.Trim(),
            BranchName = request.BranchName?.Trim(),
            IsCancelled = request.IsCancelled,
            SaleDateFrom = request.SaleDateFrom,
            SaleDateTo = request.SaleDateTo
        };

        var pagedSales = await _saleRepository.ListAsync(filter, cancellationToken);
        var items = _mapper.Map<List<ListSaleResultItem>>(pagedSales.Items);

        return new PagedResult<ListSaleResultItem>(items, pagedSales.CurrentPage, pagedSales.PageSize, pagedSales.TotalCount);
    }
}
