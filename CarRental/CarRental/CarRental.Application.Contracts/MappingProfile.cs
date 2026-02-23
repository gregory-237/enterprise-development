using AutoMapper;
using CarRental.Application.Contracts.Dto;
using CarRental.Domain.Entities;

namespace CarRental.Application.Contracts;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<CarModel, CarModelGetDto>();
        CreateMap<CarModelEditDto, CarModel>();

        CreateMap<ModelGeneration, ModelGenerationGetDto>()
            .ForMember(d => d.Model, o => o.MapFrom(s => s.Model));
        CreateMap<ModelGenerationEditDto, ModelGeneration>();

        CreateMap<Car, CarGetDto>()
            .ForMember(d => d.ModelGeneration, o => o.MapFrom(s => s.ModelGeneration));
        CreateMap<CarEditDto, Car>();

        CreateMap<Client, ClientGetDto>();
        CreateMap<ClientEditDto, Client>();

        CreateMap<Rental, RentalGetDto>()
            .ForMember(d => d.Car,    o => o.MapFrom(s => s.Car))
            .ForMember(d => d.Client, o => o.MapFrom(s => s.Client));
        CreateMap<RentalEditDto, Rental>();
    }
}
