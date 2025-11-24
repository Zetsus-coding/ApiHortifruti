using ApiHortifruti.Domain;
using ApiHortifruti.DTO.HistoricoProduto;
using AutoMapper;

namespace ApiHortifruti.DTO.Profiles;

public class HistoricoProdutoProfile : Profile
{
    public HistoricoProdutoProfile()
    {
        CreateMap<HistoricoProduto, GetHistoricoProdutoDTO>().ReverseMap();
    }
}
