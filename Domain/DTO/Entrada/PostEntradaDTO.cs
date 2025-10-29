using System.ComponentModel.DataAnnotations;
using ApiHortifruti.Domain.DTO.CustomAnnotation;
using ApiHortifruti.Domain.DTO.Item_entradaDTO;

public class PostEntradaDTO
{
    // ADICIONAR CAMPO DE TIPO DE MEDIDA (UNIDADE OU PESO)? SE FOR UNIDADE IMPEDIR VALORES DECIMAIS ENQUANTO QUE PESO PERMITE INTEIRO E DECIMAL
    // QUANTIDADES JÁ FORAM ALTERADAS PARA DECIMAL NO BANCO DE DADOS. QUANTIDADE MÁXIMA FOI REMOVIDA

    [Required(ErrorMessage = "Por favor, informe um fornecedor")]
    public int FornecedorId { get; set; }


    [Required(ErrorMessage = "Por favor, informe um motivo de movimentação")]
    public int MotivoMovimentacaoId { get; set; }


    [Required(ErrorMessage = "O valor da entrada (preço total) é obrigatório")]
    //[RegularExpression(@"^\d{1,8}(\.\d{1,2})?$", ErrorMessage = "Formato de preço inválido")]
    [Range(0, double.MaxValue, MinimumIsExclusive = true, ErrorMessage = "O preço deve ser maior que zero")] // Valor maior que zero
    public decimal PrecoTotal { get; set; }


    [Required(ErrorMessage = "A data da compra/entrada é obrigatória")]
    [DataNaoFutura] // Annotation customizada que impede datas futuras. ErrorMessage padrão definido na annotation, mas que pode ser sobrescrito aqui se achar necessário
    public DateOnly DataCompra { get; set; } // DateOnly(Ano, mês, dia)


    [StringLength(30, ErrorMessage = "A descrição do produto não pode exceder 30 caracteres")] // Tamanho máximo?
    public string NumeroNota { get; set; } = null!; // Não pode ser repetida por fornecedor // DEVE SER OBRIGATÓRIO?
    
    // public string NotaFiscal { get; set; } = null!; // Removido

    [Required(ErrorMessage = "É obrigatório adicionar ao menos um item na entrada")]
    public List<Item_entradaDTO> ItemEntrada { get; set; } = new List<Item_entradaDTO>();
}