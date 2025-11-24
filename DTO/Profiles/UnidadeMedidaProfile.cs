using ApiHortifruti.Domain;
using AutoMapper;

public class UnidadeMedidaProfile : Profile
{
    public UnidadeMedidaProfile()
    {
        CreateMap<PostUnidadeMedidaDTO, UnidadeMedida>().ReverseMap();
        CreateMap<PutUnidadeMedidaDTO, UnidadeMedida>().ReverseMap();
        CreateMap<UnidadeMedida, GetUnidadeMedidaDTO>().ReverseMap();
    }
}
