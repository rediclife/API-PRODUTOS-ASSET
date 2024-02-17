using Domain.Interfaces.Generics;
using Entities.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface ICategoriaProduto : IGeneric<CategoriaProduto>
    {
        Task<CategoriaProduto> ObterCategoriaPorIdComInclude(int Id);
    }
}
