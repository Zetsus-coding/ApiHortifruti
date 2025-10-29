namespace ApiHortifruti.Domain.DTO.Profiles;
using AutoMapper;

public class Unidade_medidaProfile : Profile
{
    public Unidade_medidaProfile()
    {
        CreateMap<PostUnidade_medidaDTO, Unidade_medida>().ReverseMap();
    }
}
