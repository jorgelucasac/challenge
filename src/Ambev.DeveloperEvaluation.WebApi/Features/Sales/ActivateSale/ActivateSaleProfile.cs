using AutoMapper;
using Ambev.DeveloperEvaluation.Application.Sales.ActivateSale;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.ActivateSale;

public class ActivateSaleProfile : Profile
{
    public ActivateSaleProfile()
    {
        CreateMap<Guid, ActivateSaleCommand>()
            .ConstructUsing(id => new ActivateSaleCommand(id));
    }
}
