using Entities.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces.InterfaceServices
{
    public interface IServiceCategoriaProduto
    {
        Task Adicionar(CategoriaProduto Objeto);
        Task Atualizar(CategoriaProduto Objeto);
        Task Excluir(CategoriaProduto Objeto);
        Task<CategoriaProduto> ObterPorId(int Id);
    }
}
