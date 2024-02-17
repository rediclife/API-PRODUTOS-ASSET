namespace WebAPI.DTOs
{
    public class ProdutoDTO
    {
        public int Id { get; set; }
        public string CodigoEan { get; set; }
        public string Descricao { get; set; }
        public decimal Valor { get; set; }
        public int CategoriaProdutoId { get; set; }
        public string? UserId { get; set; }
    }
}
