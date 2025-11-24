using ApiHortifruti.Domain;
using AutoMapper;

public class FuncionarioProfile : Profile
{
    public FuncionarioProfile()
    {
        CreateMap<PostFuncionarioDTO, Funcionario>().ReverseMap();
    }
}
