namespace ApiHortifruti.DTO.Profiles;

using ApiHortifruti.Domain;
using AutoMapper;

public class FornecedorProfile : Profile
{
    public FornecedorProfile()
    {
        CreateMap<PostFornecedorDTO, Fornecedor>().ReverseMap();

        CreateMap<Fornecedor, FornecedorComListaProdutosDTO>()
            .ForMember(dest => dest.Produtos, opt => opt.MapFrom(src => src.FornecedorProduto));

        CreateMap<FornecedorProduto, GetProdutoComDetalhesFornecimentoDTO>()
            .ForMember(dest => dest.ProdutoId, opt => opt.MapFrom(src => src.ProdutoId))
            .ForMember(dest => dest.NomeProduto, opt => opt.MapFrom(src => src.Produto.Nome))
            .ForMember(dest => dest.DescricaoProduto, opt => opt.MapFrom(src => src.Produto.Descricao))
            .ForMember(dest => dest.Codigo, opt => opt.MapFrom(src => src.Produto.Codigo))
            .ForMember(dest => dest.PrecoAtual, opt => opt.MapFrom(src => src.Produto.Preco))
            .ForMember(dest => dest.NomeCategoria, opt => opt.MapFrom(src => src.Produto.Categoria.Nome))
            .ForMember(dest => dest.AbreviacaoUnidadeMedida, opt => opt.MapFrom(src => src.Produto.UnidadeMedida.Abreviacao));
    }
}
