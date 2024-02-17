using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestApiGestaoProdutos.DTO
{
    public class DTOProduto
    {
        public int Id { get; set; }
        public string CodigoEan { get; set; }
        public string Descricao { get; set; }
        public decimal Valor { get; set; }
        public int CategoriaProdutoId { get; set; }
        public string UserId { get; set; }         
    }
}
