using Ambev.DeveloperEvaluation.Application.Products.UpdateProduct;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Products.UpdateProduct;

public class UpdateProductProfile : Profile
{
    public UpdateProductProfile()
    {
        CreateMap<UpdateProductRequest, UpdateProductCommand>()
            .ForMember(destination => destination.RatingRate, options => options.MapFrom(source => source.Rating.Rate))
            .ForMember(destination => destination.RatingCount, options => options.MapFrom(source => source.Rating.Count));
    }
}
