using ApiHortifruti.DTO.Produto;

namespace ApiHortifruti.DTO.HistoricoProduto
{
    public class GetHistoricoProdutoDTO
    {
        public int Id { get; set; }
        public int ProdutoId { get; set; }
        public decimal PrecoProduto { get; set; }
        public DateOnly DataAlteracao { get; set; }
        public ProdutoSimplesDTO? Produto { get; set; }
    }
}