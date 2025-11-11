using ApiHortifruti.Domain;
using Microsoft.EntityFrameworkCore;

namespace ApiHortifruti;

public partial class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    
   public virtual DbSet<Categoria> Categoria { get; set; }

    public virtual DbSet<Entrada> Entrada { get; set; }

    public virtual DbSet<Fornecedor> Fornecedor { get; set; }

    public virtual DbSet<FornecedorProduto> FornecedorProduto { get; set; }

    public virtual DbSet<Funcionario> Funcionario { get; set; }

    public virtual DbSet<HistoricoProduto> HistoricoProduto { get; set; }

    public virtual DbSet<ItemEntrada> ItemEntrada { get; set; }

    public virtual DbSet<ItemSaida> ItemSaida { get; set; }

    public virtual DbSet<MotivoMovimentacao> MotivoMovimentacao { get; set; }

    public virtual DbSet<Produto> Produto { get; set; }

    public virtual DbSet<Saida> Saida { get; set; }

    public virtual DbSet<UnidadeMedida> UnidadeMedida { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_0900_ai_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<Categoria>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("categoria");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Nome)
                .HasMaxLength(50)
                .HasColumnName("nome");
        });

        modelBuilder.Entity<Entrada>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("entrada");

            entity.HasIndex(e => e.FornecedorId, "fk_entrada_fornecedor1_idx");

            entity.HasIndex(e => e.MotivoMovimentacaoId, "fk_entrada_motivo_movimentacao1_idx");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.DataCompra).HasColumnName("data_compra");
            entity.Property(e => e.FornecedorId).HasColumnName("fornecedor_id");
            entity.Property(e => e.MotivoMovimentacaoId).HasColumnName("motivo_movimentacao_id");
            entity.Property(e => e.NumeroNota)
                .HasMaxLength(30)
                .HasColumnName("numero_nota");
            entity.Property(e => e.PrecoTotal)
                .HasPrecision(10, 2)
                .HasColumnName("preco_total");

            entity.HasOne(d => d.Fornecedor).WithMany(p => p.Entrada)
                .HasForeignKey(d => d.FornecedorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_entrada_fornecedor1");

            entity.HasOne(d => d.MotivoMovimentacao).WithMany(p => p.Entrada)
                .HasForeignKey(d => d.MotivoMovimentacaoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_entrada_motivo_movimentacao1");
        });

        modelBuilder.Entity<Fornecedor>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("fornecedor");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Ativo).HasColumnName("ativo");
            entity.Property(e => e.CadastroPessoa)
                .HasMaxLength(20)
                .HasColumnName("cadastro_pessoa");
            entity.Property(e => e.DataRegistro).HasColumnName("data_registro");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .HasColumnName("email");
            entity.Property(e => e.NomeFantasia)
                .HasMaxLength(100)
                .HasColumnName("nome_fantasia");
            entity.Property(e => e.Telefone)
                .HasMaxLength(20)
                .HasColumnName("telefone");
            entity.Property(e => e.TelefoneExtra)
                .HasMaxLength(20)
                .HasColumnName("telefone_extra");
        });

        modelBuilder.Entity<FornecedorProduto>(entity =>
        {
            entity.HasKey(e => new { e.FornecedorId, e.ProdutoId })
                .HasName("PRIMARY")
                .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

            entity.ToTable("fornecedor_produto");

            entity.HasIndex(e => e.FornecedorId, "fk_fornecedor_has_produto_fornecedor_idx");

            entity.HasIndex(e => e.ProdutoId, "fk_fornecedor_has_produto_produto1_idx");

            entity.Property(e => e.FornecedorId).HasColumnName("fornecedor_id");
            entity.Property(e => e.ProdutoId).HasColumnName("produto_id");
            entity.Property(e => e.CodigoFornecedor)
                .HasMaxLength(50)
                .HasColumnName("codigo_fornecedor");
            entity.Property(e => e.DataRegistro).HasColumnName("data_registro");
            entity.Property(e => e.Disponibilidade).HasColumnName("disponibilidade");

            entity.HasOne(d => d.Fornecedor).WithMany(p => p.FornecedorProduto)
                .HasForeignKey(d => d.FornecedorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_fornecedor_has_produto_fornecedor");

            entity.HasOne(d => d.Produto).WithMany(p => p.FornecedorProduto)
                .HasForeignKey(d => d.ProdutoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_fornecedor_has_produto_produto1");
        });

        modelBuilder.Entity<Funcionario>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("funcionario");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AgenciaBancaria)
                .HasMaxLength(20)
                .HasColumnName("agencia_bancaria");
            entity.Property(e => e.Ativo).HasColumnName("ativo");
            entity.Property(e => e.ContaBancaria)
                .HasMaxLength(20)
                .HasColumnName("conta_bancaria");
            entity.Property(e => e.Cpf)
                .HasMaxLength(14)
                .HasColumnName("cpf");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .HasColumnName("email");
            entity.Property(e => e.Nome)
                .HasMaxLength(100)
                .HasColumnName("nome");
            entity.Property(e => e.Rg)
                .HasMaxLength(20)
                .HasColumnName("rg");
            entity.Property(e => e.Telefone)
                .HasMaxLength(20)
                .HasColumnName("telefone");
            entity.Property(e => e.TelefoneExtra)
                .HasMaxLength(20)
                .HasColumnName("telefone_extra");
        });

        modelBuilder.Entity<HistoricoProduto>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("historico_produto");

            entity.HasIndex(e => e.ProdutoId, "fk_historico_produto_produto1_idx");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.DataAlteracao).HasColumnName("data_alteracao");
            entity.Property(e => e.PrecoProduto)
                .HasPrecision(10, 2)
                .HasColumnName("preco_produto");
            entity.Property(e => e.ProdutoId).HasColumnName("produto_id");

            entity.HasOne(d => d.Produto).WithMany(p => p.HistoricoProduto)
                .HasForeignKey(d => d.ProdutoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_historico_produto_produto1");
        });

        modelBuilder.Entity<ItemEntrada>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("item_entrada");

            entity.HasIndex(e => e.EntradaId, "fk_item_entrada_entrada1_idx");

            entity.HasIndex(e => e.ProdutoId, "fk_item_entrada_produto1_idx");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.EntradaId).HasColumnName("entrada_id");
            entity.Property(e => e.Lote)
                .HasMaxLength(50)
                .HasColumnName("lote");
            entity.Property(e => e.PrecoUnitario)
                .HasPrecision(10, 2)
                .HasColumnName("preco_unitario");
            entity.Property(e => e.ProdutoId).HasColumnName("produto_id");
            entity.Property(e => e.Quantidade)
                .HasPrecision(10, 2)
                .HasColumnName("quantidade");
            entity.Property(e => e.Validade)
                .HasMaxLength(50)
                .HasColumnName("validade");

            entity.HasOne(d => d.Entrada).WithMany(p => p.ItemEntrada)
                .HasForeignKey(d => d.EntradaId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_item_entrada_entrada1");

            entity.HasOne(d => d.Produto).WithMany(p => p.ItemEntrada)
                .HasForeignKey(d => d.ProdutoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_item_entrada_produto1");
        });

        modelBuilder.Entity<ItemSaida>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("item_saida");

            entity.HasIndex(e => e.ProdutoId, "fk_itens_saida_produto1_idx");

            entity.HasIndex(e => e.SaidaId, "fk_itens_saida_saida1_idx");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ProdutoId).HasColumnName("produto_id");
            entity.Property(e => e.Quantidade)
                .HasPrecision(10, 2)
                .HasColumnName("quantidade");
            entity.Property(e => e.SaidaId).HasColumnName("saida_id");
            entity.Property(e => e.Valor)
                .HasPrecision(10, 2)
                .HasColumnName("valor");

            entity.HasOne(d => d.Produto).WithMany(p => p.ItemSaida)
                .HasForeignKey(d => d.ProdutoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_itens_saida_produto1");

            entity.HasOne(d => d.Saida).WithMany(p => p.ItemSaida)
                .HasForeignKey(d => d.SaidaId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_itens_saida_saida1");
        });

        modelBuilder.Entity<MotivoMovimentacao>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("motivo_movimentacao");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Ativo).HasColumnName("ativo");
            entity.Property(e => e.TipoMovimentacao)
                .HasMaxLength(20)
                .HasColumnName("tipo_movimentacao");
        });

        modelBuilder.Entity<Produto>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("produto");

            entity.HasIndex(e => e.Codigo, "codigo_UNIQUE").IsUnique();

            entity.HasIndex(e => e.CategoriaId, "fk_produto_categoria1_idx");

            entity.HasIndex(e => e.UnidadeMedidaId, "fk_produto_unidade_medida1_idx");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Ativo).HasColumnName("ativo");
            entity.Property(e => e.CategoriaId).HasColumnName("categoria_id");
            entity.Property(e => e.Codigo)
                .HasMaxLength(50)
                .HasColumnName("codigo");
            entity.Property(e => e.Descricao)
                .HasMaxLength(150)
                .HasColumnName("descricao");
            entity.Property(e => e.Nome)
                .HasMaxLength(75)
                .HasColumnName("nome");
            entity.Property(e => e.Preco)
                .HasPrecision(10, 2)
                .HasColumnName("preco");
            entity.Property(e => e.QuantidadeAtual)
                .HasPrecision(10, 2)
                .HasColumnName("quantidade_atual");
            entity.Property(e => e.QuantidadeMinima)
                .HasPrecision(10, 2)
                .HasColumnName("quantidade_minima");
            entity.Property(e => e.UnidadeMedidaId).HasColumnName("unidade_medida_id");

            entity.HasOne(d => d.Categoria).WithMany(p => p.Produto)
                .HasForeignKey(d => d.CategoriaId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_produto_categoria1");

            entity.HasOne(d => d.UnidadeMedida).WithMany(p => p.Produto)
                .HasForeignKey(d => d.UnidadeMedidaId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_produto_unidade_medida1");
        });

        modelBuilder.Entity<Saida>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("saida");

            entity.HasIndex(e => e.MotivoMovimentacaoId, "fk_saida_motivo_movimentacao1_idx");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CadastroCliente)
                .HasMaxLength(20)
                .HasColumnName("cadastro_cliente");
            entity.Property(e => e.DataSaida).HasColumnName("data_saida");
            entity.Property(e => e.Desconto).HasColumnName("desconto");
            entity.Property(e => e.HoraSaida)
                .HasColumnType("time")
                .HasColumnName("hora_saida");
            entity.Property(e => e.MotivoMovimentacaoId).HasColumnName("motivo_movimentacao_id");
            entity.Property(e => e.ValorDesconto)
                .HasPrecision(10, 2)
                .HasColumnName("valor_desconto");
            entity.Property(e => e.ValorFinal)
                .HasPrecision(10, 2)
                .HasColumnName("valor_final");
            entity.Property(e => e.ValorTotal)
                .HasPrecision(10, 2)
                .HasColumnName("valor_total");

            entity.HasOne(d => d.MotivoMovimentacao).WithMany(p => p.Saida)
                .HasForeignKey(d => d.MotivoMovimentacaoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_saida_motivo_movimentacao1");
        });

        modelBuilder.Entity<UnidadeMedida>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("unidade_medida");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Abreviacao)
                .HasMaxLength(10)
                .HasColumnName("abreviacao");
            entity.Property(e => e.Nome)
                .HasMaxLength(50)
                .HasColumnName("nome");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}