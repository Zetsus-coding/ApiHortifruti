using ApiHortifruti.Domain;
using AutoMapper;

namespace Domain.DTO.Profiles;

public class MotivoMovimentacaoProfile : Profile
{
    public MotivoMovimentacaoProfile()
    {
        CreateMap<PostMotivoMovimentacaoDTO, MotivoMovimentacao>().ReverseMap();
    }
}