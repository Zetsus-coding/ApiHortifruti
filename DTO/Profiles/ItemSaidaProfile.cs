namespace ApiHortifruti.DTO.Profiles;

using ApiHortifruti.Domain;
using ApiHortifruti.DTO.ItemSaida;

using AutoMapper;

public class ItemSaidaProfile : Profile
{
    public ItemSaidaProfile()
    {
        CreateMap<ItemSaidaDTO, ItemSaida>().ReverseMap();
    }
}