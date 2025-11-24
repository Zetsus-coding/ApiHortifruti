using AutoMapper;
using ApiHortifruti.Domain;
using ApiHortifruti.DTO.ItemEntrada;
using ApiHortifruti.DTO.PutFuncionarioDTO;
using ApiHortifruti.DTO.ItemSaida;

namespace ApiHortifruti.Configuration;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        // --- Mapeamentos de ENTRADA ---
        CreateMap<PostEntradaDTO, Entrada>();

        CreateMap<Entrada, GetEntradaSimplesDTO>()
            // Mapeia o NomeFantasia do Fornecedor (Entidade -> DTO)
            .ForMember(dest => dest.NomeFantasiaFornecedor, opt => opt.MapFrom(src => src.Fornecedor.NomeFantasia))
            // Mapeia o Motivo (Entidade -> DTO)
            .ForMember(dest => dest.Motivo, opt => opt.MapFrom(src => src.MotivoMovimentacao.Motivo));

        // --- Mapeamentos de FORNECEDOR ---
        CreateMap<PostFornecedorDTO, Fornecedor>();

        // Correção para o erro 500 no teste de Fornecedor
        CreateMap<Fornecedor, FornecedorComListaProdutosDTO>()
            .ForMember(dest => dest.Produtos, opt => opt.MapFrom(src => src.FornecedorProduto));

        // --- Mapeamentos de ITEM ENTRADA ---
        CreateMap<ItemEntradaDTO, ItemEntrada>();

        // --- Outros Mapeamentos (Categorias, Produtos, etc) ---
        CreateMap<PostCategoriaDTO, Categoria>();
        CreateMap<PutCategoriaDTO, Categoria>();

        CreateMap<PostFornecedorProdutoDTO, FornecedorProduto>();

        // ... dentro do construtor do AutoMapperProfile

        // Mapeamento Principal (Já sugerido antes)
        CreateMap<Fornecedor, FornecedorComListaProdutosDTO>()
            .ForMember(dest => dest.Produtos, opt => opt.MapFrom(src => src.FornecedorProduto));

        // Ensina a converter o relacionamento (Tabela meio) para o DTO de detalhe do produto
        CreateMap<FornecedorProduto, GetProdutoComDetalhesFornecimentoDTO>()
            .ForMember(dest => dest.ProdutoId, opt => opt.MapFrom(src => src.ProdutoId))
            .ForMember(dest => dest.NomeProduto, opt => opt.MapFrom(src => src.Produto.Nome))
            .ForMember(dest => dest.PrecoAtual, opt => opt.MapFrom(src => src.Produto.Preco))
            .ForMember(dest => dest.CodigoFornecedor, opt => opt.MapFrom(src => src.CodigoFornecedor))
            .ForMember(dest => dest.Disponibilidade, opt => opt.MapFrom(src => src.Disponibilidade));
        // Mapeamento de Funcionario
        CreateMap<PostFuncionarioDTO, Funcionario>();
        CreateMap<PutFuncionarioDTO, Funcionario>();

        // Mapeamento de Motivo
        CreateMap<PostMotivoMovimentacaoDTO, MotivoMovimentacao>();
        CreateMap<PutMotivoMovimentacaoDTO, MotivoMovimentacao>();

        // --- PRODUTO ---
        CreateMap<PostProdutoDTO, Produto>();
        CreateMap<PutProdutoDTO, Produto>();
        CreateMap<Produto, GetProdutoEstoqueCriticoDTO>(); // Para a rota de estoque crítico

        // --- SAÍDA ---
        CreateMap<PostSaidaDTO, Saida>();
        CreateMap<ItemSaidaDTO, ItemSaida>(); // Garante que os itens sejam mapeados

        // --- UNIDADE DE MEDIDA ---
        CreateMap<PostUnidadeMedidaDTO, UnidadeMedida>();
        CreateMap<PutUnidadeMedidaDTO, UnidadeMedida>();
    }
}