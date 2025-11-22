namespace ApiHortifruti.Domain.DTO.Profiles;
using AutoMapper;

public class EntradaProfile : Profile
{
    public EntradaProfile()
    {
        CreateMap<PostEntradaDTO, Entrada>().ReverseMap();

        CreateMap<Entrada, GetEntradaSimplesDTO>()
            .ForMember(dest => dest.NomeFantasiaFornecedor, opt => opt.MapFrom(src => src.Fornecedor.NomeFantasia))
            .ReverseMap();
    }
}
