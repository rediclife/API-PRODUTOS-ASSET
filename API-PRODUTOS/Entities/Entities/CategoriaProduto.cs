using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Entities
{
    [Table("TB_CATEGORIA_PRODUTO")]
    public class CategoriaProduto : Notifies
    {
        [Column("CPR_ID")]
        public int Id { get; set; }

        [Column("CPR_DESCRICAO")]
        [MaxLength(150)]
        public string Descricao { get; set; }

        [ForeignKey("ApplicationUser")]
        [Column("CPR_UserId")]
        public string UserId { get; set; }


        public virtual ApplicationUser ApplicationUser { get; set; }

        public virtual List<Produto> Produtos { get; set; }
    }
}
