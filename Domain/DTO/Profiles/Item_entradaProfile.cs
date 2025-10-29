namespace ApiHortifruti.Domain.DTO.Profiles;

using ApiHortifruti.Domain.DTO.Item_entradaDTO;

using AutoMapper;

public class Item_entradaProfile : Profile
{
    public Item_entradaProfile()
    {
        CreateMap<Item_entradaDTO, Item_entrada>().ReverseMap();
    }
}
