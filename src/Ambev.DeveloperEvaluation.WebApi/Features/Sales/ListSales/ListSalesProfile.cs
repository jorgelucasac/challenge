using AutoMapper;
using Ambev.DeveloperEvaluation.Application.Sales.ListSales;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.ListSales;

public class ListSalesProfile : Profile
{
    public ListSalesProfile()
    {
        CreateMap<ListSalesRequest, ListSalesCommand>()
            .ForMember(destination => destination.Page, options => options.MapFrom(source => source._page ?? ListSalesDefaults.DefaultPage))
            .ForMember(destination => destination.Size, options => options.MapFrom(source => source._size ?? ListSalesDefaults.DefaultPageSize))
            .ForMember(destination => destination.Order, options => options.MapFrom(source => ListSalesOrderParser.Normalize(source._order)));

        CreateMap<ListSaleResultItem, ListSaleResponse>();
    }
}
