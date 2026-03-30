using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.ValueObjects;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.Application.Products.Common;

public class ProductProfile : Profile
{
    public ProductProfile()
    {
        CreateMap<Product, ProductResult>();
        CreateMap<ProductRating, ProductRatingResult>();
    }
}
