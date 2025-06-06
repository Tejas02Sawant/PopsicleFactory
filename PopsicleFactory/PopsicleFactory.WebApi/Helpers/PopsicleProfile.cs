using AutoMapper;
using PopsicleFactory.DataProvider.Models;
using PopsicleFactory.WebApi.Models;

namespace PopsicleFactory.WebApi.Helpers;

public class PopsicleProfile : Profile
{
    public PopsicleProfile()
    {
        CreateMap<Popsicle, PopsicleReadDto>();
        CreateMap<PopsicleCreateDto, Popsicle>();
    }
}
