using ApiHortifruti.Domain;
using AutoMapper;

public class ItemEntradaProfile : Profile
{
    public ItemEntradaProfile()
    {
        CreateMap<ItemEntradaDTO, ItemEntrada>().ReverseMap();
    }
}
