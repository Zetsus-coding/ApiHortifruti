using ApiHortifruti.Domain;
using AutoMapper;

namespace ApiHortifruti.DTO.Profiles;

public class FuncionarioProfile : Profile
{
    public FuncionarioProfile()
    {
        CreateMap<PostFuncionarioDTO, Funcionario>().ReverseMap();
        CreateMap<PutFuncionarioDTO, Funcionario>().ReverseMap();
        CreateMap<Funcionario, GetFuncionarioDTO>().ReverseMap();
    }
}
