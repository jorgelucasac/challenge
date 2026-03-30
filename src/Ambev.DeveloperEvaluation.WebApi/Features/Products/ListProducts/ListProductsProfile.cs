using Ambev.DeveloperEvaluation.Application.Products.Common;
using Ambev.DeveloperEvaluation.Application.Products.ListProducts;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Products.ListProducts;

public class ListProductsProfile : Profile
{
    public ListProductsProfile()
    {
        CreateMap<ListProductsRequest, ListProductsCommand>()
            .ForMember(destination => destination.Page, options => options.MapFrom(source => source._page ?? ListProductsDefaults.DefaultPage))
            .ForMember(destination => destination.Size, options => options.MapFrom(source => source._size ?? ListProductsDefaults.DefaultPageSize))
            .ForMember(destination => destination.Order, options => options.MapFrom(source => ListProductsOrderParser.Normalize(source._order)))
            .ForMember(destination => destination.MinPrice, options => options.MapFrom(source => source._minPrice))
            .ForMember(destination => destination.MaxPrice, options => options.MapFrom(source => source._maxPrice));

        CreateMap<ListProductsByCategoryRequest, ListProductsCommand>()
            .ForMember(destination => destination.Page, options => options.MapFrom(source => source._page ?? ListProductsDefaults.DefaultPage))
            .ForMember(destination => destination.Size, options => options.MapFrom(source => source._size ?? ListProductsDefaults.DefaultPageSize))
            .ForMember(destination => destination.Order, options => options.MapFrom(source => ListProductsOrderParser.Normalize(source._order)))
            .ForMember(destination => destination.Title, options => options.Ignore())
            .ForMember(destination => destination.Price, options => options.Ignore())
            .ForMember(destination => destination.MinPrice, options => options.Ignore())
            .ForMember(destination => destination.MaxPrice, options => options.Ignore());
    }
}
