namespace ApiHortifruti.Domain.DTO.Profiles;
using AutoMapper;

public class SaidaProfile : Profile
{
    public SaidaProfile()
    {
        CreateMap<PostSaidaDTO, Saida>().ReverseMap();
    }
}