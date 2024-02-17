using Domain.Interfaces;
using Entities.Entities;
using Infrastructure.Configuration;
using Infrastructure.Repository.Generics;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repository.Repositories
{
    public class RepositoryCategoriaProduto : RepositoyGenerics<CategoriaProduto>, ICategoriaProduto
    {
        private readonly DbContextOptions<ContextBase> _OptionsBuilder;
        public RepositoryCategoriaProduto()
        {
            _OptionsBuilder = new DbContextOptions<ContextBase>();
        }

        public async Task<CategoriaProduto> ObterCategoriaPorIdComInclude(int Id)
        {
            using (var banco = new ContextBase(_OptionsBuilder))
            {
                return await banco.CategoriaProduto.Include("Produtos").Include("ApplicationUser").FirstOrDefaultAsync(c => c.Id == Id);
            }
        }
    }
}
