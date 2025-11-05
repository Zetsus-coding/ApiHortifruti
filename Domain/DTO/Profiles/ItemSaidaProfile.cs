namespace ApiHortifruti.Domain.DTO.Profiles;

using ApiHortifruti.Domain;
using ApiHortifruti.Domain.DTO.ItemSaida;

using AutoMapper;

public class ItemSaidaProfile : Profile
{
    public ItemSaidaProfile()
    {
        CreateMap<ItemSaidaDTO, ItemSaida>().ReverseMap();
    }
}