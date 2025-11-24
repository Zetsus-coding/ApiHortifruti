using ApiHortifruti.Domain;
using AutoMapper;

namespace ApiHortifruti.DTO.Profiles;

public class SaidaProfile : Profile
{
    public SaidaProfile()
    {
        CreateMap<PostSaidaDTO, Saida>().ReverseMap();

        CreateMap<Saida, GetSaidaSimplesDTO>()
            .ForMember(dest => dest.Motivo, opt => opt.MapFrom(src => src.MotivoMovimentacao.TipoMovimentacao))
            .ReverseMap();

        CreateMap<Saida, GetSaidaDTO>()
            .ForMember(dest => dest.Motivo, opt => opt.MapFrom(src => src.MotivoMovimentacao.TipoMovimentacao))
            .ForMember(dest => dest.Itens, opt => opt.MapFrom(src => src.ItemSaida));
    }
}
