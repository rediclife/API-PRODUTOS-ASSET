using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Entities
{
    [Table("TB_PRODUTO")]
    public class Produto : Notifies
    {
        [Column("PRD_ID")]
        public int Id { get; set; }

        [Column("PRD_CODIGO_EAN13")]
        [MaxLength(13)]
        public string CodigoEan { get; set; }

        [Column("PRD_DESCRICAO")]
        [MaxLength(150)]
        public string Descricao { get; set; }

        [Column("PRD_VALOR", TypeName = "decimal(8, 2)")]
        public decimal Valor { get; set; }

        [ForeignKey("CategoriaProduto")]
        [Column("PRD_CPR_ID")]
        public int CategoriaProdutoId { get; set; }

        [ForeignKey("ApplicationUser")]
        [Column("PRD_UserId")]
        public string UserId { get; set; }

        public virtual CategoriaProduto CategoriaProduto { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }
    }
}
