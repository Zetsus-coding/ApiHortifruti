using ApiHortifruti.Domain;
using AutoMapper;

public class ItemSaidaProfile : Profile
{
    public ItemSaidaProfile()
    {
        CreateMap<ItemSaidaDTO, ItemSaida>().ReverseMap();
    }
}