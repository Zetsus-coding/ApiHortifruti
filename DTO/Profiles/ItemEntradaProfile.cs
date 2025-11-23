namespace ApiHortifruti.DTO.Profiles;

using ApiHortifruti.Domain;
using ApiHortifruti.DTO.ItemEntrada;

using AutoMapper;

public class ItemEntradaProfile : Profile
{
    public ItemEntradaProfile()
    {
        CreateMap<ItemEntradaDTO, ItemEntrada>().ReverseMap();
    }
}
