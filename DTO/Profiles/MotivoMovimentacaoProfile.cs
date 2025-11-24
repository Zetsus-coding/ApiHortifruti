using ApiHortifruti.Domain;
using ApiHortifruti.DTO.MotivoMovimentacao;
using AutoMapper;

namespace ApiHortifruti.DTO.Profiles;

public class MotivoMovimentacaoProfile : Profile
{
    public MotivoMovimentacaoProfile()
    {
        CreateMap<PostMotivoMovimentacaoDTO, MotivoMovimentacao>().ReverseMap();
        CreateMap<PutMotivoMovimentacaoDTO, MotivoMovimentacao>().ReverseMap();
        CreateMap<MotivoMovimentacao, GetMotivoMovimentacaoDTO>().ReverseMap();
    }
}
