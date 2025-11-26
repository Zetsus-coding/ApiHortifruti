using ApiHortifruti.Domain;
using ApiHortifruti.DTO.HistoricoProduto;
using ApiHortifruti.DTO.Produto;
using AutoMapper;

namespace ApiHortifruti.DTO.Profiles;

public class HistoricoProdutoProfile : Profile
{
    public HistoricoProdutoProfile()
    {
        CreateMap<Produto, ProdutoSimplesDTO>();
        CreateMap<HistoricoProduto, GetHistoricoProdutoDTO>()
            .ForMember(dest => dest.Produto, opt => opt.MapFrom(src => src.Produto));

        CreateMap<GetHistoricoProdutoDTO, HistoricoProduto>();
    }
}
