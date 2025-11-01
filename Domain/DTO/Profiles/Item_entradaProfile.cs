namespace ApiHortifruti.Domain.DTO.Profiles;

using ApiHortifruti.Domain.DTO.ItemEntradaDTO;

using AutoMapper;

public class ItemEntradaProfile : Profile
{
    public ItemEntradaProfile()
    {
        CreateMap<ItemEntradaDTO, ItemEntrada>().ReverseMap();
    }
}
