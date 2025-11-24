using ApiHortifruti.Domain;
using ApiHortifruti.DTO.FornecedorProduto;
using AutoMapper;

namespace ApiHortifruti.DTO.Profiles;

public class FornecedorProdutoProfile : Profile
 {
     public FornecedorProdutoProfile()
    {
        CreateMap<PostFornecedorProdutoDTO, FornecedorProduto>().ReverseMap();
        CreateMap<PutFornecedorProdutoDTO, FornecedorProduto>().ReverseMap();
        CreateMap<FornecedorProduto, GetFornecedorProdutoDTO>().ReverseMap();

        CreateMap<Fornecedor, FornecedorComListaProdutosDTO>()
            // Mapeamento das propriedades de fornecedor
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.NomeFantasia, opt => opt.MapFrom(src => src.NomeFantasia))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
            .ForMember(dest => dest.Telefone, opt => opt.MapFrom(src => src.Telefone))
            
            // Mapeamento da coleção "aninhada" 
            // (o automapper usará o profile do inner dto da lista de produtos com informações de fornecimento)
            .ForMember(dest => dest.Produtos, opt => opt.MapFrom(src => src.FornecedorProduto));


        // Inner dto de buscar lista de produtos com detalhes de fornecimento
        CreateMap<FornecedorProduto, GetProdutoComDetalhesFornecimentoDTO>()
            // Mapeamento das propriedades do relacionamento (FornecedorProduto)
            .ForMember(dest => dest.CodigoFornecedor, opt => opt.MapFrom(src => src.CodigoFornecedor))
            .ForMember(dest => dest.DataRegistro, opt => opt.MapFrom(src => src.DataRegistro))
            .ForMember(dest => dest.Disponibilidade, opt => opt.MapFrom(src => src.Disponibilidade))

            // Mapeamento das propriedades "aninhadas" (Produto, Categoria, UnidadeMedida etc.(?))
            .ForMember(dest => dest.ProdutoId, opt => opt.MapFrom(src => src.ProdutoId))
            .ForMember(dest => dest.NomeProduto, opt => opt.MapFrom(src => src.Produto.Nome))
            .ForMember(dest => dest.DescricaoProduto, opt => opt.MapFrom(src => src.Produto.Descricao))
            .ForMember(dest => dest.PrecoAtual, opt => opt.MapFrom(src => src.Produto.Preco))
            .ForMember(dest => dest.Codigo, opt => opt.MapFrom(src => src.Produto.Codigo))
            .ForMember(dest => dest.CategoriaDTO, opt => opt.MapFrom(src => src.Produto.Categoria))
            .ForMember(dest => dest.UnidadeMedidaDTO, opt => opt.MapFrom(src => src.Produto.UnidadeMedida));

        CreateMap<FornecedorProduto, GetFornecedoresComDetalhesFornecimentoDTO>()
            .ForMember(dest => dest.NomeFantasia, opt => opt.MapFrom(src => src.Fornecedor.NomeFantasia))
            .ForMember(dest => dest.Telefone, opt => opt.MapFrom(src => src.Fornecedor.Telefone))
            .ForMember(dest => dest.TelefoneExtra, opt => opt.MapFrom(src => src.Fornecedor.TelefoneExtra))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Fornecedor.Email))
            .ForMember(dest => dest.Ativo, opt => opt.MapFrom(src => src.Fornecedor.Ativo))
            .ForMember(dest => dest.CodigoFornecedor, opt => opt.MapFrom(src => src.CodigoFornecedor))
            .ForMember(dest => dest.Disponibilidade, opt => opt.MapFrom(src => src.Disponibilidade));
    }
 }