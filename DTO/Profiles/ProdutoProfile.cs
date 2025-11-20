using ApiHortifruti.Domain;
using AutoMapper;

namespace ApiHortifruti.DTO.Profiles;

public class ProdutoProfile : Profile
{
    public ProdutoProfile()
    {
        CreateMap<PostProdutoDTO, Produto>().ReverseMap();
        CreateMap<GetProdutoEstoqueCriticoDTO, Produto>().ReverseMap();
        CreateMap<PutProdutoDTO, Produto>().ReverseMap();
    }
}
