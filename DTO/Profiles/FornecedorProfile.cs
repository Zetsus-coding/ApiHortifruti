using ApiHortifruti.Domain;
using AutoMapper;

public class FornecedorProfile : Profile
{
    public FornecedorProfile()
    {
        CreateMap<PostFornecedorDTO, Fornecedor>().ReverseMap();
        CreateMap<Fornecedor, GetFornecedorDTO>().ReverseMap();
        CreateMap<PutFornecedorDTO, Fornecedor>().ReverseMap();

        CreateMap<Fornecedor, FornecedorComListaProdutosDTO>()
            .ForMember(dest => dest.Produtos, opt => opt.MapFrom(src => src.FornecedorProduto));

        CreateMap<GetFornecedoresComDetalhesFornecimentoDTO, Fornecedor>();
    }
}
