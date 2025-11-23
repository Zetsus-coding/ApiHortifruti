namespace ApiHortifruti.DTO.Profiles;

using ApiHortifruti.Domain;
using AutoMapper;

public class SaidaProfile : Profile
{
    public SaidaProfile()
    {
        CreateMap<PostSaidaDTO, Saida>().ReverseMap();
    }
}