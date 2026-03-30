using Ambev.DeveloperEvaluation.Application.Products.Common;
using Ambev.DeveloperEvaluation.Application.Products.CreateProduct;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Products.CreateProduct;

public class CreateProductProfile : Profile
{
    public CreateProductProfile()
    {
        CreateMap<CreateProductRequest, CreateProductCommand>()
            .ForMember(destination => destination.RatingRate, options => options.MapFrom(source => source.Rating.Rate))
            .ForMember(destination => destination.RatingCount, options => options.MapFrom(source => source.Rating.Count));

        CreateMap<ProductResult, ProductResponse>();
        CreateMap<ProductRatingResult, ProductRatingResponse>();
    }
}
