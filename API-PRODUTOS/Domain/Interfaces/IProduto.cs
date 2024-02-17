using Domain.Interfaces.Generics;
using Entities.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IProduto : IGeneric<Produto>
    {
        Task<List<Produto>> ListarProdutos(Expression<Func<Produto, bool>> predicate);
        Task<ICollection<Produto>> ObterProdutoPorIdComInclude(int Id);
    }
}
