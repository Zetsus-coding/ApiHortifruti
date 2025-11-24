using ApiHortifruti.Domain;
using AutoMapper;

namespace ApiHortifruti.DTO.Profiles;

public class EntradaProfile : Profile
{
    public EntradaProfile()
    {
        CreateMap<PostEntradaDTO, Entrada>().ReverseMap();

        CreateMap<Entrada, GetEntradaSimplesDTO>()
            .ForMember(dest => dest.NomeFantasiaFornecedor, opt => opt.MapFrom(src => src.Fornecedor.NomeFantasia))
            .ForMember(dest => dest.Motivo, opt => opt.MapFrom(src => src.MotivoMovimentacao.TipoMovimentacao))
            .ReverseMap();

        CreateMap<Entrada, GetEntradaDTO>()
            .ForMember(dest => dest.NomeFantasiaFornecedor, opt => opt.MapFrom(src => src.Fornecedor.NomeFantasia))
            .ForMember(dest => dest.Motivo, opt => opt.MapFrom(src => src.MotivoMovimentacao.TipoMovimentacao))
            .ForMember(dest => dest.Itens, opt => opt.MapFrom(src => src.ItemEntrada));
    }
}
