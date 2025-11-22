using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApiHortifruti.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "categoria",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    nome = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.id);
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateTable(
                name: "fornecedor",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    nome_fantasia = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    cadastro_pessoa = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    telefone = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    telefone_extra = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: true, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    email = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    data_registro = table.Column<DateOnly>(type: "date", nullable: false),
                    ativo = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.id);
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateTable(
                name: "funcionario",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    cpf = table.Column<string>(type: "varchar(14)", maxLength: 14, nullable: false, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    rg = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    nome = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    telefone = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    telefone_extra = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: true, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    email = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    conta_bancaria = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    agencia_bancaria = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ativo = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.id);
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateTable(
                name: "motivo_movimentacao",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    motivo = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ativo = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.id);
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateTable(
                name: "unidade_medida",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    nome = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    abreviacao = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: false, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.id);
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateTable(
                name: "entrada",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    fornecedor_id = table.Column<int>(type: "int", nullable: false),
                    motivo_movimentacao_id = table.Column<int>(type: "int", nullable: false),
                    preco_total = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false),
                    data_compra = table.Column<DateOnly>(type: "date", nullable: false),
                    numero_nota = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.id);
                    table.ForeignKey(
                        name: "fk_entrada_fornecedor1",
                        column: x => x.fornecedor_id,
                        principalTable: "fornecedor",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_entrada_motivo_movimentacao1",
                        column: x => x.motivo_movimentacao_id,
                        principalTable: "motivo_movimentacao",
                        principalColumn: "id");
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateTable(
                name: "saida",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    motivo_movimentacao_id = table.Column<int>(type: "int", nullable: false),
                    cadastro_cliente = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: true, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    valor_total = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false),
                    desconto = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    valor_desconto = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: true),
                    valor_final = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false),
                    data_saida = table.Column<DateOnly>(type: "date", nullable: false),
                    hora_saida = table.Column<TimeOnly>(type: "time", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.id);
                    table.ForeignKey(
                        name: "fk_saida_motivo_movimentacao1",
                        column: x => x.motivo_movimentacao_id,
                        principalTable: "motivo_movimentacao",
                        principalColumn: "id");
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateTable(
                name: "produto",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    categoria_id = table.Column<int>(type: "int", nullable: false),
                    unidade_medida_id = table.Column<int>(type: "int", nullable: false),
                    nome = table.Column<string>(type: "varchar(75)", maxLength: 75, nullable: false, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    codigo = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    descricao = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: true, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    preco = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false),
                    quantidade_atual = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false),
                    quantidade_minima = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false),
                    ativo = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.id);
                    table.ForeignKey(
                        name: "fk_produto_categoria1",
                        column: x => x.categoria_id,
                        principalTable: "categoria",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_produto_unidade_medida1",
                        column: x => x.unidade_medida_id,
                        principalTable: "unidade_medida",
                        principalColumn: "id");
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateTable(
                name: "fornecedor_produto",
                columns: table => new
                {
                    fornecedor_id = table.Column<int>(type: "int", nullable: false),
                    produto_id = table.Column<int>(type: "int", nullable: false),
                    codigo_fornecedor = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    data_registro = table.Column<DateOnly>(type: "date", nullable: false),
                    disponibilidade = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => new { x.fornecedor_id, x.produto_id })
                        .Annotation("MySql:IndexPrefixLength", new[] { 0, 0 });
                    table.ForeignKey(
                        name: "fk_fornecedor_has_produto_fornecedor",
                        column: x => x.fornecedor_id,
                        principalTable: "fornecedor",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_fornecedor_has_produto_produto1",
                        column: x => x.produto_id,
                        principalTable: "produto",
                        principalColumn: "id");
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateTable(
                name: "historico_produto",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    produto_id = table.Column<int>(type: "int", nullable: false),
                    preco_produto = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false),
                    data_alteracao = table.Column<DateOnly>(type: "date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.id);
                    table.ForeignKey(
                        name: "fk_historico_produto_produto1",
                        column: x => x.produto_id,
                        principalTable: "produto",
                        principalColumn: "id");
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateTable(
                name: "item_entrada",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    entrada_id = table.Column<int>(type: "int", nullable: false),
                    produto_id = table.Column<int>(type: "int", nullable: false),
                    quantidade = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false),
                    lote = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    preco_unitario = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false),
                    validade = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.id);
                    table.ForeignKey(
                        name: "fk_item_entrada_entrada1",
                        column: x => x.entrada_id,
                        principalTable: "entrada",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_item_entrada_produto1",
                        column: x => x.produto_id,
                        principalTable: "produto",
                        principalColumn: "id");
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateTable(
                name: "item_saida",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    produto_id = table.Column<int>(type: "int", nullable: false),
                    saida_id = table.Column<int>(type: "int", nullable: false),
                    quantidade = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false),
                    valor = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.id);
                    table.ForeignKey(
                        name: "fk_itens_saida_produto1",
                        column: x => x.produto_id,
                        principalTable: "produto",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_itens_saida_saida1",
                        column: x => x.saida_id,
                        principalTable: "saida",
                        principalColumn: "id");
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateIndex(
                name: "fk_entrada_fornecedor1_idx",
                table: "entrada",
                column: "fornecedor_id");

            migrationBuilder.CreateIndex(
                name: "fk_entrada_motivo_movimentacao1_idx",
                table: "entrada",
                column: "motivo_movimentacao_id");

            migrationBuilder.CreateIndex(
                name: "fk_fornecedor_has_produto_fornecedor_idx",
                table: "fornecedor_produto",
                column: "fornecedor_id");

            migrationBuilder.CreateIndex(
                name: "fk_fornecedor_has_produto_produto1_idx",
                table: "fornecedor_produto",
                column: "produto_id");

            migrationBuilder.CreateIndex(
                name: "fk_historico_produto_produto1_idx",
                table: "historico_produto",
                column: "produto_id");

            migrationBuilder.CreateIndex(
                name: "fk_item_entrada_entrada1_idx",
                table: "item_entrada",
                column: "entrada_id");

            migrationBuilder.CreateIndex(
                name: "fk_item_entrada_produto1_idx",
                table: "item_entrada",
                column: "produto_id");

            migrationBuilder.CreateIndex(
                name: "fk_itens_saida_produto1_idx",
                table: "item_saida",
                column: "produto_id");

            migrationBuilder.CreateIndex(
                name: "fk_itens_saida_saida1_idx",
                table: "item_saida",
                column: "saida_id");

            migrationBuilder.CreateIndex(
                name: "codigo_UNIQUE",
                table: "produto",
                column: "codigo",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "fk_produto_categoria1_idx",
                table: "produto",
                column: "categoria_id");

            migrationBuilder.CreateIndex(
                name: "fk_produto_unidade_medida1_idx",
                table: "produto",
                column: "unidade_medida_id");

            migrationBuilder.CreateIndex(
                name: "fk_saida_motivo_movimentacao1_idx",
                table: "saida",
                column: "motivo_movimentacao_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "fornecedor_produto");

            migrationBuilder.DropTable(
                name: "funcionario");

            migrationBuilder.DropTable(
                name: "historico_produto");

            migrationBuilder.DropTable(
                name: "item_entrada");

            migrationBuilder.DropTable(
                name: "item_saida");

            migrationBuilder.DropTable(
                name: "entrada");

            migrationBuilder.DropTable(
                name: "produto");

            migrationBuilder.DropTable(
                name: "saida");

            migrationBuilder.DropTable(
                name: "fornecedor");

            migrationBuilder.DropTable(
                name: "categoria");

            migrationBuilder.DropTable(
                name: "unidade_medida");

            migrationBuilder.DropTable(
                name: "motivo_movimentacao");
        }
    }
}
