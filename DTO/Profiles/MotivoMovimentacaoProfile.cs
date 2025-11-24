using ApiHortifruti.Domain;
using AutoMapper;

public class MotivoMovimentacaoProfile : Profile
{
    public MotivoMovimentacaoProfile()
    {
        CreateMap<PostMotivoMovimentacaoDTO, MotivoMovimentacao>().ReverseMap();
    }
}