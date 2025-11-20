namespace ApiHortifruti.Domain.DTO.Profiles;
using AutoMapper;

public class FuncionarioProfile : Profile
{
    public FuncionarioProfile()
    {
        CreateMap<PostFuncionarioDTO, Funcionario>().ReverseMap();
    }
}
