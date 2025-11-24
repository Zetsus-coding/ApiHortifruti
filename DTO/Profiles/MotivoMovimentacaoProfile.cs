using ApiHortifruti.Domain;
using AutoMapper;

namespace ApiHortifruti.DTO.Profiles;

public class MotivoMovimentacaoProfile : Profile
{
    public MotivoMovimentacaoProfile()
    {
        CreateMap<PostMotivoMovimentacaoDTO, MotivoMovimentacao>()
            .ForMember(dest => dest.TipoMovimentacao, opt => opt.MapFrom(src => src.Motivo))
            .ReverseMap();

        CreateMap<PutMotivoMovimentacaoDTO, MotivoMovimentacao>()
            .ForMember(dest => dest.TipoMovimentacao, opt => opt.MapFrom(src => src.Motivo))
            .ReverseMap();

        CreateMap<MotivoMovimentacao, GetMotivoMovimentacaoDTO>()
            .ForMember(dest => dest.Motivo, opt => opt.MapFrom(src => src.TipoMovimentacao))
            .ReverseMap();
    }
}
