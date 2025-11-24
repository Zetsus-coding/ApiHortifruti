using System.ComponentModel.DataAnnotations;
using ApiHortifruti.DTO.CustomAnnotation;
using ApiHortifruti.DTO.ItemEntrada;

public class PostEntradaDTO
{
    // ADICIONAR CAMPO DE TIPO DE MEDIDA (UNIDADE OU PESO)? SE FOR UNIDADE IMPEDIR VALORES DECIMAIS ENQUANTO QUE PESO PERMITE INTEIRO E DECIMAL
    // QUANTIDADES JÁ FORAM ALTERADAS PARA DECIMAL NO BANCO DE DADOS. QUANTIDADE MÁXIMA FOI REMOVIDA

    [Required(ErrorMessage = "Por favor, informe um fornecedor")]
    [Range(1, int.MaxValue, ErrorMessage = "Por favor, informe um fornecedor válido")]
    public int FornecedorId { get; set; }


    [Required(ErrorMessage = "Por favor, informe um motivo de movimentação")]
    [Range(1, int.MaxValue, ErrorMessage = "Por favor, informe um motivo de movimentação válido")]
    public int MotivoMovimentacaoId { get; set; }


    [Required(ErrorMessage = "O valor da entrada (preço total) é obrigatório")]
    [ValidacaoCampoPreco]
    public decimal PrecoTotal { get; set; }


    [Required(ErrorMessage = "A data da compra/entrada é obrigatória")]
    [DataNaoFutura] // Annotation customizada que impede datas futuras. ErrorMessage padrão definido na annotation, mas que pode ser sobrescrito aqui se achar necessário
    public DateOnly DataCompra { get; set; } // DateOnly(Ano, mês, dia)


    [StringLength(30, ErrorMessage = "A descrição do produto não pode exceder 30 caracteres")] // Tamanho máximo?
    public string NumeroNota { get; set; } = null!; // Não pode ser repetida por fornecedor // DEVE SER OBRIGATÓRIO?

    // public string NotaFiscal { get; set; } = null!; // Removido

    [Required(ErrorMessage = "É obrigatório informar o(s) item(ns) na(da) entrada")]
    [MinLength(1, ErrorMessage = "É obrigatório informar ao menos um item na entrada")]
    public List<ItemEntradaDTO> ItemEntrada { get; set; } = new List<ItemEntradaDTO>();
}