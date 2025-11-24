using ApiHortifruti.Domain;
using AutoMapper;

public class ProdutoProfile : Profile
{
    public ProdutoProfile()
    {
        CreateMap<PostProdutoDTO, Produto>().ReverseMap();
        CreateMap<GetProdutoEstoqueCriticoDTO, Produto>();
        CreateMap<GetProdutoDTO, Produto>().ReverseMap();
        
        CreateMap<PutProdutoDTO, Produto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ReverseMap();

        CreateMap<Produto, ProdutoComListaDeFornecedoresDTO>()
            .ForMember(dest => dest.Fornecedores, opt => opt.MapFrom(src => src.FornecedorProduto));
            
    }
}
