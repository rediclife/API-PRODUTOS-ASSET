using Domain.Interfaces;
using Entities.Entities;
using Infrastructure.Configuration;
using Infrastructure.Repository.Generics;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repository.Repositories
{
    public class RepositoryProduto : RepositoyGenerics<Produto>, IProduto
    {
        private readonly DbContextOptions<ContextBase> _OptionsBuilder;
        public RepositoryProduto()
        {
            _OptionsBuilder = new DbContextOptions<ContextBase>();
        }

        public async Task<List<Produto>> ListarProdutos(Expression<Func<Produto, bool>> predicate)
        {
            using (var banco = new ContextBase(_OptionsBuilder))
            {
                return await banco.Produto.Where(predicate).AsNoTracking().ToListAsync();
            }
        }

        public async Task<ICollection<Produto>> ObterProdutoPorIdComInclude(int Id)
        {
            using (var banco = new ContextBase(_OptionsBuilder))
            {
                return await banco.Produto.Where(c => c.Id == Id).Include("CategoriaProduto").Include("ApplicationUser").ToListAsync();
            }
        }
    }
}
