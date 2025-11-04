namespace ApiHortifruti.Domain.DTO.Profiles;
using AutoMapper;

public class UsuarioProfile : Profile
{
    public UsuarioProfile()
    {
        CreateMap<UsuarioDTO, Usuario>().ReverseMap();
    }
}
